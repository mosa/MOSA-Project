/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using MbUnit.Framework;
using Mosa.Compiler.Framework;

namespace Mosa.Test.Compiler.Framework
{
	[TestFixture]
	public class SimpleFastDominanceTests
	{

		internal static void Dump(BasicBlocks basicBlocks, IDominanceProvider provider)
		{

			foreach (var b in basicBlocks)
			{
				Console.WriteLine(b.ToString() + " -> " + provider.GetImmediateDominator(b));
			}

			Console.WriteLine();

			foreach (var b in basicBlocks)
			{
				Console.Write(b.ToString() + " -> ");

				foreach (var d in provider.GetDominators(b))
					Console.Write(d.ToString() + " ");

				Console.WriteLine();
			}

			return;
		}

		[Test]
		public static void DominanceCalculation1()
		{
			var basicBlocks = BlockTests.Scenario3;

			SimpleFastDominance dominance = new SimpleFastDominance(basicBlocks, basicBlocks[0]);

			IDominanceProvider provider = dominance as IDominanceProvider;

			Assert.AreSame<BasicBlock>(provider.GetImmediateDominator(basicBlocks[0]), null);
			Assert.AreSame<BasicBlock>(provider.GetImmediateDominator(basicBlocks[1]), basicBlocks[0]);
			Assert.AreSame<BasicBlock>(provider.GetImmediateDominator(basicBlocks[2]), basicBlocks[0]);
			Assert.AreSame<BasicBlock>(provider.GetImmediateDominator(basicBlocks[3]), basicBlocks[2]);
			Assert.AreSame<BasicBlock>(provider.GetImmediateDominator(basicBlocks[4]), basicBlocks[2]);
			Assert.AreSame<BasicBlock>(provider.GetImmediateDominator(basicBlocks[5]), basicBlocks[2]);
			Assert.AreSame<BasicBlock>(provider.GetImmediateDominator(basicBlocks[6]), basicBlocks[0]);

			Assert.AreElementsEqualIgnoringOrder<BasicBlock>(provider.GetDominators(basicBlocks[0]), new[] { basicBlocks[0] });
			Assert.AreElementsEqualIgnoringOrder<BasicBlock>(provider.GetDominators(basicBlocks[1]), new[] { basicBlocks[0], basicBlocks[1] });
			Assert.AreElementsEqualIgnoringOrder<BasicBlock>(provider.GetDominators(basicBlocks[2]), new[] { basicBlocks[0], basicBlocks[2] });
			Assert.AreElementsEqualIgnoringOrder<BasicBlock>(provider.GetDominators(basicBlocks[3]), new[] { basicBlocks[0], basicBlocks[2], basicBlocks[3] });
			Assert.AreElementsEqualIgnoringOrder<BasicBlock>(provider.GetDominators(basicBlocks[4]), new[] { basicBlocks[0], basicBlocks[2], basicBlocks[4] });
			Assert.AreElementsEqualIgnoringOrder<BasicBlock>(provider.GetDominators(basicBlocks[5]), new[] { basicBlocks[0], basicBlocks[2], basicBlocks[5] });
			Assert.AreElementsEqualIgnoringOrder<BasicBlock>(provider.GetDominators(basicBlocks[6]), new[] { basicBlocks[0], basicBlocks[6] });

			Dump(basicBlocks, dominance);
		}

