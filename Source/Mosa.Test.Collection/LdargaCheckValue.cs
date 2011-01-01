 
/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com> 
 *
 */

using System;

namespace Mosa.Test.Collection
{

	public static class LdargaCheckValue 
	{
	
		public static bool LdargaCheckValueI1(sbyte expect, sbyte a) 
		{
			return LdargaCheckValueRefI1(expect, ref a);
		}

		public static bool LdargaCheckValueRefI1(sbyte expect, ref sbyte a)
		{
			return (expect == a);
		}
	
		public static bool LdargaCheckValueI2(short expect, short a) 
		{
			return LdargaCheckValueRefI2(expect, ref a);
		}

		public static bool LdargaCheckValueRefI2(short expect, ref short a)
		{
			return (expect == a);
		}
	
		public static bool LdargaCheckValueI4(int expect, int a) 
		{
			return LdargaCheckValueRefI4(expect, ref a);
		}

		public static bool LdargaCheckValueRefI4(int expect, ref int a)
		{
			return (expect == a);
		}
	
		public static bool LdargaCheckValueI8(long expect, long a) 
		{
			return LdargaCheckValueRefI8(expect, ref a);
		}

		public static bool LdargaCheckValueRefI8(long expect, ref long a)
		{
			return (expect == a);
		}
	
		public static bool LdargaCheckValueU1(byte expect, byte a) 
		{
			return LdargaCheckValueRefU1(expect, ref a);
		}

		public static bool LdargaCheckValueRefU1(byte expect, ref byte a)
		{
			return (expect == a);
		}
	
		public static bool LdargaCheckValueU2(ushort expect, ushort a) 
		{
			return LdargaCheckValueRefU2(expect, ref a);
		}

		public static bool LdargaCheckValueRefU2(ushort expect, ref ushort a)
		{
			return (expect == a);
		}
	
		public static bool LdargaCheckValueU4(uint expect, uint a) 
		{
			return LdargaCheckValueRefU4(expect, ref a);
		}

		public static bool LdargaCheckValueRefU4(uint expect, ref uint a)
		{
			return (expect == a);
		}
	
		public static bool LdargaCheckValueU8(ulong expect, ulong a) 
		{
			return LdargaCheckValueRefU8(expect, ref a);
		}

		public static bool LdargaCheckValueRefU8(ulong expect, ref ulong a)
		{
			return (expect == a);
		}
	
		public static bool LdargaCheckValueR4(float expect, float a) 
		{
			return LdargaCheckValueRefR4(expect, ref a);
		}

		public static bool LdargaCheckValueRefR4(float expect, ref float a)
		{
			return (expect == a);
		}
	
		public static bool LdargaCheckValueR8(double expect, double a) 
		{
			return LdargaCheckValueRefR8(expect, ref a);
		}

		public static bool LdargaCheckValueRefR8(double expect, ref double a)
		{
			return (expect == a);
		}
		}
}
 