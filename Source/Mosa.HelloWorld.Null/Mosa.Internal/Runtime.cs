/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Internal
{
	public unsafe static class Runtime
	{

		private static void* AllocateMemory(uint size)
		{
			return null; 
		}

		public static void* AllocateObject(void* methodTable, uint classSize)
		{
			return null; 
		}

		public static void* AllocateArray(void* methodTable, uint elementSize, uint elements)
		{
			return null; 
		}

		public static void* AllocateString(void* methodTable, uint length)
		{
			return null; 
		}

		public static uint IsInstanceOfType(uint methodTable, uint obj)
		{
			return 0;
		}

		public static uint IsInstanceOfInterfaceType(int interfaceSlot, uint obj)
		{
			return 0;
		}

		public static void* BoxChar(void* methodTable, uint classSize, char value)
		{
			return (void *)0;
		}

		public static void* BoxBoolean(void* methodTable, uint classSize, bool value)
		{
			return (void*)0;
		}

		public static void* BoxInt8(void* methodTable, uint classSize, sbyte value)
		{
			return (void*)0;
		}

		public static void* BoxUInt8(void* methodTable, uint classSize, uint value)
		{
			return (void*)0;
		}

		public static void* BoxInt16(void* methodTable, uint classSize, short value)
		{
			return (void*)0;
		}

		public static void* BoxUInt16(void* methodTable, uint classSize, ushort value)
		{
			return (void*)0;
		}

		public static void* BoxInt32(void* methodTable, uint classSize, int value)
		{
			return (void*)0;
		}

		public static void* BoxUInt32(void* methodTable, uint classSize, uint value)
		{
			return (void*)0;
		}

		public static void* BoxInt64(void* methodTable, uint classSize, long value)
		{
			return (void*)0;
		}

		public static void* BoxUInt64(void* methodTable, uint classSize, ulong value)
		{
			return (void*)0;
		}

		public static void* BoxSingle(void* methodTable, uint classSize, float value)
		{
			return (void*)0;
		}

		public static void* BoxDouble(void* methodTable, uint classSize, double value)
		{
			return (void*)0;
		}

		public static char UnboxChar(void* obj)
		{
			return ' ';
		}

		public static bool UnboxBoolean(void* obj)
		{
			return true;
		}

		public static sbyte UnboxInt8(void* obj)
		{
			return 0;
		}

		public static byte UnboxUInt8(void* obj)
		{
			return 0;
		}

		public static short UnboxInt16(void* obj)
		{
			return 0;
		}

		public static ushort UnboxUInt16(void* obj)
		{
			return 0;
		}

		public static int UnboxInt32(void* obj)
		{
			return 0;
		}

		public static uint UnboxUInt32(void* obj)
		{
			return 0;
		}

		public static long UnboxInt64(void* obj)
		{
			return 0;
		}

		public static ulong UnboxUInt64(void* obj)
		{
			return 0;
		}

		public static float UnboxSingle(void* obj)
		{
			return 0;
		}

		public static double UnboxDouble(void* obj)
		{
			return 0;
		}

		public static void Throw(uint something)
		{

		}

		public static uint GetSizeOfObject(void* obj)
		{
			return 0;
		}

		public static uint GetSizeOfType(void* methodTable)
		{
			return 0;
		}


	}
}
