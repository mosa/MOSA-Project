/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.RegisterAllocator;
using System.Collections.Generic;
using Xunit;

namespace Mosa.Compiler.Framework.xUnit
{
	public class LiveRangeTests
	{
		private static BasicBlocks CreateBasicBlockInstructionSet()
		{
			var basicBlocks = new BasicBlocks();

			var block = basicBlocks.CreateBlock();
			basicBlocks.AddHeaderBlock(block);

			var context = new Context(block);

			context.AppendInstruction(IRInstruction.Nop);
			context.AppendInstruction(IRInstruction.Nop);
			context.AppendInstruction(IRInstruction.Nop);
			context.AppendInstruction(IRInstruction.Nop);
			context.AppendInstruction(IRInstruction.Nop);
			context.AppendInstruction(IRInstruction.Nop);

			return basicBlocks;
		}

		[Fact]
		public void LiveRangeTest()
		{
			var basicBlocks = CreateBasicBlockInstructionSet();

			GreedyRegisterAllocator.NumberInstructions(basicBlocks);

			var liveRange = new LiveRange(
				new SlotIndex(basicBlocks[0].First),
				new SlotIndex(basicBlocks[0].Last),
				new List<SlotIndex>(),
				new List<SlotIndex>()
			);

			Assert.True(liveRange.IsEmpty);

			//liveRange.SplitAt(basicBlocks.)
		}
	}
}
