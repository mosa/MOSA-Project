using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Mosa.Compiler.Framework;

namespace Mosa.Test.Compiler.Framework
{
	[TestFixture]
	public class LoopTests
	{

		protected List<BasicBlock> Scenario1
		{
			get
			{
				List<BasicBlock> basicBlocks = new List<BasicBlock>();

				BasicBlock B0 = new BasicBlock(0, 0, 0);
				BasicBlock B1 = new BasicBlock(1, 1, 0);
				BasicBlock B2 = new BasicBlock(2, 2, 0);
				BasicBlock B3 = new BasicBlock(3, 3, 0);
				BasicBlock B4 = new BasicBlock(4, 4, 0);
				BasicBlock B5 = new BasicBlock(5, 5, 0);
				BasicBlock B6 = new BasicBlock(6, 6, 0);
				BasicBlock B7 = new BasicBlock(7, 7, 0);

				LinkBlocks(B0, B1);
				LinkBlocks(B1, B2);
				LinkBlocks(B1, B3);

				LinkBlocks(B2, B4);
				LinkBlocks(B2, B5);

				LinkBlocks(B4, B6);
				LinkBlocks(B4, B7);

				basicBlocks.Add(B0);
				basicBlocks.Add(B1);
				basicBlocks.Add(B2);
				basicBlocks.Add(B3);
				basicBlocks.Add(B4);
				basicBlocks.Add(B5);
				basicBlocks.Add(B6);
				basicBlocks.Add(B7);

				return basicBlocks;
			}
		}

		protected void LinkBlocks(BasicBlock from, BasicBlock to)
		{
			from.NextBlocks.Add(to);
			to.PreviousBlocks.Add(from);
		}

		[Test]
		public void TestLoops1()
		{
			LoopAwareBlockOrder loops = new LoopAwareBlockOrder(Scenario1);

			Assert.AreEqual(3, loops.LoopEdges.Count);

		}

	}
}
