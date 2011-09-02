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

using Mosa.Test.System;

namespace Mosa.Internal
{

	/// <summary>
	/// Provides central runtime entry points for various features.
	/// </summary>
	public static class Runtime
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

		public static unsafe void* AllocateObject(void* methodTable, uint classSize)
		{
			// HACK: Add compiler architecture to the runtime
			uint nativeIntSize = 4;

			// An object has the following memory layout:
			//   - IntPtr MTable
			//   - IntPtr SyncBlock
			//   - 0 .. n object data fields
			//
			ulong allocationSize = (ulong)((2 * nativeIntSize) + classSize);

			void* memory = MemoryPageManager.Allocate(IntPtr.Zero, allocationSize, PageProtectionFlags.Read | PageProtectionFlags.Write | PageProtectionFlags.WriteCombine).ToPointer();
			if (memory == null)
			{
				throw new OutOfMemoryException();
			}

			uint* destination = (uint*)memory;
			//Memset((byte*)destination, 0, (int)allocationSize);
			destination[0] = (uint)methodTable;
			destination[1] = 0; // No sync block initially

			return memory;
		}

		/// <summary>
		/// This function requests allocation of an array.
		/// </summary>
		/// <param name="methodTable">Pointer to the array method table.</param>
		/// <param name="elementSize">The size of a single element in the method table.</param>
		/// <param name="elements">The number of elements to allocate of the type.</param>
		/// <returns>A ptr to the allocated memory.</returns>
		/// <remarks>
		/// The allocated object is not constructed, e.g. the caller must invoke
		/// the appropriate constructor in order to obtain a real object. The object header
		/// has been set.
		/// </remarks>
		public static unsafe void* AllocateArray(void* methodTable, uint elementSize, uint elements)
		{
			if (elements < 0)
			{
				throw new OverflowException();
			}

			// HACK: Add compiler architecture to the runtime
			uint nativeIntSize = 4;

			//
			// An array has the following memory layout:
			//   - IntPtr MTable
			//   - IntPtr SyncBlock
			//   - int length
			//   - ElementType[length] elements
			//
			uint allocationSize = (uint)(nativeIntSize + (elements * elementSize));

			void* memory = AllocateObject(methodTable, allocationSize);

			uint* destination = (uint*)memory;
			//Memset((byte*)(destination + 3), 0, (int)allocationSize);
			destination[2] = elements;

			return memory;
		}

		#endregion // Virtual Machine Call Prototypes

	}
}
