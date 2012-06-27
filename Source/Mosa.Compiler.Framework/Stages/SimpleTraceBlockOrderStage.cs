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

		/// <summary>
		/// Runs the specified compiler.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			// Retrieve the first block
			BasicBlock first = basicBlocks.PrologueBlock;

			// Create dictionary of referenced blocks
			Dictionary<BasicBlock, int> referenced = new Dictionary<BasicBlock, int>(basicBlocks.Count);

			// Allocate list of ordered Blocks
			BasicBlock[] ordered = new BasicBlock[basicBlocks.Count];
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
						ordered[orderBlockCnt++] = block;

						foreach (BasicBlock successor in block.NextBlocks)
							if (!referenced.ContainsKey(successor))
								workList.Push(successor);
					}
				}
			}

			basicBlocks.ReorderBlocks(ordered);
		}


	}
}
