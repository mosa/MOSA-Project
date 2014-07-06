/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Framework.Analysis
{
	/// <summary>
	/// Keeps the same block ordering.
	/// </summary>
	public class NaturalBlockOrder : IBlockOrderAnalysis
	{
		#region Data members

		private BasicBlock[] blockOrder;

		#endregion Data members

		#region IBlockOrderAnalysis

		public BasicBlock[] NewBlockOrder { get { return blockOrder; } }

		public int GetLoopDepth(BasicBlock block)
		{
			return 0;
		}

		public int GetLoopIndex(BasicBlock block)
		{
			return 0;
		}

		public void PerformAnalysis(BasicBlocks basicBlocks)
		{
			blockOrder = new BasicBlock[basicBlocks.Count];
			int orderBlockCnt = 0;

			blockOrder[orderBlockCnt++] = basicBlocks.PrologueBlock;

			for (int i = 0; i < basicBlocks.Count; i++)
			{
				if (basicBlocks[i] != basicBlocks.PrologueBlock)
				{
					blockOrder[orderBlockCnt++] = basicBlocks[i];
				}
			}
		}

		#endregion IBlockOrderAnalysis
	}
}