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

	public static class LdlocaTests 
	{
	
		public static bool LdlocaCheckValueU1(byte expect) 
		{
			byte a = expect;
			return LdlocaCheckValueRefU1(expect, ref a);
		}

		private static bool LdlocaCheckValueRefU1(byte expect, ref byte a)
		{
			return (expect.Equals(a));
		}
	
		public static bool LdlocaCheckValueU2(ushort expect) 
		{
			ushort a = expect;
			return LdlocaCheckValueRefU2(expect, ref a);
		}

		private static bool LdlocaCheckValueRefU2(ushort expect, ref ushort a)
		{
			return (expect.Equals(a));
		}
	
		public static bool LdlocaCheckValueU4(uint expect) 
		{
			uint a = expect;
			return LdlocaCheckValueRefU4(expect, ref a);
		}

		private static bool LdlocaCheckValueRefU4(uint expect, ref uint a)
		{
			return (expect.Equals(a));
		}
	
		public static bool LdlocaCheckValueU8(ulong expect) 
		{
			ulong a = expect;
			return LdlocaCheckValueRefU8(expect, ref a);
		}

		private static bool LdlocaCheckValueRefU8(ulong expect, ref ulong a)
		{
			return (expect.Equals(a));
		}
	
		public static bool LdlocaCheckValueI1(sbyte expect) 
		{
			sbyte a = expect;
			return LdlocaCheckValueRefI1(expect, ref a);
		}

		private static bool LdlocaCheckValueRefI1(sbyte expect, ref sbyte a)
		{
			return (expect.Equals(a));
		}
	
		public static bool LdlocaCheckValueI2(short expect) 
		{
			short a = expect;
			return LdlocaCheckValueRefI2(expect, ref a);
		}

		private static bool LdlocaCheckValueRefI2(short expect, ref short a)
		{
			return (expect.Equals(a));
		}
	
		public static bool LdlocaCheckValueI4(int expect) 
		{
			int a = expect;
			return LdlocaCheckValueRefI4(expect, ref a);
		}

		private static bool LdlocaCheckValueRefI4(int expect, ref int a)
		{
			return (expect.Equals(a));
		}
	
		public static bool LdlocaCheckValueI8(long expect) 
		{
			long a = expect;
			return LdlocaCheckValueRefI8(expect, ref a);
		}

		private static bool LdlocaCheckValueRefI8(long expect, ref long a)
		{
			return (expect.Equals(a));
		}
	
		public static bool LdlocaCheckValueR4(float expect) 
		{
			float a = expect;
			return LdlocaCheckValueRefR4(expect, ref a);
		}

		private static bool LdlocaCheckValueRefR4(float expect, ref float a)
		{
			return (expect.Equals(a));
		}
	
		public static bool LdlocaCheckValueR8(double expect) 
		{
			double a = expect;
			return LdlocaCheckValueRefR8(expect, ref a);
		}

		private static bool LdlocaCheckValueRefR8(double expect, ref double a)
		{
			return (expect.Equals(a));
		}
	
		public static bool LdlocaCheckValueC(char expect) 
		{
			char a = expect;
			return LdlocaCheckValueRefC(expect, ref a);
		}

		private static bool LdlocaCheckValueRefC(char expect, ref char a)
		{
			return (expect.Equals(a));
		}
	
	}
}
