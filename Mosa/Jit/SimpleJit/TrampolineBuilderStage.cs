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

using Mosa.Runtime.CompilerFramework;

namespace Mosa.Jit.SimpleJit
{
    /// <summary>
    /// The first stage in a method compiler building the call trampoline.
    /// </summary>
    sealed class TrampolineBuilderStage : BaseStage, IMethodCompilerStage
    {

        #region IMethodCompilerStage Members

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public string Name
        {
            get { return @"TrampolineBuilderStage"; }
        }

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
        public override void Run(IMethodCompiler compiler)
        {
			base.Run(compiler);

            // Nothing to do here, the jit already gives us a list of instructions 
            // we need to compile so we don't have to do anything here.
        }

		/// <summary>
		/// Adds the stage to the pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline to add to.</param>
        public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
        {
            pipeline.Add(this);
        }

        #endregion // IMethodCompilerStage Members

    }
}
