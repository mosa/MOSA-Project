// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Analysis.LiveVariableAnalysis
{
	/// <summary>
	/// Register Allocator Environment
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Analysis.LiveVariableAnalysis .BaseLivenessAnalysisEnvironment" />
	public class GCEnvironment : BaseLivenessAnalysisEnvironment
	{
		protected Dictionary<Operand, int> stackLookup = new Dictionary<Operand, int>();
		protected Dictionary<int, Operand> stackLookupReverse = new Dictionary<int, Operand>();
		protected int PhysicalRegisterCount { get; }
		protected bool[] StackLocalReference;

		public GCEnvironment(BasicBlocks basicBlocks, BaseArchitecture architecture, List<Operand> localStack)
		{
			BasicBlocks = basicBlocks;
			StackLocalReference = new bool[localStack.Count];

			PhysicalRegisterCount = architecture.RegisterSet.Length;

			CollectReferenceStackObjects(localStack);

			IndexCount = PhysicalRegisterCount + stackLookup.Count;
		}

		protected int GetIndex(Operand operand)
		{
			return operand.IsCPURegister ? operand.Register.Index : stackLookup[operand];
		}

		protected bool ContainsObjectReference(Operand operand)
		{
			if (operand.IsCPURegister && ContainsReference(operand))
			{
				return true;
			}
			else if (operand.IsStackLocal && StackLocalReference[operand.Index])
			{
				return true;
			}

			return false;
		}

		public override IEnumerable<int> GetInputs(InstructionNode node)
		{
			if (node.Instruction == IRInstruction.KillAll || node.Instruction == IRInstruction.KillAllExcept || node.Instruction == IRInstruction.Kill)
				yield break;

			foreach (var operand in node.Operands)
			{
				if (ContainsObjectReference(operand))
				{
					yield return GetIndex(operand);
				}
			}
		}

		public override IEnumerable<int> GetOutputs(InstructionNode node)
		{
			if (node.Instruction == IRInstruction.KillAll || node.Instruction == IRInstruction.KillAllExcept || node.Instruction == IRInstruction.Kill)
				yield break;

			foreach (var operand in node.Results)
			{
				if (ContainsObjectReference(operand))
				{
					yield return GetIndex(operand);
				}
			}
		}

		public override IEnumerable<int> GetKills(InstructionNode node)
		{
			if (node.Instruction.FlowControl == FlowControl.Call || node.Instruction == IRInstruction.KillAll)
			{
				for (int reg = 0; reg < IndexCount; reg++)
				{
					yield return reg;
				}
				yield break;
			}
			else if (node.Instruction == IRInstruction.KillAllExcept)
			{
				var except = node.Operand1.Register.Index;

				for (int reg = 0; reg < IndexCount; reg++)
				{
					if (reg != except)
					{
						yield return reg;
					}
				}
				yield break;
			}
			else if (node.Instruction == IRInstruction.Kill)
			{
				foreach (var op in node.Operands)
				{
					yield return op.Register.Index;
				}
				yield break;
			}

			foreach (var operand in node.Operands)
			{
				if (operand.IsCPURegister && !ContainsReference(operand))
				{
					yield return GetIndex(operand);
				}

				// IsStackLocal ???
			}
		}

		public bool ContainsReference(Operand operand)
		{
			if (operand.IsReferenceType || operand.IsManagedPointer)
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

		protected void CollectReferenceStackObjects(List<Operand> localStack)
		{
			foreach (var local in localStack)
			{
				var containsRefernce = ContainsReference(local);

				StackLocalReference[local.Index] = containsRefernce;

				if (containsRefernce)
				{
					stackLookupReverse.Add(PhysicalRegisterCount + stackLookup.Count, local);
					stackLookup.Add(local, PhysicalRegisterCount + stackLookup.Count);
				}
			}
		}
	}
}
