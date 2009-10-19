/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

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
		/// Initializes a new instance of <see cref="Mosa.Runtime.CompilerFramework.FunctionCallInliningProcessor"/>.
		/// </summary>
		public FunctionCallInliningProcessor()
		{
		}

		#endregion // Construction

		#region IMethodCompilerStage Members

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
		public string Name
		{
			get { return @"Inlining optimization"; }
		}

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
		public void Run(IMethodCompiler compiler)
		{
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipeline"></param>
        public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
        {
        }

		#endregion
	}
}
