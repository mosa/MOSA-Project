/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// 
	/// </summary>
	public class IRCheckStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{
		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{

			foreach (var block in basicBlocks)
			{
				for (var context = new Context(this.instructionSet, block); !context.EndOfInstruction; context.GotoNext())
				{
					if (!(context.Instruction is IR.BaseIRInstruction))
						continue;

					if (context.Instruction is IR.Call || context.Instruction is IR.Switch)
						continue;

					if (context.Instruction is IR.Jmp && context.OperandCount <= 1)
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
