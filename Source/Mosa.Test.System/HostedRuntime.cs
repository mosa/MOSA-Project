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

using Mosa.Runtime.TypeSystem;
using Mosa.Runtime.Memory;

namespace Mosa.Test.System
{

	/// <summary>
	/// Provides central runtime entry points for various features.
	/// </summary>
	public static class HostedRuntime
	{
		#region Data members

		/// <summary>
		/// The memory page manager of this runtime.
		/// </summary>
		public static IMemoryPageManager MemoryPageManager = new Win32MemoryPageManager();

		#endregion // Data members

		#region Internal Call Prototypes

		public unsafe static void* AllocateMemory(uint size)
		{
			void* memory = MemoryPageManager.Allocate(IntPtr.Zero, size, PageProtectionFlags.Read | PageProtectionFlags.Write | PageProtectionFlags.WriteCombine).ToPointer();
			if (memory == null)
			{
				throw new OutOfMemoryException();
			}

			return memory;
		}

		#endregion // Virtual Machine Call Prototypes

	}
}
