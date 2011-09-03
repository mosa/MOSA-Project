using System;

namespace Mosa.Internal
{
	public static class Runtime
	{

		private unsafe static void* AllocateMemory(uint size)
		{
			return null;
		}

		public static unsafe void* AllocateObject(void* methodTable, uint classSize)
		{
			return null;
		}

		public static unsafe void* AllocateArray(void* methodTable, uint elementSize, uint elements)
		{
			return null;
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

		public unsafe static void* BoxUInt32(void* methodTable, uint classSize, uint value)
		{
			void* memory = (void*)AllocateMemory(4);

			uint* destination = (uint*)memory;
			destination[0] = (uint)value;

			return memory;
		}

		public unsafe static void* BoxSingle(void* methodTable, uint classSize, float value)
		{
			void* memory = (void*)AllocateMemory(4);

			float* destination = (float*)memory;
			destination[0] = (float)value;

			return memory;
		}

		public unsafe static void* BoxDouble(void* methodTable, uint classSize, double value)
		{
			void* memory = (void*)AllocateMemory(4);

			double* destination = (double*)memory;
			destination[0] = (double)value;

			return memory;
		}

		public unsafe static int UnboxInt16(void* data)
		{
			return ((int*)data)[0];
		}

		public unsafe static int UnboxInt32(void* data)
		{
			return ((int*)data)[0];
		}

		public unsafe static uint UnboxUInt32(void* data)
		{
			return ((uint*)data)[0];
		}

		public unsafe static float UnboxSingle(void* data)
		{
			return ((float*)data)[0];
		}

		public unsafe static double UnboxDouble(void* data)
		{
			return ((double*)data)[0];
		}

		public static unsafe void Throw(uint something)
		{

		}

	}
}
