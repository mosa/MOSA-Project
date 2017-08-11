// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Analysis
{
	public sealed class Dominance
	{
		#region Data members

		private readonly Func<IDominanceAnalysis> dominanceAnalysisFactory;

		private readonly Dictionary<BasicBlock, IDominanceAnalysis> blockAnalysis = new Dictionary<BasicBlock, IDominanceAnalysis>();

		private readonly BasicBlocks basicBlocks;

		#endregion Data members

		#region Constructors

		public Dominance(Func<IDominanceAnalysis> dominanceAnalysisFactory, BasicBlocks basicBlocks)
		{
			this.dominanceAnalysisFactory = dominanceAnalysisFactory;
			this.basicBlocks = basicBlocks;
		}

		#endregion Constructors

		#region Properties

		public IDominanceAnalysis GetDominanceAnalysis(BasicBlock headBlock)
		{
			if (!blockAnalysis.TryGetValue(headBlock, out IDominanceAnalysis analysis))
			{
				analysis = dominanceAnalysisFactory();
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

		public void Clear()
		{
			blockAnalysis.Clear();
		}

		#endregion Members
	}
}
