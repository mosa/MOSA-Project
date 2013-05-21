﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 */

namespace Mosa.Internal
{
	public static class Runtime
	{
		private unsafe static uint* free = (uint*)(1024 * 1024 * 4);

		private unsafe static uint* AllocateMemory(uint size)
		{
			free = free + size;
			return free;
		}

		public static unsafe void* AllocateObject(void* methodTable, uint classSize)
		{
			// HACK: Add compiler architecture to the runtime
			uint nativeIntSize = 4;

			// An object has the following memory layout:
			//   - IntPtr MTable
			//   - IntPtr SyncBlock
			//   - 0 .. n object data fields

			uint allocationSize = ((2 * nativeIntSize) + classSize);

			void* memory = (void*)AllocateMemory(allocationSize);

			uint* destination = (uint*)memory;

			// FIXME: Memset((byte*)destination, 0, (int)allocationSize);
			destination[0] = (uint)methodTable;
			destination[1] = 0; // No sync block initially

			return memory;
		}

		public static unsafe void* AllocateArray(void* methodTable, uint elementSize, uint elements)
		{
			// HACK: Add compiler architecture to the runtime
			uint nativeIntSize = 4;

			// An array has the following memory layout:
			//   - IntPtr MTable
			//   - IntPtr SyncBlock
			//   - int length
			//   - ElementType[length] elements

			uint allocationSize = nativeIntSize + (uint)(elements * elementSize);
			void* memory = AllocateObject(methodTable, allocationSize);

			uint* destination = (uint*)memory;
			destination[2] = elements;

			return memory;
		}

		public static unsafe void* AllocateString(void* methodTable, uint length)
		{
			void* result = AllocateArray(methodTable, 2, length);
			return result;
		}

		public static unsafe bool IsInstanceOfType(void* methodTable, void* type)
		{
			return false; // TODO
		}
	}
}