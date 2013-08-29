 
using System;

namespace Mosa.Test.Collection
{

	public static class BoxingTests 
	{
			
		public static byte BoxU1(byte value) 
		{
			object o = value;
			return (byte)o;
		}

		
		public static ushort BoxU2(ushort value) 
		{
			object o = value;
			return (ushort)o;
		}

		
		public static uint BoxU4(uint value) 
		{
			object o = value;
			return (uint)o;
		}

		
		public static ulong BoxU8(ulong value) 
		{
			object o = value;
			return (ulong)o;
		}

		
		public static sbyte BoxI1(sbyte value) 
		{
			object o = value;
			return (sbyte)o;
		}

		
		public static short BoxI2(short value) 
		{
			object o = value;
			return (short)o;
		}

		
		public static int BoxI4(int value) 
		{
			object o = value;
			return (int)o;
		}

		
		public static long BoxI8(long value) 
		{
			object o = value;
			return (long)o;
		}

		
		public static float BoxR4(float value) 
		{
			object o = value;
			return (float)o;
		}

		
		public static double BoxR8(double value) 
		{
			object o = value;
			return (double)o;
		}

		
		public static char BoxC(char value) 
		{
			object o = value;
			return (char)o;
		}

		}
}
			