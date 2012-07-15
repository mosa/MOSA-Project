/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using Mosa.Internal.Plug;

namespace Mosa.Internal
{
	public unsafe static class Runtime
	{
		private const uint nativeIntSize = 4;

		// This method will be plugged by "Mosa.Kernel.x86.KernelMemory.AllocateMemory"
		private static uint AllocateMemory(uint size) { return 0; }

		public static void* AllocateObject(void* methodTable, uint classSize)
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

		public static void* AllocateArray(void* methodTable, uint elementSize, uint elements)
		{
			// An array has the following memory layout:
			//   - IntPtr MTable
			//   - IntPtr SyncBlock
			//   - int length
			//   - ElementType[length] elements

			uint allocationSize = (nativeIntSize * 3) + (uint)(elements * elementSize);
			void* memory = (void*)AllocateMemory(allocationSize);

			uint* destination = (uint*)memory;
			destination[0] = (uint)methodTable;
			destination[1] = 0; // No sync block initially
			destination[2] = elements;

			return memory;
		}

		public static void* AllocateString(void* methodTable, uint length)
		{
			return AllocateArray(methodTable, 2, length);
		}

		public static uint IsInstanceOfType(uint methodTable, uint obj)
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

		public static uint IsInstanceOfInterfaceType(int interfaceSlot, uint obj)
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

		public static uint Castclass(uint methodTable, uint obj)
		{
			//TODO: Fake result
			return obj;
		}

		public static void* BoxChar(void* methodTable, uint classSize, char value)
		{
			byte* memory = (byte*)AllocateObject(methodTable, classSize);
			char* destination = (char*)(memory + (nativeIntSize * 2));
			destination[0] = value;
			return memory;
		}

		public static void* BoxBoolean(void* methodTable, uint classSize, bool value)
		{
			byte* memory = (byte*)AllocateObject(methodTable, classSize);
			bool* destination = (bool*)(memory + (nativeIntSize * 2));
			destination[0] = value;
			return memory;
		}

		public static void* BoxInt8(void* methodTable, uint classSize, sbyte value)
		{
			byte* memory = (byte*)AllocateObject(methodTable, classSize);
			sbyte* destination = (sbyte*)(memory + (nativeIntSize * 2));
			destination[0] = value;
			return memory;
		}

		public static void* BoxUInt8(void* methodTable, uint classSize, uint value)
		{
			byte* memory = (byte*)AllocateObject(methodTable, 4);
			uint* destination = (uint*)(memory + (nativeIntSize * 2));
			destination[0] = value;
			return memory;
		}

		public static void* BoxInt16(void* methodTable, uint classSize, short value)
		{
			byte* memory = (byte*)AllocateObject(methodTable, classSize);
			short* destination = (short*)(memory + (nativeIntSize * 2));
			destination[0] = value;
			return memory;
		}

		public static void* BoxUInt16(void* methodTable, uint classSize, ushort value)
		{
			byte* memory = (byte*)AllocateObject(methodTable, classSize);
			ushort* destination = (ushort*)(memory + (nativeIntSize * 2));
			destination[0] = value;
			return memory;
		}

		public static void* BoxInt32(void* methodTable, uint classSize, int value)
		{
			byte* memory = (byte*)AllocateObject(methodTable, classSize);
			int* destination = (int*)(memory + (nativeIntSize * 2));
			destination[0] = value;
			return memory;
		}

		public static void* BoxUInt32(void* methodTable, uint classSize, uint value)
		{
			byte* memory = (byte*)AllocateObject(methodTable, classSize);
			uint* destination = (uint*)(memory + (nativeIntSize * 2));
			destination[0] = value;
			return memory;
		}

		public static void* BoxInt64(void* methodTable, uint classSize, long value)
		{
			byte* memory = (byte*)AllocateObject(methodTable, classSize);
			long* destination = (long*)(memory + (nativeIntSize * 2));
			destination[0] = value;
			return memory;
		}

		public static void* BoxUInt64(void* methodTable, uint classSize, ulong value)
		{
			byte* memory = (byte*)AllocateObject(methodTable, classSize);
			ulong* destination = (ulong*)(memory + (nativeIntSize * 2));
			destination[0] = value;
			return memory;
		}

		public static void* BoxSingle(void* methodTable, uint classSize, float value)
		{
			byte* memory = (byte*)AllocateObject(methodTable, classSize);
			float* destination = (float*)(memory + (nativeIntSize * 2));
			destination[0] = value;
			return memory;
		}

		public static void* BoxDouble(void* methodTable, uint classSize, double value)
		{
			byte* memory = (byte*)AllocateObject(methodTable, classSize);
			double* destination = (double*)(memory + (nativeIntSize * 2));
			destination[0] = value;
			return memory;
		}

		public static char UnboxChar(void* obj)
		{
			byte* memory = (byte*)obj + (nativeIntSize * 2);
			return ((char*)memory)[0];
		}

		public static bool UnboxBoolean(void* obj)
		{
			byte* memory = (byte*)obj + (nativeIntSize * 2);
			return ((bool*)memory)[0];
		}

		public static sbyte UnboxInt8(void* obj)
		{
			byte* memory = (byte*)obj + (nativeIntSize * 2);
			return ((sbyte*)memory)[0];
		}

		public static byte UnboxUInt8(void* obj)
		{
			byte* memory = (byte*)obj + (nativeIntSize * 2);
			return ((byte*)memory)[0];
		}

		public static short UnboxInt16(void* obj)
		{
			byte* memory = (byte*)obj + (nativeIntSize * 2);
			return ((short*)memory)[0];
		}

		public static ushort UnboxUInt16(void* obj)
		{
			byte* memory = (byte*)obj + (nativeIntSize * 2);
			return ((ushort*)memory)[0];
		}

		public static int UnboxInt32(void* obj)
		{
			byte* memory = (byte*)obj + (nativeIntSize * 2);
			return ((int*)memory)[0];
		}

		public static uint UnboxUInt32(void* obj)
		{
			byte* memory = (byte*)obj + (nativeIntSize * 2);
			return ((uint*)memory)[0];
		}

		public static long UnboxInt64(void* obj)
		{
			byte* memory = (byte*)obj + (nativeIntSize * 2);
			return ((long*)memory)[0];
		}

		public static ulong UnboxUInt64(void* obj)
		{
			byte* memory = (byte*)obj + (nativeIntSize * 2);
			return ((ulong*)memory)[0];
		}

		public static float UnboxSingle(void* obj)
		{
			byte* memory = (byte*)obj + (nativeIntSize * 2);
			return ((float*)memory)[0];
		}

		public static double UnboxDouble(void* obj)
		{
			byte* memory = (byte*)obj + (nativeIntSize * 2);
			return ((double*)memory)[0];
		}

		public static void Throw(uint something)
		{

		}

		public static uint GetSizeOfObject(void* obj)
		{
			void* methodTable = (void*)((uint*)obj)[0];

			return GetSizeOfType((void*)methodTable);
		}

		public static uint GetSizeOfType(void* methodTable)
		{
			uint definitionTable = ((uint*)methodTable)[4];

			if (definitionTable == 0)
				return 0; // Not good

			uint sizeOf = ((uint*)definitionTable)[0];

			return sizeOf;
		}


	}
}
