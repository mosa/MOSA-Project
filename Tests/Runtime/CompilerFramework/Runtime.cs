/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Runtime.CompilerServices;

using Mosa.Runtime.Vm;
using Mosa.Runtime.Memory;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime;

namespace Test.Mosa.Runtime.CompilerFramework
{

	/// <summary>
	/// Provides central runtime entry points for various features.
	/// </summary>
	public static class Runtime
	{
		#region Static data members

		/// <summary>
		/// Holds the static instance of the runtime.
		/// </summary>
		public static RuntimeBase RuntimeBase;

		#endregion // Static data members

		#region Properties

		/// <summary>
		/// Retrieves the memory manager.
		/// </summary>
		/// <value>The memory manager.</value>
		public static IMemoryPageManager MemoryManager { get { return RuntimeBase.MemoryManager; } }

		/// <summary>
		/// Retrieves the type loader of the runtime.
		/// </summary>
		/// <value>The type loader.</value>
		public static ITypeSystem TypeLoader { get { return RuntimeBase.TypeLoader; } }
		/// <summary>
		/// Gets the assembly loader.
		/// </summary>
		/// <value>The assembly loader.</value>
		public static IAssemblyLoader AssemblyLoader { get { return RuntimeBase.AssemblyLoader; } }

		/// <summary>
		/// Gets the JIT service.
		/// </summary>
		/// <value>The JIT service.</value>
		public static IJitService JitService { get { return RuntimeBase.JitService; } }

		#endregion // Properties

	}
}
