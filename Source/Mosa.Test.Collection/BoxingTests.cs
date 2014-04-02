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

		public static bool EqualsU1(byte value)
		{
			object o = value;
			return o.Equals(value);
		}
	
	
		public static ushort BoxU2(ushort value) 
		{
			object o = value;
			return (ushort)o;
		}

		public static bool EqualsU2(ushort value)
		{
			object o = value;
			return o.Equals(value);
		}
	
	
		public static uint BoxU4(uint value) 
		{
			object o = value;
			return (uint)o;
		}

		public static bool EqualsU4(uint value)
		{
			object o = value;
			return o.Equals(value);
		}
	
	
		public static ulong BoxU8(ulong value) 
		{
			object o = value;
			return (ulong)o;
		}

		public static bool EqualsU8(ulong value)
		{
			object o = value;
			return o.Equals(value);
		}
	
	
		public static sbyte BoxI1(sbyte value) 
		{
			object o = value;
			return (sbyte)o;
		}

		public static bool EqualsI1(sbyte value)
		{
			object o = value;
			return o.Equals(value);
		}
	
	
		public static short BoxI2(short value) 
		{
			object o = value;
			return (short)o;
		}

		public static bool EqualsI2(short value)
		{
			object o = value;
			return o.Equals(value);
		}
	
	
		public static int BoxI4(int value) 
		{
			object o = value;
			return (int)o;
		}

		public static bool EqualsI4(int value)
		{
			object o = value;
			return o.Equals(value);
		}
	
	
		public static long BoxI8(long value) 
		{
			object o = value;
			return (long)o;
		}

		public static bool EqualsI8(long value)
		{
			object o = value;
			return o.Equals(value);
		}
	
	
		public static float BoxR4(float value) 
		{
			object o = value;
			return (float)o;
		}

		public static bool EqualsR4(float value)
		{
			object o = value;
			return o.Equals(value);
		}
	
	
		public static double BoxR8(double value) 
		{
			object o = value;
			return (double)o;
		}

		public static bool EqualsR8(double value)
		{
			object o = value;
			return o.Equals(value);
		}
	
	
		public static char BoxC(char value) 
		{
			object o = value;
			return (char)o;
		}

		public static bool EqualsC(char value)
		{
			object o = value;
			return o.Equals(value);
		}
	
	}
}
