/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// This class orders blocks in reverse order. This stage is used for testing.
	/// </summary>
	public class ReverseBlockOrderStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage, IBlockOrderStage
	{
		/// <summary>
		/// Runs the specified compiler.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			for (int i = 1; i <= basicBlocks.Count / 2; i++)
			{
				BasicBlock temp = basicBlocks[i];
				basicBlocks[i] = basicBlocks[basicBlocks.Count - i];
				basicBlocks[basicBlocks.Count - i] = temp;
			}
		}
	}
}
