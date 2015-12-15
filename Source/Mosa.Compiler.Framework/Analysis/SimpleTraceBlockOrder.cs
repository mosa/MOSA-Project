// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Analysis
{
	/// <summary>
	/// The Simple Trace Block Order quickly reorders blocks to optimize loops and reduce the distance of jumps and branches.
	/// </summary>
	public class SimpleTraceBlockOrder : IBlockOrderAnalysis
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
			// Create dictionary of referenced blocks
			var referenced = new Dictionary<BasicBlock, int>(basicBlocks.Count);

			// Allocate list of ordered Blocks
			blockOrder = new BasicBlock[basicBlocks.Count];
			int orderBlockCnt = 0;

			// Create sorted worklist
			var workList = new Stack<BasicBlock>();

			foreach (var head in basicBlocks.HeadBlocks)
			{
				workList.Push(head);

				while (workList.Count != 0)
				{
					var block = workList.Pop();

					if (!referenced.ContainsKey(block))
					{
						referenced.Add(block, 0);
						blockOrder[orderBlockCnt++] = block;

						foreach (var successor in block.NextBlocks)
							if (!referenced.ContainsKey(successor))
								workList.Push(successor);
					}
				}
			}
		}

		#endregion IBlockOrderAnalysis
	}
}
