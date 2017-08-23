// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Trace;
using System.Collections.Generic;

// INCOMPLETE

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// This stage determine were object references are located in code.
	/// </summary>
	public class PreciseGCStage : BaseMethodCompilerStage
	{
		protected class RangeData
		{
			public Operand StackLocal;
			public Register Register;

			public bool IsRegister { get { return Register != null; } }

			public List<int> GenList = new List<int>();
			public List<int> KillList = new List<int>();
			public List<int> UseList = new List<int>();
		}

		private Dictionary<Operand, int> stack = new Dictionary<Operand, int>();

		private TraceLog trace;

		private RangeData[] registerRangeData;
		private RangeData[] stackLocalRangeData;

		private int PhysicalRegisterCount;

		protected override void Run()
		{
			if (IsPlugged)
				return;

			PhysicalRegisterCount = Architecture.RegisterSet.Length;

			trace = CreateTraceLog();

			CollectReferenceStackObjects();

			stackLocalRangeData = new RangeData[stack.Count];
			registerRangeData = new RangeData[PhysicalRegisterCount];

			for (int i = 0; i < registerRangeData.Length; i++)
			{
				registerRangeData[i] = new RangeData()
				{
					Register = Architecture.RegisterSet[i]
				};
			}

			foreach (var local in stack)
			{
				stackLocalRangeData[local.Value] = new RangeData()
				{
					StackLocal = local.Key
				};
			}

			CollectObjectAssignmentsAndUses();
		}

		protected void CollectReferenceStackObjects()
		{
			foreach (var local in MethodCompiler.LocalStack)
			{
				if (ContainsReference(local))
				{
					stack.Add(local, stack.Count);
				}
			}
		}

		private void CollectObjectAssignmentsAndUses()
		{
			foreach (var block in BasicBlocks)
			{
				for (var node = block.First.Next; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					if (node.ResultCount == 0 && node.OperandCount == 0)
						continue;

					var visitor = new OperandVisitor(node);

					foreach (var input in visitor.Input)
					{
						if (ContainsReference(input))
						{
							if (input.IsCPURegister)
								registerRangeData[input.Register.Index].UseList.Add(node.Offset);
							else if (input.IsStackLocal)
								stackLocalRangeData[stack[input]].UseList.Add(node.Offset);
						}
						else
						{
							if (input.IsCPURegister)
								registerRangeData[input.Register.Index].KillList.Add(node.Offset);
							else if (input.IsStackLocal)
								stackLocalRangeData[stack[input]].KillList.Add(node.Offset);
						}
					}

					if (node.Instruction.FlowControl == FlowControl.Call || node.Instruction == IRInstruction.KillAll)
					{
						for (int reg = 0; reg < PhysicalRegisterCount; reg++)
						{
							registerRangeData[reg].KillList.Add(node.Offset);
						}
					}
					else if (node.Instruction == IRInstruction.KillAllExcept)
					{
						var except = node.Operand1.Register.Index;

						for (int reg = 0; reg < PhysicalRegisterCount; reg++)
						{
							if (reg != except)
							{
								registerRangeData[reg].KillList.Add(node.Offset);
							}
						}
					}

					foreach (var output in visitor.Output)
					{
						if (ContainsReference(output))
						{
							if (output.IsCPURegister)
								registerRangeData[output.Register.Index].GenList.Add(node.Offset);
							else if (output.IsStackLocal)
								stackLocalRangeData[stack[output]].GenList.Add(node.Offset);
						}
					}
				}
			}
		}

		protected bool ContainsReference(Operand operand)
		{
			if (operand.Type.IsReferenceType || operand.Type.IsManagedPointer)
				return true;

			if (!operand.IsValueType)
				return false;

			foreach (var field in operand.Type.Fields)
			{
				if (field.IsStatic)
					continue;

				if (field.FieldType.IsReferenceType || field.FieldType.IsManagedPointer)
					return true;
			}

			return false;
		}

		protected override void Finish()
		{
			stack = null;
			stackLocalRangeData = null;
			registerRangeData = null;
		}
	}
}
