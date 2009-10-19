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
using Mosa.Runtime;

namespace Mosa.Tools.Compiler
{
    /// <summary>
    /// Implements a mock jit service.
    /// </summary>
    /// <remarks>
    /// The mock jit service does not setup trampolines, but initializes the raw method instruction pointer to zero.
    /// </remarks>
    sealed class MockJitService : IJitService
    {
        #region IJitService Members

        public void SetupJit(Mosa.Runtime.Vm.RuntimeMethod method)
        {
            /* Do nothing in this mock, as we're compiling every method to native anyways. */
            method.Address = IntPtr.Zero;
        }

        #endregion // IJitService Members
    }
}
