/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework.Analysis;
using System;
using Xunit;

namespace Mosa.Compiler.Framework.xUnit
{
	public class SimpleFastDominanceTests
	{
		internal static void Dump(BasicBlocks basicBlocks, IDominanceAnalysis analysis)
		{
			foreach (var b in basicBlocks)
			{
				Console.WriteLine(b.ToString() + " -> " + analysis.GetImmediateDominator(b));
			}

			Console.WriteLine();

			foreach (var b in basicBlocks)
			{
				Console.Write(b.ToString() + " -> ");

				foreach (var d in analysis.GetDominators(b))
					Console.Write(d.ToString() + " ");

				Console.WriteLine();
			}

			return;
		}

		[Fact]
		public static void DominanceCalculation1()
		{
			var basicBlocks = BlockTests.Scenario3;

			SimpleFastDominanceAnalysis dominance = new SimpleFastDominanceAnalysis(basicBlocks, basicBlocks[0]);

			IDominanceAnalysis provider = dominance as IDominanceAnalysis;

			Assert.Same(provider.GetImmediateDominator(basicBlocks[0]), null);
			Assert.Same(provider.GetImmediateDominator(basicBlocks[1]), basicBlocks[0]);
			Assert.Same(provider.GetImmediateDominator(basicBlocks[2]), basicBlocks[0]);
			Assert.Same(provider.GetImmediateDominator(basicBlocks[3]), basicBlocks[2]);
			Assert.Same(provider.GetImmediateDominator(basicBlocks[4]), basicBlocks[2]);
			Assert.Same(provider.GetImmediateDominator(basicBlocks[5]), basicBlocks[2]);
			Assert.Same(provider.GetImmediateDominator(basicBlocks[6]), basicBlocks[0]);

			Assert.Equal(provider.GetDominators(basicBlocks[0]), new[] { basicBlocks[0] });
			//Assert.Equal(provider.GetDominators(basicBlocks[1]), new[] { basicBlocks[0], basicBlocks[1] });
			//Assert.Equal(provider.GetDominators(basicBlocks[2]), new[] { basicBlocks[0], basicBlocks[2] });
			//Assert.Equal(provider.GetDominators(basicBlocks[3]), new[] { basicBlocks[0], basicBlocks[2], basicBlocks[3] });
			//Assert.Equal(provider.GetDominators(basicBlocks[4]), new[] { basicBlocks[0], basicBlocks[2], basicBlocks[4] });
			//Assert.Equal(provider.GetDominators(basicBlocks[5]), new[] { basicBlocks[0], basicBlocks[2], basicBlocks[5] });
			//Assert.Equal(provider.GetDominators(basicBlocks[6]), new[] { basicBlocks[0], basicBlocks[6] });

			Dump(basicBlocks, dominance);
		}

		[Fact]
		public static void DominanceCalculation2()
		{
			var basicBlocks = BlockTests.Scenario4;

			SimpleFastDominanceAnalysis dominance = new SimpleFastDominanceAnalysis(basicBlocks, basicBlocks[0]);

			IDominanceAnalysis provider = dominance as IDominanceAnalysis;

			Assert.Same(provider.GetImmediateDominator(basicBlocks[0]), null);
			Assert.Same(provider.GetImmediateDominator(basicBlocks[1]), basicBlocks[0]);
			Assert.Same(provider.GetImmediateDominator(basicBlocks[2]), basicBlocks[1]);
			Assert.Same(provider.GetImmediateDominator(basicBlocks[3]), basicBlocks[0]);
			Assert.Same(provider.GetImmediateDominator(basicBlocks[4]), basicBlocks[0]);
			Assert.Same(provider.GetImmediateDominator(basicBlocks[5]), basicBlocks[4]);
			Assert.Same(provider.GetImmediateDominator(basicBlocks[6]), basicBlocks[4]);
			Assert.Same(provider.GetImmediateDominator(basicBlocks[7]), basicBlocks[4]);
			Assert.Same(provider.GetImmediateDominator(basicBlocks[8]), basicBlocks[0]);
			Assert.Same(provider.GetImmediateDominator(basicBlocks[9]), basicBlocks[8]);
			Assert.Same(provider.GetImmediateDominator(basicBlocks[10]), basicBlocks[8]);
			Assert.Same(provider.GetImmediateDominator(basicBlocks[11]), basicBlocks[0]);
			Assert.Same(provider.GetImmediateDominator(basicBlocks[12]), basicBlocks[0]);

			Assert.Equal(provider.GetDominators(basicBlocks[0]), new[] { basicBlocks[0] });
			//Assert.Equal(provider.GetDominators(basicBlocks[1]), new[] { basicBlocks[0], basicBlocks[1] });
			//Assert.Equal(provider.GetDominators(basicBlocks[2]), new[] { basicBlocks[0], basicBlocks[1], basicBlocks[2] });
			//Assert.Equal(provider.GetDominators(basicBlocks[3]), new[] { basicBlocks[0], basicBlocks[3] });
			//Assert.Equal(provider.GetDominators(basicBlocks[4]), new[] { basicBlocks[0], basicBlocks[4] });
			//Assert.Equal(provider.GetDominators(basicBlocks[5]), new[] { basicBlocks[0], basicBlocks[4], basicBlocks[5] });
			//Assert.Equal(provider.GetDominators(basicBlocks[6]), new[] { basicBlocks[0], basicBlocks[4], basicBlocks[6] });
			//Assert.Equal(provider.GetDominators(basicBlocks[7]), new[] { basicBlocks[0], basicBlocks[4], basicBlocks[7] });
			//Assert.Equal(provider.GetDominators(basicBlocks[8]), new[] { basicBlocks[0], basicBlocks[8] });
			//Assert.Equal(provider.GetDominators(basicBlocks[9]), new[] { basicBlocks[0], basicBlocks[8], basicBlocks[9] });
			//Assert.Equal(provider.GetDominators(basicBlocks[10]), new[] { basicBlocks[0], basicBlocks[8], basicBlocks[10] });
			//Assert.Equal(provider.GetDominators(basicBlocks[11]), new[] { basicBlocks[0], basicBlocks[11] });
			//Assert.Equal(provider.GetDominators(basicBlocks[12]), new[] { basicBlocks[0], basicBlocks[12] });

			//Assert.Equal(provider.GetDominanceFrontier(basicBlocks[4]), new[] { basicBlocks[3], basicBlocks[4], basicBlocks[11], basicBlocks[12] });

			Dump(basicBlocks, dominance);
		}
	}
}