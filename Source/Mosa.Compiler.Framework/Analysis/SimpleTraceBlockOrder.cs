// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Analysis
{
	/// <summary>
	/// The Simple Trace Block Order quickly reorders blocks to optimize loops and reduce the distance of jumps and branches.
	/// </summary>
	public class SimpleTraceBlockOrder : BaseBlockOrder
	{
		public override int GetLoopDepth(BasicBlock block)
		{
			return 0;
		}

		public override int GetLoopIndex(BasicBlock block)
		{
			return 0;
		}

		public override void Analyze(BasicBlocks basicBlocks)
		{
			// Create dictionary of referenced blocks
			var referenced = new BitArray(basicBlocks.Count);

			// Allocate list of ordered Blocks
			NewBlockOrder = new List<BasicBlock>(basicBlocks.Count);

			// Create sorted worklist
			var workList = new Stack<BasicBlock>();

			foreach (var head in basicBlocks.HeadBlocks)
			{
				workList.Push(head);

				while (workList.Count != 0)
				{
					var block = workList.Pop();

					if (!referenced.Get(block.Sequence))
					{
						referenced.Set(block.Sequence, true);
						NewBlockOrder.Add(block);

						foreach (var successor in block.NextBlocks)
						{
							if (!referenced.Get(successor.Sequence))
							{
								workList.Push(successor);
							}
						}
					}
				}
			}
		}
	}
}
