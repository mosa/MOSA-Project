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
		protected Operand ConstantZero;

		protected override void Setup()
		{
			base.Setup();

			ConstantZero = Operand.CreateConstant(MethodCompiler.TypeSystem, 0);
		}

		protected override void Run()
		{
			for (int index = 0; index < BasicBlocks.Count; index++)
			{
				for (var ctx = new Context(InstructionSet, BasicBlocks[index]); !ctx.IsBlockEndInstruction; ctx.GotoNext())
				{
					{
						if (ctx.IsEmpty)
							continue;

						instructionCount++;

						ctx.Clone().Visit(this);
					}
				}
			}
		}
	}
}
