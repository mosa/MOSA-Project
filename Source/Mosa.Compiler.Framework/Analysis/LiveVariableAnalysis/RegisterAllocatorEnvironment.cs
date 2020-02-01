// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Analysis.LiveVariableAnalysis
{
	/// <summary>
	/// Register Allocator Environment
	/// </summary>
	public class RegisterAllocatorEnvironment : BaseLivenessAnalysisEnvironment
	{
		protected int PhysicalRegisterCount { get; set; }

		public RegisterAllocatorEnvironment(BasicBlocks basicBlocks, BaseArchitecture architecture)
		{
			BasicBlocks = basicBlocks;
			PhysicalRegisterCount = architecture.RegisterSet.Length;
		}

		protected int GetIndex(Operand operand)
		{
			return (operand.IsCPURegister) ? (operand.Register.Index) : (operand.Index + PhysicalRegisterCount - 1);
		}

		public override IEnumerable<int> GetInputs(InstructionNode node)
		{
			foreach (var operand in node.Operands)
			{
				if (operand.IsCPURegister && operand.Register.IsSpecial)
					continue;

				if (operand.IsVirtualRegister || operand.IsCPURegister)
				{
					yield return GetIndex(operand);
				}
			}
		}

		public override IEnumerable<int> GetOutputs(InstructionNode node)
		{
			foreach (var operand in node.Results)
			{
				if (operand.IsCPURegister && operand.Register.IsSpecial)
					continue;

				if (operand.IsVirtualRegister || operand.IsCPURegister)
				{
					yield return GetIndex(operand);
				}
			}
		}

		public override IEnumerable<int> GetKills(InstructionNode node)
		{
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
	}
}
