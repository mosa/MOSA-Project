/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using Mosa.Runtime.Vm;
using Mosa.Runtime.Memory;
using Mosa.Runtime.Loader;
using System.Runtime.CompilerServices;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime {
	
	/// <summary>
	/// Provides central runtime entry points for various features.
	/// </summary>
	public abstract class BaseRuntime : IDisposable
	{

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseRuntime"/> class.
		/// </summary>
		protected BaseRuntime()
		{
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Retrieves the memory manager.
		/// </summary>
		/// <value>The memory manager.</value>
		public abstract IMemoryPageManager MemoryManager
		{
			get;
		}

		/// <summary>
		/// Retrieves the type loader of the runtime.
		/// </summary>
		/// <value>The type loader.</value>
		public abstract ITypeSystem TypeSystem
		{
			get;
		}

		/// <summary>
		/// Gets the assembly loader.
		/// </summary>
		/// <value>The assembly loader.</value>
		public abstract IAssemblyLoader AssemblyLoader
		{
			get;
		}

		/// <summary>
		/// Gets the JIT service.
		/// </summary>
		/// <value>The JIT service.</value>
		public abstract IJitService JitService
		{
			get;
		}

		#endregion // Properties

		#region IDisposable Members

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			foreach (IMetadataModule module in AssemblyLoader.Modules)
				AssemblyLoader.Unload(module);
		}

		#endregion // IDisposable Members
	}
}
