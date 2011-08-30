/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mosa.Runtime.CompilerFramework.Operands;

namespace Mosa.Runtime.CompilerFramework
{
	public class LeaveSSA : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{

		public void Run()
		{
			foreach (var block in this.basicBlocks)
			{
				for (var context = new Context(this.instructionSet, block); !context.EndOfInstruction; context.GotoNext())
				{
					for (var i = 0; i < context.OperandCount; ++i)
					{
						var op = context.GetOperand(i);
						if (op is SsaOperand)
							context.SetOperand(i, (op as SsaOperand).Operand);
					}

					for (var i = 0; i < context.ResultCount; ++i)
					{
						var op = context.GetResult(i);
						if (op is SsaOperand)
							context.SetResult(i, (op as SsaOperand).Operand);
					}
				}
			}
		}

		public string Name
		{
			get { return @"Leave Static Single Assignment Form"; }
		}
	}
}
