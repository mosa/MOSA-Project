using System;

namespace Mosa.Internal
{
	public static class Runtime
	{
		public static unsafe void* AllocateObject(void* methodTable, uint classSize)
		{
			return null;
		}

		public static unsafe void* AllocateArray(void* methodTable, uint elementSize, uint elements)
		{
			return null;
		}

		public static object Box(ValueType valueType)
		{
			return null;
		}

		public static object Castclass(object obj, UIntPtr typeHandle)
		{
			return null;
		}

		public static unsafe void* IsInstanceOfType(void* methodTable, void* obj)
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

		public static unsafe void* IsInstanceOfInterfaceType(void* methodTable, int interfaceSlot, void* obj)
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

		public unsafe static void Memcpy(byte* destination, byte* source, int count)
		{
		}

		public unsafe static void Memset(byte* destination, byte value, int count)
		{
		}

		public static void Rethrow()
		{
		}

		public static void Throw(object exception)
		{
		}

		public static void Unbox(object obj, ValueType valueType)
		{
		}
	}
}
