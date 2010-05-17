/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.IO;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.Jit.SimpleJit
{
    /// <summary>
    /// The simple jit method compiler
    /// </summary>
    sealed class MethodCompiler : MethodCompilerBase
    {
        #region Data members

        /// <summary>
        /// The code stream, where the final code is emitted to.
        /// </summary>
        private Stream _codeStream;

        #endregion // Data members

        #region Construction

        public MethodCompiler(IAssemblyLinker linker, IArchitecture architecture, ICompilationSchedulerStage compilationScheduler, RuntimeType type, RuntimeMethod method, Stream codeStream) :
            base(linker, architecture, compilationScheduler, type, method)
        {
            if (null == codeStream)
                throw new ArgumentNullException(@"codeStream");

            _codeStream = codeStream;
        }

        #endregion // Construction

        #region MethodCompilerBase Overrides

        #endregion // MethodCompilerBase Overrides
    }
}
