// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Common
{
	public struct OperandVisitor
	{
		private readonly InstructionNode Node;

		public OperandVisitor(InstructionNode node)
		{
			Node = node;
		}

		public IEnumerable<Operand> Input
		{
			get
			{
				foreach (var operand in Node.Operands)
				{
					if (operand.IsVirtualRegister || operand.IsCPURegister)
					{
						yield return operand;
					}
				}
			}
		}

		public IEnumerable<Operand> Output
		{
			get
			{
				foreach (var operand in Node.Results)
				{
					if (operand.IsVirtualRegister || operand.IsCPURegister)
					{
						yield return operand;
					}
				}
			}
		}
	}
}
