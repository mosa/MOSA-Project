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
	public class FastDominanceTests
	{

		[Test]
		public static void DominanceCalculation()
		{
			var basicBlocks = BlockTests.Scenario3;

			FastDominance dominance = new FastDominance(basicBlocks, basicBlocks[0]);

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

	}
}
