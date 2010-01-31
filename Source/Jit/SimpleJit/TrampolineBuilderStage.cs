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
        public void Run()
        {
            // Nothing to do here, the jit already gives us a list of instructions 
            // we need to compile so we don't have to do anything here.
        }

        #endregion // IMethodCompilerStage Members

    }
}
