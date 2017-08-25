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

		private RangeData[] rangeData;

		private int PhysicalRegisterCount;
		private int RangeCount;

		protected override void Run()
		{
			if (IsPlugged)
				return;

			PhysicalRegisterCount = Architecture.RegisterSet.Length;

			CollectReferenceStackObjects();

			RangeCount = PhysicalRegisterCount + stack.Count;

			trace = CreateTraceLog();

			rangeData = new RangeData[RangeCount];

			for (int i = 0; i < PhysicalRegisterCount; i++)
			{
				rangeData[i] = new RangeData()
				{
					Register = Architecture.RegisterSet[i]
				};
			}

			foreach (var local in stack)
			{
				rangeData[local.Value] = new RangeData()
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
					stack.Add(local, PhysicalRegisterCount + stack.Count);
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
								rangeData[input.Register.Index].UseList.Add(node.Offset);
							else if (input.IsStackLocal)
								rangeData[stack[input]].UseList.Add(node.Offset);
						}
						else
						{
							if (input.IsCPURegister)
								rangeData[input.Register.Index].KillList.Add(node.Offset);
							else if (input.IsStackLocal)
								rangeData[stack[input]].KillList.Add(node.Offset);
						}
					}

					if (node.Instruction.FlowControl == FlowControl.Call || node.Instruction == IRInstruction.KillAll)
					{
						for (int reg = 0; reg < RangeCount; reg++)
						{
							rangeData[reg].KillList.Add(node.Offset);
						}
					}
					else if (node.Instruction == IRInstruction.KillAllExcept)
					{
						var except = node.Operand1.Register.Index;

						for (int reg = 0; reg < RangeCount; reg++)
						{
							if (reg != except)
							{
								rangeData[reg].KillList.Add(node.Offset);
							}
						}
					}

					foreach (var output in visitor.Output)
					{
						if (ContainsReference(output))
						{
							if (output.IsCPURegister)
								rangeData[output.Register.Index].GenList.Add(node.Offset);
							else if (output.IsStackLocal)
								rangeData[stack[output]].GenList.Add(node.Offset);
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
			rangeData = null;
		}
	}
}
