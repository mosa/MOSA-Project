/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Diagnostics;

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

		private static unsafe void* memory = null;
		private static uint remaining = 0;

		#endregion // Data members

		#region Internal Call Prototypes

		public unsafe static void* AllocateMemory(uint size)
		{
			//if (size != size + 1)
			//    throw new Exception("AllocateMemory: " + size.ToString());

			//Debug.WriteLine("AllocateMemory: " + size.ToString());

			if (size >= 1024 * 10)
			{
				void* large = MemoryPageManager.Allocate(IntPtr.Zero, 1024 * 10, PageProtectionFlags.Read | PageProtectionFlags.Write | PageProtectionFlags.WriteCombine).ToPointer();

				if (large == null)
				{
					throw new OutOfMemoryException();
				}

				return large;
			}

			if (memory == null || remaining < size)
			{
				// 4Mb increments
				remaining = 1024 * 1024 * 4;
				memory = MemoryPageManager.Allocate(IntPtr.Zero, remaining, PageProtectionFlags.Read | PageProtectionFlags.Write | PageProtectionFlags.WriteCombine).ToPointer();

				if (memory == null)
				{
					throw new OutOfMemoryException();
				}
			}

			void* alloc = memory;
			remaining -= size;
			memory = ((byte*)memory) + size;

			return alloc;
		}

		#endregion // Virtual Machine Call Prototypes

	}
}
