// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Trace;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Promotes Temporary Variables to Virtual Registers
	/// </summary>
	public class PromoteTemporaryVariables : BaseMethodCompilerStage
	{
		private Counter TemporariesPromoted = new Counter("PromoteTemporaryVariables.TemporariesPromoted");

		private TraceLog trace;

		protected override void Initialize()
		{
			Register(TemporariesPromoted);
		}

		protected override void Setup()
		{
			trace = CreateTraceLog(6);
		}

		protected override void Run()
		{
			foreach (var operand in MethodCompiler.LocalStack)
			{
				Debug.Assert(operand.IsStackLocal);

				if (CanPromote(operand))
				{
					Promote(operand);
				}
			}
		}

		protected bool CanPromote(Operand operand)
		{
			trace?.Log($"Check: {operand}");

			if (operand.Uses.Count == 0)
			{
				trace?.Log($"No uses: {operand}");
				return false;
			}

			if (MosaTypeLayout.IsStoredOnStack(operand.Type))
			{
				trace?.Log($"incompatible type: {operand}");
				return false;
			}

			foreach (var node in operand.Uses)
			{
				if (node.Instruction == IRInstruction.AddressOf)
				{
					Debug.Assert(node.Operand1 == operand);

					foreach (var node2 in node.Result.Uses)
					{
						if (!Check(node2))
						{
							return false;
						}
					}
					continue;
				}
				else if (node.Instruction == IRInstruction.LoadInt32 || node.Instruction == IRInstruction.LoadInt64)
				{
					Debug.Assert(node.Operand2 == operand);
					continue;
				}
				else if (node.Instruction == IRInstruction.StoreInt32 || node.Instruction == IRInstruction.StoreInt64)
				{
					Debug.Assert(node.Operand2 == operand);
					continue;
				}

				trace?.Log($"A-No: {node}");
				return false;
			}

			return true;
		}

		private bool Check(InstructionNode node)
		{
			if (node.Instruction == IRInstruction.StoreInt32 || node.Instruction == IRInstruction.StoreInt64)
			{
				// check offset
				return true;
			}
			else if (node.Instruction == IRInstruction.MemorySet)
			{
				if (node.Operand3.ConstantUnsignedLongInteger == 4)
				{
					return true;
				}
				else if (node.Operand3.ConstantUnsignedLongInteger == 8)
				{
					return true;
				}

				trace?.Log($"B-No: {node}");
				return false;
			}
			else if (node.Instruction == IRInstruction.MoveInt32 || node.Instruction == IRInstruction.MoveInt64)
			{
				foreach (var node2 in node.Result.Uses)
				{
					if (!Check(node2))
					{
						trace?.Log($"C-No (parent): {node}");
						trace?.Log($"C-No: {node2}");
						return false;
					}
				}

				return true;
			}
			else if (node.Instruction == IRInstruction.LoadInt32 || node.Instruction == IRInstruction.LoadInt64)
			{
				// check offset
				return true;
			}

			trace?.Log($"D-No: {node}");
			return false;
		}

		protected void Promote(Operand operand)
		{
			var virtualRegister = AllocateVirtualRegister(operand.Type);
			TemporariesPromoted++;

			trace?.Log($"VR: {virtualRegister}");

			foreach (var node in operand.Uses.ToArray())
			{
				if (node.Instruction == IRInstruction.AddressOf)
				{
					foreach (var node2 in node.Result.Uses.ToArray())
					{
						Promote(node2, virtualRegister);
					}
				}
				else if (node.Instruction == IRInstruction.LoadInt32)
				{
					trace?.Log($"BEFORE:\t{node}");
					node.SetInstruction(IRInstruction.MoveInt32, node.Result, virtualRegister);
					trace?.Log($"AFTER: \t{node}");
				}
				else if (node.Instruction == IRInstruction.LoadInt64)
				{
					trace?.Log($"BEFORE:\t{node}");
					node.SetInstruction(IRInstruction.MoveInt64, node.Result, virtualRegister);
					trace?.Log($"AFTER: \t{node}");
				}
				else if (node.Instruction == IRInstruction.StoreInt32)
				{
					trace?.Log($"BEFORE:\t{node}");
					node.SetInstruction(IRInstruction.MoveInt32, virtualRegister, node.Operand3);
					trace?.Log($"AFTER: \t{node}");
				}
				else if (node.Instruction == IRInstruction.StoreInt64)
				{
					trace?.Log($"BEFORE:\t{node}");
					node.SetInstruction(IRInstruction.MoveInt64, virtualRegister, node.Operand3);
					trace?.Log($"AFTER: \t{node}");
				}
			}
		}

		private void Promote(InstructionNode node, Operand virtualRegister)
		{
			if (node.Instruction == IRInstruction.MoveInt32 || node.Instruction == IRInstruction.MoveInt64)
			{
				foreach (var node2 in node.Result.Uses.ToArray())
				{
					Promote(node2, virtualRegister);
				}
			}
			else if (node.Instruction == IRInstruction.StoreInt32)
			{
				trace?.Log($"BEFORE:\t{node}");
				node.SetInstruction(IRInstruction.MoveInt32, virtualRegister, node.Operand3);
				trace?.Log($"AFTER: \t{node}");
			}
			else if (node.Instruction == IRInstruction.StoreInt64)
			{
				trace?.Log($"BEFORE:\t{node}");
				node.SetInstruction(IRInstruction.MoveInt64, virtualRegister, node.Operand3);
				trace?.Log($"AFTER: \t{node}");
			}
			else if (node.Instruction == IRInstruction.MemorySet)
			{
				if (node.Operand3.ConstantUnsignedLongInteger == 4)
				{
					trace?.Log($"BEFORE:\t{node}");
					node.SetInstruction(IRInstruction.MoveInt32, virtualRegister, node.Operand2);
					trace?.Log($"AFTER: \t{node}");
				}
				else if (node.Operand3.ConstantUnsignedLongInteger == 8)
				{
					trace?.Log($"BEFORE:\t{node}");
					node.SetInstruction(IRInstruction.MoveInt64, virtualRegister, node.Operand2);
					trace?.Log($"AFTER: \t{node}");
				}
			}
			else if (node.Instruction == IRInstruction.LoadInt32)
			{
				trace?.Log($"BEFORE:\t{node}");
				node.SetInstruction(IRInstruction.MoveInt32, node.Result, virtualRegister);
				trace?.Log($"AFTER: \t{node}");
			}
			else if (node.Instruction == IRInstruction.LoadInt64)
			{
				trace?.Log($"BEFORE:\t{node}");
				node.SetInstruction(IRInstruction.MoveInt64, node.Result, virtualRegister);
				trace?.Log($"AFTER: \t{node}");
			}
		}
	}
}
