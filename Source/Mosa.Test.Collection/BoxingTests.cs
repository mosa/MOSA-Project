/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com> 
 *
 */


namespace Mosa.Test.Collection
{

	public static class BoxingTests 
	{
			
		public static bool BoxU1(byte value) 
		{
			object o = value;
			return (byte)o == value;
		}

		
		public static bool BoxU2(ushort value) 
		{
			object o = value;
			return (ushort)o == value;
		}

		
		public static bool BoxU4(uint value) 
		{
			object o = value;
			return (uint)o == value;
		}

		
		public static bool BoxU8(ulong value) 
		{
			object o = value;
			return (ulong)o == value;
		}

		
		public static bool BoxI1(sbyte value) 
		{
			object o = value;
			return (sbyte)o == value;
		}

		
		public static bool BoxI2(short value) 
		{
			object o = value;
			return (short)o == value;
		}

		
		public static bool BoxI4(int value) 
		{
			object o = value;
			return (int)o == value;
		}

		
		public static bool BoxI8(long value) 
		{
			object o = value;
			return (long)o == value;
		}

		
		public static bool BoxR4(float value) 
		{
			object o = value;
			return (float)o == value;
		}

		
		public static bool BoxR8(double value) 
		{
			object o = value;
			return (double)o == value;
		}

		
		public static bool BoxC(char value) 
		{
			object o = value;
			return (char)o == value;
		}

		}
}
			