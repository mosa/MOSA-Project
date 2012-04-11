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

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// The Simple Trace Block Order Stage reorders Blocks to optimize loops and reduce the distance of jumps and branches.
	/// </summary>
	public class SimpleTraceBlockOrderStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage, IBlockOrderStage
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		protected BasicBlock[] _ordered;

		#endregion // Data members

		/// <summary>
		/// Runs the specified compiler.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			// Retrieve the first block
			BasicBlock first = FindBlock(-1);

			// Create dictionary of referenced blocks
			Dictionary<BasicBlock, int> referenced = new Dictionary<BasicBlock, int>(basicBlocks.Count);

			// Allocate list of ordered Blocks
			_ordered = new BasicBlock[basicBlocks.Count];
			int orderBlockCnt = 0;

			// Create sorted worklist
			Stack<BasicBlock> workList = new Stack<BasicBlock>();

			// Start worklist with first block
			workList.Push(first);

			while (workList.Count != 0)
			{
				BasicBlock block = workList.Pop();

				if (!referenced.ContainsKey(block))
				{
					referenced.Add(block, 0);
					_ordered[orderBlockCnt++] = block;

					foreach (BasicBlock successor in block.NextBlocks)
						if (!referenced.ContainsKey(successor))
							workList.Push(successor);
				}
			}

			// Place unreferenced blocks at the end of the list
			foreach (BasicBlock block in basicBlocks)
				if (!referenced.ContainsKey(block))
					_ordered[orderBlockCnt++] = block;

			OrderBlocks();
		}

		/// <summary>
		/// Orders the blocks.
		/// </summary>
		private void OrderBlocks()
		{
			for (int i = 0; i < basicBlocks.Count; i++)
				basicBlocks[i] = _ordered[i];
		}

	}
}
