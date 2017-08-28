// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Analysis.Live
{
	/// <summary>
	/// Register Allocator Environment
	/// </summary>
	public class LiveAnalysisGCEnvironment
	{
		public int IndexCount { get; protected set; }

		public BasicBlocks BasicBlocks { get; protected set; }

		public int PhysicalRegisterCount { get; set; }

		private Dictionary<Operand, int> stackLookup = new Dictionary<Operand, int>();

		//protected BaseArchitecture Architecture;

		protected int GetIndex(Operand operand)
		{
			return operand.IsCPURegister ? operand.Register.Index : stackLookup[operand];
		}

		public IEnumerable<int> GetInputs(InstructionNode node)
		{
			foreach (var operand in node.Operands)
			{
				if (operand.IsCPURegister && operand.Register.IsSpecial)
					continue;

				if (ContainsReference(operand))
				{
					yield return GetIndex(operand);
				}
			}
		}

		public IEnumerable<int> GetOutput(InstructionNode node)
		{
			foreach (var operand in node.Results)
			{
				if (operand.IsCPURegister && operand.Register.IsSpecial)
					continue;

				if (ContainsReference(operand))
				{
					yield return GetIndex(operand);
				}
			}
		}

		public IEnumerable<int> GetKill(InstructionNode node)
		{
			foreach (var operand in node.Operands)
			{
				if (operand.IsCPURegister && operand.Register.IsSpecial || !ContainsReference(operand))
				{
					yield return GetIndex(operand);
				}
			}

			if (node.Instruction.FlowControl == FlowControl.Call || node.Instruction == IRInstruction.KillAll)
			{
				for (int reg = 0; reg < PhysicalRegisterCount; reg++)
				{
					yield return reg;
				}
			}
			else if (node.Instruction == IRInstruction.KillAllExcept)
			{
				var except = node.Operand1.Register.Index;

				for (int reg = 0; reg < PhysicalRegisterCount; reg++)
				{
					if (reg != except)
					{
						yield return reg;
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

		public LiveAnalysisGCEnvironment(BasicBlocks basicBlocks, BaseArchitecture architecture, List<Operand> localStack)
		{
			BasicBlocks = basicBlocks;

			//Architecture = architecture;
			PhysicalRegisterCount = architecture.RegisterSet.Length;

			CollectReferenceStackObjects(localStack);

			IndexCount = PhysicalRegisterCount + stackLookup.Count;
		}

		protected void CollectReferenceStackObjects(IList<Operand> localStack)
		{
			foreach (var local in localStack)
			{
				if (ContainsReference(local))
				{
					stackLookup.Add(local, PhysicalRegisterCount + stackLookup.Count);
				}
			}
		}
	}
}
