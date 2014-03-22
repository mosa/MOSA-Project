/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///
	/// </summary>
	public class MultipleDefinitionMarkerStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			Dictionary<Operand, int> list = new Dictionary<Operand, int>();

			foreach (var block in this.BasicBlocks)
			{
				for (var context = new Context(this.InstructionSet, block); !context.IsBlockEndInstruction; context.GotoNext())
				{
					context.Marked = false;

					if (context.Result == null)
						continue;

					int index = 0;

					if (list.TryGetValue(context.Result, out index))
					{
						InstructionSet.Data[index].Marked = true;
						context.Marked = true;
					}
					else
					{
						list.Add(context.Result, context.Index);
					}
				}
			}
		}
	}
}