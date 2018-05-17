// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Trace;
using System;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Promote Struct To Register
	/// </summary>
	public class PromoteStructToRegister : EmptyBlockRemovalStage
	{
		private TraceLog trace;

		private int temporariesPromoted = 0;

		protected override void Setup()
		{
			base.Setup();

			trace = CreateTraceLog();

			temporariesPromoted = 0;
		}

		protected override void Run()
		{
			// go thru list of temporaries
			// for temporaries that are structs with a single value and can be place in a register, consider promoting them into a register

			foreach (var operand in MethodCompiler.LocalStack)
			{
				Debug.Assert(operand.IsStackLocal);

				if (CanPromote(operand))
				{
					Promote(operand);
				}
			}

			UpdateCounter("PromoteStructToRegister.TemporariesPromoted", temporariesPromoted);
		}

		protected bool CanPromote(Operand operand)
		{
			if (trace.Active) trace.Log("Check: " + operand);

			if (operand.Uses.Count == 0)
			{
				if (trace.Active) trace.Log("No uses: " + operand);
				return false;
			}

			if (MosaTypeLayout.IsStoredOnStack(operand.Type))
			{
				if (trace.Active) trace.Log("incompatible type: " + operand);
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
							if (trace.Active) trace.Log("No: " + node2);

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

				if (trace.Active) trace.Log("No: " + node);
				return false;
			}

			return true;
		}

		private bool Check(InstructionNode node)
		{
			if (node.Instruction == IRInstruction.StoreInt32 || node.Instruction == IRInstruction.StoreInt64)
			{
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

				if (trace.Active) trace.Log("No: " + node);

				return false;
			}
			else if (node.Instruction == IRInstruction.MoveInt32 || node.Instruction == IRInstruction.MoveInt64)
			{
				foreach (var node2 in node.Result.Uses)
				{
					if (!Check(node2))
					{
						if (trace.Active) trace.Log("No (parent): " + node);
						if (trace.Active) trace.Log("No: " + node2);
						return false;
					}
				}

				return true;
			}

			if (trace.Active) trace.Log("No: " + node);
			return false;
		}

		protected void Promote(Operand operand)
		{
			var virtualRegister = AllocateVirtualRegister(operand.Type);
			temporariesPromoted++;

			if (trace.Active) trace.Log("VR: " + virtualRegister);

			// IR.AddressOf  v12 <= unresolved (t0) - find uses of result
			//		IR.StoreInt32  v12, 0, v23 {t:System.Void*} ===> IR.Move x <= v23
			//		IR.MemorySet  v12[Mosa.TestWorld.x86.Tests.NotBoxedStruct &], const= 0[U4], const= 4[I4]} ===> IR.StoreInt32 v12, [constant]
			// IR.LoadInt32  v5 <= EBP, unresolved (t0)   ===> IR.Move v5 <= x

			foreach (var node in operand.Uses.ToArray())
			{
				if (node.Instruction == IRInstruction.AddressOf)
				{
					foreach (var node2 in node.Result.Uses.ToArray())
					{
						Promote(node2, virtualRegister);
					}

					//Debug.Assert(node.Result.Uses.Count == 0);
					//if (trace.Active) trace.Log("REMOVED:\t" + node);
					//node.Empty();
				}
				else if (node.Instruction == IRInstruction.LoadInt32)
				{
					if (trace.Active) trace.Log("BEFORE:\t" + node);
					node.SetInstruction(IRInstruction.MoveInt32, node.Result, virtualRegister);
					if (trace.Active) trace.Log("AFTER: \t" + node);
				}
				else if (node.Instruction == IRInstruction.LoadInt64)
				{
					if (trace.Active) trace.Log("BEFORE:\t" + node);
					node.SetInstruction(IRInstruction.MoveInt32, node.Result, virtualRegister);
					if (trace.Active) trace.Log("AFTER: \t" + node);
				}
			}
		}

		private void Promote(InstructionNode node, Operand virtualRegister)
		{
			if (node.Instruction == IRInstruction.StoreInt32)
			{
				if (trace.Active) trace.Log("BEFORE:\t" + node);
				node.SetInstruction(IRInstruction.MoveInt32, virtualRegister, node.Operand3);
				if (trace.Active) trace.Log("AFTER: \t" + node);
			}
			else if (node.Instruction == IRInstruction.StoreInt64)
			{
				if (trace.Active) trace.Log("BEFORE:\t" + node);
				node.SetInstruction(IRInstruction.MoveInt32, virtualRegister, node.Operand3);
				if (trace.Active) trace.Log("AFTER: \t" + node);
			}
			else if (node.Instruction == IRInstruction.MemorySet)
			{
				if (node.Operand3.ConstantUnsignedLongInteger == 4)
				{
					if (trace.Active) trace.Log("BEFORE:\t" + node);
					node.SetInstruction(IRInstruction.MoveInt32, virtualRegister, node.Operand2);
					if (trace.Active) trace.Log("AFTER: \t" + node);
				}
				else if (node.Operand3.ConstantUnsignedLongInteger == 8)
				{
					if (trace.Active) trace.Log("BEFORE:\t" + node);
					node.SetInstruction(IRInstruction.MoveInt64, virtualRegister, node.Operand2);
					if (trace.Active) trace.Log("AFTER: \t" + node);
				}
			}
			else if (node.Instruction == IRInstruction.MoveInt32 || node.Instruction == IRInstruction.MoveInt64)
			{
				foreach (var node2 in node.Result.Uses.ToArray())
				{
					Promote(node2, virtualRegister);
				}
			}
		}
	}
}
