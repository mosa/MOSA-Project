// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Analysis
{
	public sealed class Dominance
	{
		#region Data members

		private readonly Dictionary<BasicBlock, BaseDominanceAnalysis> blockAnalysis = new Dictionary<BasicBlock, BaseDominanceAnalysis>();

		private readonly BasicBlocks basicBlocks;

		#endregion Data members

		#region Constructors

		public Dominance(BasicBlocks basicBlocks)
		{
			this.basicBlocks = basicBlocks;
		}

		#endregion Constructors

		#region Properties

		public BaseDominanceAnalysis GetDominanceAnalysis(BasicBlock headBlock)
		{
			if (!blockAnalysis.TryGetValue(headBlock, out BaseDominanceAnalysis analysis))
			{
				analysis = new SimpleFastDominance();
				analysis.PerformAnalysis(basicBlocks, headBlock);
				blockAnalysis.Add(headBlock, analysis);
			}

			return analysis;
		}

		#endregion Properties

		#region Members

		/// <summary>
		/// Analysises the basic blocks.
		/// </summary>
		public void Analysis()
		{
			foreach (var headBlock in basicBlocks.HeadBlocks)
			{
				GetDominanceAnalysis(headBlock);
			}
		}

		#endregion Members
	}
}
