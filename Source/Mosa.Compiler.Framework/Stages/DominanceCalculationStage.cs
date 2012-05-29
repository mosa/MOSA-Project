/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Performs dominance calculations on basic blocks built by a previous compilation stage.
	/// </summary>
	public sealed class DominanceCalculationStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{
		#region Data members

		private Dictionary<BasicBlock, IDominanceProvider> dominanceProviders = new Dictionary<BasicBlock, IDominanceProvider>();

		#endregion // Data members

		#region Properties

		public IDominanceProvider GetDominanceProvider(BasicBlock basicBlock) { return dominanceProviders[basicBlock]; }

		#endregion // Properties

		#region IMethodCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			foreach (var headBlock in basicBlocks.HeadBlocks)
				dominanceProviders.Add(headBlock, new SimpleFastDominance(basicBlocks, headBlock));
		}

		#endregion // IMethodCompilerStage Members

	}
}
