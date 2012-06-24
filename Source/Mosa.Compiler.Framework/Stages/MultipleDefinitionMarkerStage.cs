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
	public class MultipleDefinitionMarkerStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{
		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			Dictionary<Operand, int> list = new Dictionary<Operand, int>();

			foreach (var block in this.basicBlocks)
			{
				for (var context = new Context(this.instructionSet, block); !context.EndOfInstruction; context.GotoNext())
				{
					context.Marked = false;

					if (context.Result == null)
						continue;

					int index = 0;

					if (list.TryGetValue(context.Result, out index))
					{
						instructionSet.Data[index].Marked = true;
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
