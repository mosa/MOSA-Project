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
	public class CompilationRuntime : BaseRuntime
	{
		/// <summary>
		/// The assembly loader of this runtime.
		/// </summary>
		private IAssemblyLoader assemblyLoader;

		/// <summary>
		/// The type loader of this runtime.
		/// </summary>
		private ITypeSystem typeSystem;

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
			this.typeSystem = new DefaultTypeSystem(this);
			this.assemblyLoader = new AssemblyLoader(typeSystem);
			this.memoryManager = new MockMemoryPageManager();
			this.jitService = new MockJitService();
		}

		/// <summary>
		/// Retrieves the memory manager.
		/// </summary>
		/// <value>The memory manager.</value>
		public override IMemoryPageManager MemoryManager
		{
			get { return this.memoryManager; }
		}

		/// <summary>
		/// Retrieves the type loader of the runtime.
		/// </summary>
		/// <value>The type loader.</value>
		public override ITypeSystem TypeSystem
		{
			get { return this.typeSystem; }
		}

		/// <summary>
		/// Gets the assembly loader.
		/// </summary>
		/// <value>The assembly loader.</value>
		public override IAssemblyLoader AssemblyLoader
		{
			get { return this.assemblyLoader; }
			set { this.assemblyLoader = value; } // HACK
		}

		/// <summary>
		/// Gets the JIT service.
		/// </summary>
		/// <value>The JIT service.</value>
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

		/// <summary>
		/// Finds the private paths.
		/// </summary>
		/// <param name="assemblyPaths">The assembly paths.</param>
		/// <returns></returns>
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
