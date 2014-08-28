/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Base class for code transformation stages.
	/// </summary>
	public abstract class BaseCodeTransformationStage : BaseMethodCompilerStage, IVisitor
	{
		protected override void Run()
		{
			for (int index = 0; index < BasicBlocks.Count; index++)
			{
				for (var ctx = new Context(InstructionSet, BasicBlocks[index]); !ctx.IsBlockEndInstruction; ctx.GotoNext())
				{
					{
						if (ctx.IsEmpty)
							continue;

						ctx.Clone().Visit(this);
					}
				}
			}
		}
	}
}