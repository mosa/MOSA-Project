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
using Mosa.Runtime.Memory;
using Mosa.Runtime;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Vm;
using Mosa.Jit.SimpleJit;

namespace Test.Mosa.Runtime.CompilerFramework.BaseCode
{
    /// <summary>
    /// An implementation of the MOSA runtime for test purposes.
    /// </summary>
    /// <remarks>
    /// Specifically this runtime uses Win32 virtual memory as a memory page 
    /// manager and it uses the default type system, default assembly loader
    /// and simple jit compiler.
    /// </remarks>
    sealed class TestRuntime : RuntimeBase
    {
        #region Data members

        /// <summary>
        /// The memory page manager of this runtime.
        /// </summary>
        private IMemoryPageManager _memoryPageManager;

        /// <summary>
        /// The type loader of this runtime.
        /// </summary>
        private ITypeSystem _typeLoader;

        /// <summary>
        /// The assembly loader of this runtime.
        /// </summary>
        private IAssemblyLoader _assemblyLoader;

        /// <summary>
        /// The jit service of this runtime.
        /// </summary>
        private IJitService _jitService;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="TestRuntime"/>.
        /// </summary>
        public TestRuntime()
        {
            _memoryPageManager = new Win32MemoryPageManager();
            _typeLoader = new DefaultTypeSystem();
            _assemblyLoader = new AssemblyLoader(_typeLoader);
            _jitService = new SimpleJitService();
        }

        #endregion // Construction

        #region RuntimeBase Overrides

        public override IMemoryPageManager MemoryManager
        {
            get { return _memoryPageManager; }
        }

        public override ITypeSystem TypeLoader
        {
            get { return _typeLoader; }
        }

        public override IAssemblyLoader AssemblyLoader
        {
            get { return _assemblyLoader; }
        }

        public override IJitService JitService
        {
            get { return _jitService; }
        }

        #endregion // RuntimeBase Overrides
    }
}
