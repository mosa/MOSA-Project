/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	public sealed class OperandVisitor
	{
		private InstructionNode node;

		public OperandVisitor(InstructionNode node)
		{
			this.node = node;
		}

		public IEnumerable<Operand> Input
		{
			get
			{
				foreach (var operand in node.Operands)
				{
					if (operand.IsVirtualRegister || operand.IsCPURegister)
					{
						yield return operand;
					}
					else if (operand.IsMemoryAddress && operand.OffsetBase != null)
					{
						yield return operand.OffsetBase;
					}
				}

				foreach (var operand in node.Results)
				{
					if (operand.IsMemoryAddress && operand.OffsetBase != null)
					{
						yield return operand.OffsetBase;
					}
				}
			}
		}

		public IEnumerable<Operand> Output
		{
			get
			{
				foreach (var operand in node.Results)
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
