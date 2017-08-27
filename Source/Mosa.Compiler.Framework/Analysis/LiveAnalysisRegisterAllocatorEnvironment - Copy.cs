// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Analysis
{
	/// <summary>
	/// Register Allocator Environment
	/// </summary>
	public class RegisterAllocatorEnvironment
	{
		public int IndexCount { get; protected set; }

		public BasicBlocks BasicBlocks { get; protected set; }

		public int PhysicalRegisterCount { get; set; }

		protected int GetIndex(Operand operand)
		{
			return (operand.IsCPURegister) ? (operand.Register.Index) : (operand.Index + PhysicalRegisterCount - 1);
		}

		public IEnumerable<int> GetInputs(InstructionNode node)
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

		public IEnumerable<int> GetOutput(InstructionNode node)
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

		public IEnumerable<int> GetKill(InstructionNode node)
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

		public RegisterAllocatorEnvironment(BasicBlocks basicBlocks, BaseArchitecture architecture)
		{
			BasicBlocks = basicBlocks;
			PhysicalRegisterCount = architecture.RegisterSet.Length;
		}
	}
}
