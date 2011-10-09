/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

using Mosa.Kernel;

namespace Mosa.Internal
{
	public static class Runtime
	{
		// HACK: Add compiler architecture to the runtime
		private const uint nativeIntSize = 4;

		private unsafe static void* AllocateMemory(uint size)
		{
			return (void*)KernelMemory.AllocateMemory(size);
		}

		public static unsafe void* AllocateObject(void* methodTable, uint classSize)
		{
			// An object has the following memory layout:
			//   - IntPtr MTable
			//   - IntPtr SyncBlock
			//   - 0 .. n object data fields

			uint allocationSize = (2 * nativeIntSize) + classSize;
			void* memory = (void*)AllocateMemory(allocationSize);

			uint* destination = (uint*)memory;
			destination[0] = (uint)methodTable;
			destination[1] = 0; // No sync block initially

			return memory;
		}

		public static unsafe void* AllocateArray(void* methodTable, uint elementSize, uint elements)
		{
			// An array has the following memory layout:
			//   - IntPtr MTable
			//   - IntPtr SyncBlock
			//   - int length
			//   - ElementType[length] elements

			uint allocationSize = (nativeIntSize * 3) + (uint)(elements * elementSize);
			void* memory = AllocateMemory(allocationSize);

			uint* destination = (uint*)memory;
			destination[0] = (uint)methodTable;
			destination[1] = 0; // No sync block initially
			destination[2] = elements;

			return memory;
		}

		public static unsafe void* AllocateString(void* methodTable, uint length)
		{
			return AllocateArray(methodTable, 2, length);
		}

		public static unsafe uint IsInstanceOfType(uint methodTable, uint obj)
		{
			if (obj == 0)
				return 0;

			uint objMethodTable = ((uint*)obj)[0];

			while (objMethodTable != 0)
			{
				if (objMethodTable == methodTable)
					return obj;

				objMethodTable = ((uint*)objMethodTable)[3];
			}

			return 0;
		}

		public static unsafe uint IsInstanceOfInterfaceType(int interfaceSlot, uint obj)
		{
			uint objMethodTable = ((uint*)obj)[0];

			if (objMethodTable == 0)
				return 0;

			uint bitmap = ((uint*)(objMethodTable))[2];

			if (bitmap == 0)
				return 0;

			int index = interfaceSlot / 32;
			int bit = interfaceSlot % 32;
			uint value = ((uint*)bitmap)[index];
			uint result = value & (uint)(1 << bit);

			if (result == 0)
				return 0;

			return obj;
		}

		public unsafe static void* BoxInt32(void* methodTable, uint classSize, int value)
		{
			byte* memory = (byte*)AllocateObject(methodTable, 4);
			byte* data = memory + (nativeIntSize * 2);

			int* destination = (int*)data;
			destination[0] = value;

			return memory;
		}

		public unsafe static void* BoxUInt32(void* methodTable, uint classSize, uint value)
		{
			byte* memory = (byte*)AllocateObject(methodTable, 4);
			byte* data = memory + (nativeIntSize * 2);

			uint* destination = (uint*)data;
			destination[0] = value;

			return memory;
		}

		public unsafe static void* BoxInt64(void* methodTable, uint classSize, long value)
		{
			byte* memory = (byte*)AllocateObject(methodTable, 8);
			byte* data = memory + (nativeIntSize * 2);

			long* destination = (long*)data;
			destination[0] = value;

			return memory;
		}

		public unsafe static void* BoxUInt64(void* methodTable, uint classSize, ulong value)
		{
			byte* memory = (byte*)AllocateObject(methodTable, 8);
			byte* data = memory + (nativeIntSize * 2);

			ulong* destination = (ulong*)data;
			destination[0] = value;

			return memory;
		}

		public unsafe static void* BoxSingle(void* methodTable, uint classSize, float value)
		{
			byte* memory = (byte*)AllocateObject(methodTable, 3);
			byte* data = memory + (nativeIntSize * 2);

			float* destination = (float*)data;
			destination[0] = value;

			return memory;
		}

		public unsafe static void* BoxDouble(void* methodTable, uint classSize, double value)
		{
			byte* memory = (byte*)AllocateObject(methodTable, 8);
			byte* data = memory + (nativeIntSize * 2);

			double* destination = (double*)data;
			destination[0] = value;

			return memory;
		}

		public unsafe static short UnboxInt16(void* data)
		{
			byte* memory = (byte*)data + (nativeIntSize * 2);
			return ((short*)memory)[0];
		}

		public unsafe static int UnboxInt32(void* data)
		{
			byte* memory = (byte*)data + (nativeIntSize * 2);
			return ((int*)memory)[0];
		}

		public unsafe static uint UnboxUInt32(void* data)
		{
			byte* memory = (byte*)data + (nativeIntSize * 2);
			return ((uint*)memory)[0];
		}

		public unsafe static long UnboxInt64(void* data)
		{
			byte* memory = (byte*)data + (nativeIntSize * 2);
			return ((long*)memory)[0];
		}

		public unsafe static ulong UnboxUInt64(void* data)
		{
			byte* memory = (byte*)data + (nativeIntSize * 2);
			return ((ulong*)memory)[0];
		}

		public unsafe static float UnboxSingle(void* data)
		{
			byte* memory = (byte*)data + (nativeIntSize * 2);
			return ((float*)memory)[0];
		}

		public unsafe static double UnboxDouble(void* data)
		{
			byte* memory = (byte*)data + (nativeIntSize * 2);
			return ((double*)memory)[0];
		}

		public static unsafe void Throw(uint something)
		{

		}
	}
}
