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
using Mosa.Runtime.Memory;
using Mosa.Runtime.Vm;
using Mosa.Runtime.Loader;

namespace Mosa.Tools.Compiler
{
    /// <summary>
    /// Implementation of the Mosa runtime for Ahead-Of-Time compilation.
    /// </summary>
    /// <remarks>
    /// This runtime implementation uses various mock services and the default
    /// type system and assembly loader.
    /// </remarks>
    public class CompilationRuntime : RuntimeBase
    {
        #region Data members

        /// <summary>
        /// The assembly loader of this runtime.
        /// </summary>
        private IAssemblyLoader _assemblyLoader;

        /// <summary>
        /// The type loader of this runtime.
        /// </summary>
        private ITypeSystem _typeLoader;

        /// <summary>
        /// The memory page manager of this runtime.
        /// </summary>
        private IMemoryPageManager _memoryManager;

        /// <summary>
        /// The jit service of this runtime.
        /// </summary>
        private IJitService _jitService;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="CompilationRuntime"/>.
        /// </summary>
        public CompilationRuntime()
        {
            _typeLoader = new DefaultTypeSystem();
            _assemblyLoader = new AssemblyLoader(_typeLoader);
            _memoryManager = new MockMemoryPageManager();
            _jitService = new MockJitService();
        }

        #endregion // Construction

        #region RuntimeBase Overrides

        /// <summary>
        /// 
        /// </summary>
        public override IMemoryPageManager MemoryManager
        {
            get { return _memoryManager; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override ITypeSystem TypeLoader
        {
            get { return _typeLoader; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override IAssemblyLoader AssemblyLoader
        {
            get { return _assemblyLoader; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override Mosa.Runtime.IJitService JitService
        {
            get { return _jitService; }
        }

        #endregion // RuntimeBase Overrides
    }
}
