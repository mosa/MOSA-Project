/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 */

using Mosa.Kernel;
using Mosa.Kernel.x86;
using System;

namespace Mosa.Internal
{
	public static class Runtime
	{
		public static object Box(ValueType valueType)
		{
			return valueType;
		}

		public unsafe static void* BoxInt32(void* methodTable, uint classSize, int value)
		{
			void* memory = (void*)AllocateMemory(4);

			uint* destination = (uint*)memory;
			destination[0] = (uint)value;

			return memory;
		}

		public unsafe static int UnboxInt32(void* data)
		{
			return ((int*)data)[0];
		}

		private unsafe static uint* AllocateMemory(uint size)
		{
			return (uint*)KernelGCMemory.AllocateMemory(size);
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
			return AllocateArray(methodTable, 2, length);
		}

		public static unsafe void* IsInstanceOfType2(void* methodTable, void* obj)
		{
			if (obj == null)
				return null;

			uint* objMethodTable = (uint*)((uint*)obj)[0];

			while (objMethodTable != null)
			{
				if (objMethodTable == methodTable)
					return obj;

				objMethodTable = (uint*)objMethodTable[3];
			}

			return null;
		}

		public static unsafe uint IsInstanceOfType(uint methodTable, uint obj)
		{
			if (obj == 0x0)
				return 0;

			uint objMethodTable = (uint)((uint*)obj)[0];

			while (objMethodTable != 0x0)
			{
				if (objMethodTable == methodTable)
					return obj;

				objMethodTable = objMethodTable + 3 * 4;
				objMethodTable = (uint)((uint*)objMethodTable)[0];
			}

			return 0x0;
		}

		public static unsafe void* IsInstanceOfInterfaceType(int interfaceSlot, void* obj)
		{
			uint* objMethodTable = (uint*)((uint*)obj)[0];
			uint* bitmap = (uint*)objMethodTable[2];

			if (bitmap == null)
				return null;

			int index = interfaceSlot / sizeof(int);
			int bit = interfaceSlot % sizeof(int);
			uint value = bitmap[index] & (uint)(1 << bit);

			if (value == 0)
				return null;

			return obj;
		}

		public static unsafe void Throw(uint something)
		{

		}
	}
}
