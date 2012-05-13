/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Performs dominance calculations on basic blocks built by a previous compilation stage.
	/// </summary>
	public sealed class DominanceCalculationStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{
		#region Data members

		private IDominanceProvider dominanceProvider;

		#endregion // Data members

		#region Properties

		public IDominanceProvider DominanceProvider { get { return dominanceProvider; } }

		#endregion // Properties

		#region IMethodCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			if (AreExceptions)
				return;

			this.dominanceProvider = new SimpleFastDominance(basicBlocks, basicBlocks.PrologueBlock);
		}

		#endregion // IMethodCompilerStage Members

	}
}
