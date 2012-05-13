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

		public static BasicBlocks Scenario1
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

		//#       1 -----+
		//#       |      |
		//#       2 <--+ |
		//#       2    | |
		//#  +--- 2 ---+ |
		//#  |    |      |
		//#  |    3 <----+
		//#  |    |
		//#  +--> 4
		public static BasicBlocks Scenario2
		{
			get
			{
				BasicBlocks basicBlocks = new BasicBlocks();

				BasicBlock B1 = basicBlocks.CreateBlock(1, 1);
				BasicBlock B2 = basicBlocks.CreateBlock(2, 2);
				BasicBlock B3 = basicBlocks.CreateBlock(3, 3);
				BasicBlock B4 = basicBlocks.CreateBlock(4, 4);

				basicBlocks.LinkBlocks(B1, B2);
				basicBlocks.LinkBlocks(B1, B3);
				basicBlocks.LinkBlocks(B2, B2);
				basicBlocks.LinkBlocks(B2, B3);
				basicBlocks.LinkBlocks(B2, B4);
				basicBlocks.LinkBlocks(B3, B4);

				return basicBlocks;
			}
		}

		public static BasicBlocks Scenario3
		{
			get
			{
				/* Example from http://www.cs.rice.edu/~keith/512/Lectures/08Dominators-1up.pdf (Page 6) */

				BasicBlocks basicBlocks = new BasicBlocks();

				BasicBlock A = basicBlocks.CreateBlock(1, 1);
				BasicBlock B = basicBlocks.CreateBlock(2, 2);
				BasicBlock C = basicBlocks.CreateBlock(3, 3);
				BasicBlock D = basicBlocks.CreateBlock(4, 4);
				BasicBlock E = basicBlocks.CreateBlock(5, 5);
				BasicBlock F = basicBlocks.CreateBlock(6, 6);
				BasicBlock G = basicBlocks.CreateBlock(7, 7);

				basicBlocks.LinkBlocks(A, B);
				basicBlocks.LinkBlocks(A, C);
				basicBlocks.LinkBlocks(B, G);
				basicBlocks.LinkBlocks(C, D);
				basicBlocks.LinkBlocks(C, E);
				basicBlocks.LinkBlocks(D, F);
				basicBlocks.LinkBlocks(E, F);
				basicBlocks.LinkBlocks(F, G);

				return basicBlocks;
			}
		}

		protected static void LinkBlocks(BasicBlock from, BasicBlock to)
		{
			from.NextBlocks.Add(to);
			to.PreviousBlocks.Add(from);
		}

		[Test]
		public static void GetConnectedBlocksStartingAtHead()
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
