/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
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
	/// The simple dead code removal stage remove unless instructions and NOP instructions prior to the register allocation stage.
	/// </summary>
	public sealed class SimpleDeadCodeRemovalStage : BaseTransformationStage, IMethodCompilerStage
	{
		#region IMethodCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			foreach (BasicBlock block in basicBlocks)
			{
				for (Context ctx = CreateContext(block); !ctx.IsLastInstruction; ctx.GotoNext())
				{
					if (ctx.IsEmpty)
						continue;

					// Remove Nop instructions
					if (ctx.Instruction is Instructions.Nop)
					{
						ctx.Remove();
					}

					// Remove useless instructions
					else if (ctx.ResultCount == 1 && ctx.Result.Uses.Count == 0 && ctx.Result.IsVirtualRegister)
					{
						ctx.Remove();
					}
				}
			}
		}

		#endregion // IMethodCompilerStage Members
	}
}