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
using System.IO;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Vm;
using Mosa.Runtime.Loader;

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

        public MethodCompiler(IArchitecture architecture, IMetadataModule module, RuntimeType type, RuntimeMethod method, Stream codeStream) :
            base(architecture, module, type, method)
        {
            if (null == codeStream)
                throw new ArgumentNullException(@"codeStream");

            _codeStream = codeStream;
        }

        #endregion // Construction

        #region MethodCompilerBase Overrides

        public override Stream RequestCodeStream()
        {
            return _codeStream;
        }

        #endregion // MethodCompilerBase Overrides
    }
}
