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
	public class BlockTests
	{

		public BasicBlocks Scenario1
		{
			get
			{
				BasicBlocks basicBlocks = new BasicBlocks();

				BasicBlock B0 = basicBlocks.CreateBlock(0, 0);
				BasicBlock B1 = basicBlocks.CreateBlock(1, 1);
				BasicBlock B2 = basicBlocks.CreateBlock(2, 2);
				BasicBlock B3 = basicBlocks.CreateBlock(3, 3);
				BasicBlock B4 = basicBlocks.CreateBlock(4, 4);
				BasicBlock B5 = basicBlocks.CreateBlock(5, 5);
				BasicBlock B6 = basicBlocks.CreateBlock(6, 6);
				BasicBlock B7 = basicBlocks.CreateBlock(7, 7);

				basicBlocks.LinkBlocks(B0, B1);
				basicBlocks.LinkBlocks(B1, B2);
				basicBlocks.LinkBlocks(B1, B3);
				basicBlocks.LinkBlocks(B3, B1);

				basicBlocks.LinkBlocks(B2, B4);
				basicBlocks.LinkBlocks(B2, B5);

				basicBlocks.LinkBlocks(B4, B6);
				basicBlocks.LinkBlocks(B4, B7);
				basicBlocks.LinkBlocks(B6, B4);

				basicBlocks.LinkBlocks(B7, B1);

				return basicBlocks;
			}
		}

		protected void LinkBlocks(BasicBlock from, BasicBlock to)
		{
			from.NextBlocks.Add(to);
			to.PreviousBlocks.Add(from);
		}

		[Test]
		public void GetConnectedBlocksStartingAtHead()
		{
			var basicBlocks = Scenario1;

			var blocks = basicBlocks.GetConnectedBlocksStartingAtHead(basicBlocks[0]);

			Assert.AreEqual(blocks.Count, 8);
		}

		[Test]
		public void GetExitBlock()
		{
			var basicBlocks = Scenario1;

			var block = basicBlocks.GetExitBlock(basicBlocks[0]);

			Assert.AreSame<BasicBlock>(basicBlocks[5], block);
		}
	}
}
