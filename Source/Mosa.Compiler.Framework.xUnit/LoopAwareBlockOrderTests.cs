﻿/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Xunit;
using Xunit.Extensions;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.xUnit
{
	public class LoopAwareBlockOrderTests
	{
		internal static void Dump(BasicBlocks basicBlocks, LoopAwareBlockOrder loopAwareBlockOrder)
		{
			int index = 0;
			foreach (var block in loopAwareBlockOrder.NewBlockOrder)
			{
				if (block != null)
					Console.WriteLine("# " + index.ToString() + " Block " + block.ToString() + " #" + block.Sequence.ToString());
				else
					Console.WriteLine("# " + index.ToString() + " NONE");
				index++;
			}

			Console.WriteLine();

			foreach (var block in basicBlocks)
			{
				int depth = loopAwareBlockOrder.GetLoopDepth(block);
				int depthindex = loopAwareBlockOrder.GetLoopIndex(block);

				Console.WriteLine("Block " + block.ToString() + " #" + block.Sequence.ToString() + " -> Depth: " + depth.ToString() + " index: " + depthindex.ToString());
			}
		}

		[Fact]
		public static void LoopAwareBlockOrder1()
		{
			var basicBlocks = BlockTests.Scenario1;

			LoopAwareBlockOrder loopAwareBlockOrder = new LoopAwareBlockOrder(basicBlocks);

			Assert.Equal(loopAwareBlockOrder.GetLoopDepth(basicBlocks[0]), 0);
			Assert.Equal(loopAwareBlockOrder.GetLoopDepth(basicBlocks[1]), 1);
			Assert.Equal(loopAwareBlockOrder.GetLoopDepth(basicBlocks[2]), 1);
			Assert.Equal(loopAwareBlockOrder.GetLoopDepth(basicBlocks[3]), 1);
			Assert.Equal(loopAwareBlockOrder.GetLoopDepth(basicBlocks[4]), 2);
			Assert.Equal(loopAwareBlockOrder.GetLoopDepth(basicBlocks[5]), 0);
			Assert.Equal(loopAwareBlockOrder.GetLoopDepth(basicBlocks[6]), 2);
			Assert.Equal(loopAwareBlockOrder.GetLoopDepth(basicBlocks[7]), 1);

			Assert.Equal(loopAwareBlockOrder.GetLoopIndex(basicBlocks[0]), -1);
			Assert.Equal(loopAwareBlockOrder.GetLoopIndex(basicBlocks[1]), 1);
			Assert.Equal(loopAwareBlockOrder.GetLoopIndex(basicBlocks[2]), 1);
			Assert.Equal(loopAwareBlockOrder.GetLoopIndex(basicBlocks[3]), 1);
			Assert.Equal(loopAwareBlockOrder.GetLoopIndex(basicBlocks[4]), 0);
			Assert.Equal(loopAwareBlockOrder.GetLoopIndex(basicBlocks[5]), -1);
			Assert.Equal(loopAwareBlockOrder.GetLoopIndex(basicBlocks[6]), 0);
			Assert.Equal(loopAwareBlockOrder.GetLoopIndex(basicBlocks[7]), 1);

			Assert.Same(loopAwareBlockOrder.NewBlockOrder[0], basicBlocks[0]);
			Assert.Same(loopAwareBlockOrder.NewBlockOrder[1], basicBlocks[1]);
			Assert.Same(loopAwareBlockOrder.NewBlockOrder[2], basicBlocks[2]);
			Assert.Same(loopAwareBlockOrder.NewBlockOrder[3], basicBlocks[4]);
			Assert.Same(loopAwareBlockOrder.NewBlockOrder[4], basicBlocks[6]);

			Assert.Same(loopAwareBlockOrder.NewBlockOrder[7], basicBlocks[5]);

			Dump(basicBlocks, loopAwareBlockOrder);
		}
	}
}