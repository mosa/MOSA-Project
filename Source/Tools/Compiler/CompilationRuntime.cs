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
using System.IO;
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
        /// <summary>
        /// The assembly loader of this runtime.
        /// </summary>
        private IAssemblyLoader assemblyLoader;

        /// <summary>
        /// The type loader of this runtime.
        /// </summary>
        private ITypeSystem typeLoader;

        /// <summary>
        /// The memory page manager of this runtime.
        /// </summary>
        private IMemoryPageManager memoryManager;

        /// <summary>
        /// The jit service of this runtime.
        /// </summary>
        private IJitService jitService;

        /// <summary>
        /// Initializes a new instance of <see cref="CompilationRuntime"/>.
        /// </summary>
        public CompilationRuntime()
        {
            this.typeLoader = new DefaultTypeSystem();
            this.assemblyLoader = new AssemblyLoader(this.typeLoader);
            this.memoryManager = new MockMemoryPageManager();
            this.jitService = new MockJitService();
        }

        /// <summary>
        /// 
        /// </summary>
        public override IMemoryPageManager MemoryManager
        {
            get { return this.memoryManager; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override ITypeSystem TypeLoader
        {
            get { return this.typeLoader; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override IAssemblyLoader AssemblyLoader
        {
            get { return this.assemblyLoader; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override IJitService JitService
        {
            get { return this.jitService; }
        }

        public void InitializePrivatePaths(IEnumerable<string> assemblyPaths)
        {
            // Append the paths of the folder to the loader path);
            foreach (string path in this.FindPrivatePaths(assemblyPaths))
            {
                this.assemblyLoader.AppendPrivatePath(path);
            }
        }

        private IEnumerable<string> FindPrivatePaths(IEnumerable<string> assemblyPaths)
        {
            List<string> privatePaths = new List<string>();
            foreach (string assembly in assemblyPaths)
            {
                string path = Path.GetDirectoryName(assembly);
                if (!privatePaths.Contains(path))
                    privatePaths.Add(path);
            }

            return privatePaths;
        }
    }
}
