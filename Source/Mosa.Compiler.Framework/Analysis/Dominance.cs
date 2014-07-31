/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework.Analysis;
using System;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Analysis
{
	public sealed class Dominance
	{
		#region Data members

		private Func<IDominanceAnalysis> dominanceAnalysisFactory;

		private Dictionary<BasicBlock, IDominanceAnalysis> blockAnalysis = new Dictionary<BasicBlock, IDominanceAnalysis>();

		private BasicBlocks basicBlocks;

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
			IDominanceAnalysis analysis;

			if (!blockAnalysis.TryGetValue(headBlock, out analysis))
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