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

namespace Mosa.Runtime
{

	/// <summary>
	/// Provides central runtime entry points for various features.
	/// </summary>
	public abstract class BaseRuntime
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
		/// Gets the JIT service.
		/// </summary>
		/// <value>The JIT service.</value>
		public abstract IJitService JitService
		{
			get;
		}

		#endregion // Properties

	}
}
