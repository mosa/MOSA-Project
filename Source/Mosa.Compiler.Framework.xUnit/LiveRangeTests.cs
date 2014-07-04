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
		private static Tuple<BasicBlocks, InstructionSet> CreateBasicBlockInstructionSet()
		{
			var basicBlocks = new BasicBlocks();
			var instructionSet = new InstructionSet(25);

			var context = instructionSet.CreateNewBlock(basicBlocks);
			basicBlocks.AddHeaderBlock(context.BasicBlock);

			context.AppendInstruction(IRInstruction.Nop);
			context.AppendInstruction(IRInstruction.Nop);
			context.AppendInstruction(IRInstruction.Nop);
			context.AppendInstruction(IRInstruction.Nop);
			context.AppendInstruction(IRInstruction.Nop);
			context.AppendInstruction(IRInstruction.Nop);

			return new Tuple<BasicBlocks, InstructionSet>(basicBlocks, instructionSet);
		}

		[Fact]
		public void LiveRangeTest()
		{
			var tuple = CreateBasicBlockInstructionSet();
			var basicBlocks = tuple.Item1;
			var instructionSet = tuple.Item2;

			GreedyRegisterAllocator.NumberInstructions(basicBlocks, instructionSet);

			var liveRange = new LiveRange(
				new SlotIndex(instructionSet, basicBlocks[0].StartIndex),
				new SlotIndex(instructionSet, basicBlocks[0].EndIndex),
				new List<SlotIndex>(),
				new List<SlotIndex>()
			);

			Assert.True(liveRange.IsEmpty);

			//liveRange.SplitAt(basicBlocks.)
		}
	}
}