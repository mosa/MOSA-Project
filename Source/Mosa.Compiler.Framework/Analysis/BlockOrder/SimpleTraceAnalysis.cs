/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Analysis.BlockOrder
{
	/// <summary>
	/// The Simple Trace Block Order quickly reorders blocks to optimize loops and reduce the distance of jumps and branches.
	/// </summary>
	public class SimpleTraceBlockOrderStage : IBlockOrderAnalysis
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
			// Retrieve the first block
			BasicBlock first = basicBlocks.PrologueBlock;

			// Create dictionary of referenced blocks
			Dictionary<BasicBlock, int> referenced = new Dictionary<BasicBlock, int>(basicBlocks.Count);

			// Allocate list of ordered Blocks
			blockOrder = new BasicBlock[basicBlocks.Count];
			int orderBlockCnt = 0;

			// Create sorted worklist
			Stack<BasicBlock> workList = new Stack<BasicBlock>();

			foreach (var head in basicBlocks.HeadBlocks)
			{
				workList.Push(head);

				while (workList.Count != 0)
				{
					BasicBlock block = workList.Pop();

					if (!referenced.ContainsKey(block))
					{
						referenced.Add(block, 0);
						blockOrder[orderBlockCnt++] = block;

						foreach (BasicBlock successor in block.NextBlocks)
							if (!referenced.ContainsKey(successor))
								workList.Push(successor);
					}
				}
			}
		}

		#endregion IBlockOrderAnalysis
	}
}