		[Test]
		public static void DominanceCalculation2()
		{
			var basicBlocks = BlockTests.Scenario4;

			SimpleFastDominance dominance = new SimpleFastDominance(basicBlocks, basicBlocks[0]);

			IDominanceProvider provider = dominance as IDominanceProvider;

			Assert.AreSame<BasicBlock>(provider.GetImmediateDominator(basicBlocks[0]), null);
			Assert.AreSame<BasicBlock>(provider.GetImmediateDominator(basicBlocks[1]), basicBlocks[0]);
			Assert.AreSame<BasicBlock>(provider.GetImmediateDominator(basicBlocks[2]), basicBlocks[1]);
			Assert.AreSame<BasicBlock>(provider.GetImmediateDominator(basicBlocks[3]), basicBlocks[0]);
			Assert.AreSame<BasicBlock>(provider.GetImmediateDominator(basicBlocks[4]), basicBlocks[0]);
			Assert.AreSame<BasicBlock>(provider.GetImmediateDominator(basicBlocks[5]), basicBlocks[4]);
			Assert.AreSame<BasicBlock>(provider.GetImmediateDominator(basicBlocks[6]), basicBlocks[4]);
			Assert.AreSame<BasicBlock>(provider.GetImmediateDominator(basicBlocks[7]), basicBlocks[4]);
			Assert.AreSame<BasicBlock>(provider.GetImmediateDominator(basicBlocks[8]), basicBlocks[0]);
			Assert.AreSame<BasicBlock>(provider.GetImmediateDominator(basicBlocks[9]), basicBlocks[8]);
			Assert.AreSame<BasicBlock>(provider.GetImmediateDominator(basicBlocks[10]), basicBlocks[8]);
			Assert.AreSame<BasicBlock>(provider.GetImmediateDominator(basicBlocks[11]), basicBlocks[0]);
			Assert.AreSame<BasicBlock>(provider.GetImmediateDominator(basicBlocks[12]), basicBlocks[0]);

			Assert.AreElementsEqualIgnoringOrder<BasicBlock>(provider.GetDominators(basicBlocks[0]), new[] { basicBlocks[0] });
			Assert.AreElementsEqualIgnoringOrder<BasicBlock>(provider.GetDominators(basicBlocks[1]), new[] { basicBlocks[0], basicBlocks[1] });
			Assert.AreElementsEqualIgnoringOrder<BasicBlock>(provider.GetDominators(basicBlocks[2]), new[] { basicBlocks[0], basicBlocks[1], basicBlocks[2] });
			Assert.AreElementsEqualIgnoringOrder<BasicBlock>(provider.GetDominators(basicBlocks[3]), new[] { basicBlocks[0], basicBlocks[3] });
			Assert.AreElementsEqualIgnoringOrder<BasicBlock>(provider.GetDominators(basicBlocks[4]), new[] { basicBlocks[0], basicBlocks[4] });
			Assert.AreElementsEqualIgnoringOrder<BasicBlock>(provider.GetDominators(basicBlocks[5]), new[] { basicBlocks[0], basicBlocks[4], basicBlocks[5] });
			Assert.AreElementsEqualIgnoringOrder<BasicBlock>(provider.GetDominators(basicBlocks[6]), new[] { basicBlocks[0], basicBlocks[4], basicBlocks[6] });
			Assert.AreElementsEqualIgnoringOrder<BasicBlock>(provider.GetDominators(basicBlocks[7]), new[] { basicBlocks[0], basicBlocks[4], basicBlocks[7] });
			Assert.AreElementsEqualIgnoringOrder<BasicBlock>(provider.GetDominators(basicBlocks[8]), new[] { basicBlocks[0], basicBlocks[8] });
			Assert.AreElementsEqualIgnoringOrder<BasicBlock>(provider.GetDominators(basicBlocks[9]), new[] { basicBlocks[0], basicBlocks[8], basicBlocks[9] });
			Assert.AreElementsEqualIgnoringOrder<BasicBlock>(provider.GetDominators(basicBlocks[10]), new[] { basicBlocks[0], basicBlocks[8], basicBlocks[10] });
			Assert.AreElementsEqualIgnoringOrder<BasicBlock>(provider.GetDominators(basicBlocks[11]), new[] { basicBlocks[0], basicBlocks[11] });
			Assert.AreElementsEqualIgnoringOrder<BasicBlock>(provider.GetDominators(basicBlocks[12]), new[] { basicBlocks[0], basicBlocks[12] });

			Assert.AreElementsEqualIgnoringOrder<BasicBlock>(provider.GetDominanceFrontier(basicBlocks[4]), new[] { basicBlocks[3], basicBlocks[4], basicBlocks[11], basicBlocks[12] });

			Dump(basicBlocks, dominance);
		}
	}
}
