/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using MbUnit.Framework;
using Mosa.Compiler.Framework;

namespace Mosa.Test.Compiler.Framework
{

	#region Extension Helper

	internal static class BasicBlocksHelp
	{

		public static void CreateBlock(this BasicBlocks basicBlocks, int index)
		{
			basicBlocks.CreateBlock(index);
		}

	}

	#endregion

	[TestFixture]
	public class BlockTests
	{

		public static BasicBlocks Scenario1
		{
			get
			{
				BasicBlocks basicBlocks = new BasicBlocks();

				BasicBlock B0 = basicBlocks.CreateBlock(0);
				BasicBlock B1 = basicBlocks.CreateBlock(1);
				BasicBlock B2 = basicBlocks.CreateBlock(2);
				BasicBlock B3 = basicBlocks.CreateBlock(3);
				BasicBlock B4 = basicBlocks.CreateBlock(4);
				BasicBlock B5 = basicBlocks.CreateBlock(5);
				BasicBlock B6 = basicBlocks.CreateBlock(6);
				BasicBlock B7 = basicBlocks.CreateBlock(7);

				basicBlocks.LinkBlocks(B0, B1);
				basicBlocks.LinkBlocks(B1, new[] { B2, B3 });
				basicBlocks.LinkBlocks(B3, B1);
				basicBlocks.LinkBlocks(B2, new[] { B4, B5 });
				basicBlocks.LinkBlocks(B4, new[] { B6, B7 });
				basicBlocks.LinkBlocks(B6, B4);
				basicBlocks.LinkBlocks(B7, B1);

				basicBlocks.AddHeaderBlock(B0);

				return basicBlocks;
			}
		}

		public static BasicBlocks Scenario2
		{
			get
			{
				//#       1 -----+
				//#       |      |
				//#       2 <--+ |
				//#       2    | |
				//#  +--- 2 ---+ |
				//#  |    |      |
				//#  |    3 <----+
				//#  |    |
				//#  +--> 4			

				BasicBlocks basicBlocks = new BasicBlocks();

				BasicBlock B1 = basicBlocks.CreateBlock(1);
				BasicBlock B2 = basicBlocks.CreateBlock(2);
				BasicBlock B3 = basicBlocks.CreateBlock(3);
				BasicBlock B4 = basicBlocks.CreateBlock(4);

				basicBlocks.LinkBlocks(B1, new[] { B2, B3 });
				basicBlocks.LinkBlocks(B2, new[] { B2, B3, B4 });
				basicBlocks.LinkBlocks(B3, B4);

				basicBlocks.AddHeaderBlock(B1);

				return basicBlocks;
			}
		}

		public static BasicBlocks Scenario3
		{
			get
			{
				/* Example from http://www.cs.rice.edu/~keith/512/Lectures/08Dominators-1up.pdf (Page 6) */

				BasicBlocks basicBlocks = new BasicBlocks();

				BasicBlock A = basicBlocks.CreateBlock(1);
				BasicBlock B = basicBlocks.CreateBlock(2);
				BasicBlock C = basicBlocks.CreateBlock(3);
				BasicBlock D = basicBlocks.CreateBlock(4);
				BasicBlock E = basicBlocks.CreateBlock(5);
				BasicBlock F = basicBlocks.CreateBlock(6);
				BasicBlock G = basicBlocks.CreateBlock(7);

				basicBlocks.LinkBlocks(A, new[] { B, C });
				basicBlocks.LinkBlocks(B, G);
				basicBlocks.LinkBlocks(C, new[] { D, E });
				basicBlocks.LinkBlocks(D, F);
				basicBlocks.LinkBlocks(E, F);
				basicBlocks.LinkBlocks(F, G);

				basicBlocks.AddHeaderBlock(A);

				return basicBlocks;
			}
		}

		public static BasicBlocks Scenario4
		{
			get
			{
				/* Example from webdocs.cs.ualberta.ca/~amaral/courses/680/.../TG-SSA/TG-SSA.ppt (Page 26) */

				BasicBlocks basicBlocks = new BasicBlocks();

				BasicBlock B1 = basicBlocks.CreateBlock(1);
				BasicBlock B2 = basicBlocks.CreateBlock(2);
				BasicBlock B3 = basicBlocks.CreateBlock(3);
				BasicBlock B4 = basicBlocks.CreateBlock(4);
				BasicBlock B5 = basicBlocks.CreateBlock(5);
				BasicBlock B6 = basicBlocks.CreateBlock(6);
				BasicBlock B7 = basicBlocks.CreateBlock(7);
				BasicBlock B8 = basicBlocks.CreateBlock(8);
				BasicBlock B9 = basicBlocks.CreateBlock(9);
				BasicBlock B10 = basicBlocks.CreateBlock(10);
				BasicBlock B11 = basicBlocks.CreateBlock(11);
				BasicBlock B12 = basicBlocks.CreateBlock(12);
				BasicBlock B13 = basicBlocks.CreateBlock(13);

				basicBlocks.LinkBlocks(B1, new[] { B2, B5, B9 });
				basicBlocks.LinkBlocks(B2, B3);
				basicBlocks.LinkBlocks(B3, new[] { B3, B4 });
				basicBlocks.LinkBlocks(B4, B13);
				basicBlocks.LinkBlocks(B5, new[] { B6, B7 });
				basicBlocks.LinkBlocks(B6, new[] { B4, B8 });
				basicBlocks.LinkBlocks(B7, new[] { B8, B12 });
				basicBlocks.LinkBlocks(B8, new[] { B5, B13 });

				basicBlocks.LinkBlocks(B9, new[] { B10, B11 });
				basicBlocks.LinkBlocks(B10, B12);
				basicBlocks.LinkBlocks(B11, B12);
				basicBlocks.LinkBlocks(B12, B13);

				basicBlocks.AddHeaderBlock(B1);

				return basicBlocks;
			}
		}

		[Test]
		public static void GetConnectedBlocksStartingAtHead()
		{
			var basicBlocks = Scenario1;

			var blocks = basicBlocks.GetConnectedBlocksStartingAtHead(basicBlocks[0]);

			Assert.AreEqual(blocks.Count, 8);
		}

		[Test]
		public void GetExitBlock1()
		{
			var basicBlocks = Scenario1;

			var block = basicBlocks.GetExitBlock(basicBlocks[0]);

			Assert.AreSame<BasicBlock>(basicBlocks[5], block);
		}

		[Test]
		public void GetExitBlock2()
		{
			var basicBlocks = Scenario2;

			var block = basicBlocks.GetExitBlock(basicBlocks[0]);

			Assert.AreSame<BasicBlock>(basicBlocks[3], block);
		}

		[Test]
		public void GetExitBlock3()
		{
			var basicBlocks = Scenario3;

			var block = basicBlocks.GetExitBlock(basicBlocks[0]);

			Assert.AreSame<BasicBlock>(basicBlocks[6], block);
		}

		[Test]
		public void GetExitBlock4()
		{
			var basicBlocks = Scenario4;

			var block = basicBlocks.GetExitBlock(basicBlocks[0]);

			Assert.AreSame<BasicBlock>(basicBlocks[12], block);
		}
	}
}
