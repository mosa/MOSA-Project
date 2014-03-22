/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///
	/// </summary>
	public class CheckIROperandCountStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{
		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Execute()
		{
			foreach (var block in BasicBlocks)
			{
				for (var context = new Context(this.InstructionSet, block); !context.IsBlockEndInstruction; context.GotoNext())
				{
					if (!(context.Instruction is IR.BaseIRInstruction))
						continue;

					if (context.Instruction == IRInstruction.Call || context.Instruction == IRInstruction.Switch)
						continue;

					if (context.Instruction == IRInstruction.Jmp && context.OperandCount <= 1)
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