// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Analysis
{
	/// <summary>
	/// Reverses the block order; this is used for testing.
	/// </summary>
	public class ReverseBlockOrder : IBlockOrderAnalysis
	{
		#region Data members

		private BasicBlock[] blockOrder;

		#endregion Data members

		#region IBlockOrderAnalysis

		public IList<BasicBlock> NewBlockOrder { get { return blockOrder; } }

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

			for (int i = basicBlocks.Count; i >= 0; i--)
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
