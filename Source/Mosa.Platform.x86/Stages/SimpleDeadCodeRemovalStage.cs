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
	/// The simple dead code removal stage remove useless instructions
	/// and NOP instructions prior to the register allocation stage.
	/// </summary>
	public sealed class SimpleDeadCodeRemovalStage : BaseTransformationStage
	{
		protected override void Run()
		{
			var trace = CreateTrace();

			bool changed = true;
			while (changed)
			{
				changed = false;

				foreach (BasicBlock block in BasicBlocks)
				{
					for (Context ctx = CreateContext(block); !ctx.IsBlockEndInstruction; ctx.GotoNext())
					{
						if (ctx.IsEmpty)
							continue;

						// Remove Nop instructions
						if (ctx.Instruction == X86.Nop)
						{
							ctx.Delete(false);
							continue;
						}

						// Remove useless instructions
						if (ctx.ResultCount == 1 && ctx.Result.Uses.Count == 0 && ctx.Result.IsVirtualRegister)
						{
							// Check is split child, if so check is parent in use (IR.Return for example)
							if (ctx.Result.IsSplitChild && ctx.Result.SplitParent.Uses.Count != 0)
								continue;

							if (trace.Active) trace.Log("REMOVED:\t" + ctx.ToString());

							ctx.Remove();
							changed = true;
						}
					}
				}
			}
		}
	}
}