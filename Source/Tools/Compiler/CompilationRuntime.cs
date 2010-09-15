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
		/// Gets the JIT service.
		/// </summary>
		/// <value>The JIT service.</value>
		public override IJitService JitService
		{
			get { return this.jitService; }
		}

	}
}
