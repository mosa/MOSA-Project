/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework.Ir;

namespace Mosa.Runtime.CompilerFramework
{

	/// <summary>
	/// Determines if the target of a function call can be inlined.
	/// </summary>
	public sealed class FunctionCallInliningProcessor : IMethodCompilerStage {

		#region Data members

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="NetOS.Runtime.CompilerFramework.FunctionCallInliningProcessor"/>.
		/// </summary>
		public FunctionCallInliningProcessor()
		{
		}

		#endregion // Construction

		#region IMethodCompilerStage Members

		string IMethodCompilerStage.Name
		{
			get { return @"Inlining optimization"; }
		}

		void IMethodCompilerStage.Run(MethodCompilerBase compiler)
		{
		}

		#endregion
	}
}
