/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

using CIL = Mosa.Runtime.CompilerFramework.CIL;

namespace Mosa.Runtime.CompilerFramework
{
	public sealed class ExceptionLayoutStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{
		#region Data members

		#endregion // Data members

		#region ExceptionClauseNode

		class ExceptionClauseNode
		{
			public ExceptionClause ExceptionClause { get; private set; }
			public ExceptionClauseNode Parent { get; set; }

			public ExceptionClauseNode(ExceptionClause exceptionClause)
			{
				ExceptionClause = exceptionClause;
			}
		}

		#endregion // ExceptionClauseNode

		#region IPipelineStage members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value></value>
		string IPipelineStage.Name { get { return @"ExceptionLayoutStage"; } }

		#endregion // IPipelineStage Members

		#region IMethodCompilerStage members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public void Run()
		{

		}

		#endregion // IMethodCompilerStage members

	}
}
