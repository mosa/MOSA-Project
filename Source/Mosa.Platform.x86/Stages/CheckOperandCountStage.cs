/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	///
	/// </summary>
	public class CheckOperandCountStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{
		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			foreach (var block in basicBlocks)
			{
				for (var context = new Context(this.instructionSet, block); !context.IsBlockEndInstruction; context.GotoNext())
				{
					if (!(context.Instruction is X86Instruction))
						continue;

					if (context.OperandCount != context.Instruction.DefaultOperandCount ||
						context.ResultCount != context.Instruction.DefaultResultCount)
					{
						context.Marked = true;
					}
				}
			}
		}
	}
}