 
using System;

namespace Mosa.Test.Collection
{

	public static class LdargaTests 
	{
	
		public static byte LdargaCheckValueU1(byte a) 
		{
			return LdargaCheckValueRefU1(ref a);
		}

		private static byte LdargaCheckValueRefU1(ref byte a)
		{
			return a;
		}
				
		public static byte LdargaChangeValueU1(byte a, byte b)
		{
			LdargaChangeValueRefU1(ref a,b);
			return a;
		}

		private static void LdargaChangeValueRefU1(ref byte a, byte b)
		{
			a = b;
		}
	
		public static ushort LdargaCheckValueU2(ushort a) 
		{
			return LdargaCheckValueRefU2(ref a);
		}

		private static ushort LdargaCheckValueRefU2(ref ushort a)
		{
			return a;
		}
				
		public static ushort LdargaChangeValueU2(ushort a, ushort b)
		{
			LdargaChangeValueRefU2(ref a,b);
			return a;
		}

		private static void LdargaChangeValueRefU2(ref ushort a, ushort b)
		{
			a = b;
		}
	
		public static uint LdargaCheckValueU4(uint a) 
		{
			return LdargaCheckValueRefU4(ref a);
		}

		private static uint LdargaCheckValueRefU4(ref uint a)
		{
			return a;
		}
				
		public static uint LdargaChangeValueU4(uint a, uint b)
		{
			LdargaChangeValueRefU4(ref a,b);
			return a;
		}

		private static void LdargaChangeValueRefU4(ref uint a, uint b)
		{
			a = b;
		}
	
		public static ulong LdargaCheckValueU8(ulong a) 
		{
			return LdargaCheckValueRefU8(ref a);
		}

		private static ulong LdargaCheckValueRefU8(ref ulong a)
		{
			return a;
		}
				
		public static ulong LdargaChangeValueU8(ulong a, ulong b)
		{
			LdargaChangeValueRefU8(ref a,b);
			return a;
		}

		private static void LdargaChangeValueRefU8(ref ulong a, ulong b)
		{
			a = b;
		}
	
		public static sbyte LdargaCheckValueI1(sbyte a) 
		{
			return LdargaCheckValueRefI1(ref a);
		}

		private static sbyte LdargaCheckValueRefI1(ref sbyte a)
		{
			return a;
		}
				
		public static sbyte LdargaChangeValueI1(sbyte a, sbyte b)
		{
			LdargaChangeValueRefI1(ref a,b);
			return a;
		}

		private static void LdargaChangeValueRefI1(ref sbyte a, sbyte b)
		{
			a = b;
		}
	
		public static short LdargaCheckValueI2(short a) 
		{
			return LdargaCheckValueRefI2(ref a);
		}

		private static short LdargaCheckValueRefI2(ref short a)
		{
			return a;
		}
				
		public static short LdargaChangeValueI2(short a, short b)
		{
			LdargaChangeValueRefI2(ref a,b);
			return a;
		}

		private static void LdargaChangeValueRefI2(ref short a, short b)
		{
			a = b;
		}
	
		public static int LdargaCheckValueI4(int a) 
		{
			return LdargaCheckValueRefI4(ref a);
		}

		private static int LdargaCheckValueRefI4(ref int a)
		{
			return a;
		}
				
		public static int LdargaChangeValueI4(int a, int b)
		{
			LdargaChangeValueRefI4(ref a,b);
			return a;
		}

		private static void LdargaChangeValueRefI4(ref int a, int b)
		{
			a = b;
		}
	
		public static long LdargaCheckValueI8(long a) 
		{
			return LdargaCheckValueRefI8(ref a);
		}

		private static long LdargaCheckValueRefI8(ref long a)
		{
			return a;
		}
				
		public static long LdargaChangeValueI8(long a, long b)
		{
			LdargaChangeValueRefI8(ref a,b);
			return a;
		}

		private static void LdargaChangeValueRefI8(ref long a, long b)
		{
			a = b;
		}
	
		public static float LdargaCheckValueR4(float a) 
		{
			return LdargaCheckValueRefR4(ref a);
		}

		private static float LdargaCheckValueRefR4(ref float a)
		{
			return a;
		}
				
		public static float LdargaChangeValueR4(float a, float b)
		{
			LdargaChangeValueRefR4(ref a,b);
			return a;
		}

		private static void LdargaChangeValueRefR4(ref float a, float b)
		{
			a = b;
		}
	
		public static double LdargaCheckValueR8(double a) 
		{
			return LdargaCheckValueRefR8(ref a);
		}

		private static double LdargaCheckValueRefR8(ref double a)
		{
			return a;
		}
				
		public static double LdargaChangeValueR8(double a, double b)
		{
			LdargaChangeValueRefR8(ref a,b);
			return a;
		}

		private static void LdargaChangeValueRefR8(ref double a, double b)
		{
			a = b;
		}
	
		public static char LdargaCheckValueC(char a) 
		{
			return LdargaCheckValueRefC(ref a);
		}

		private static char LdargaCheckValueRefC(ref char a)
		{
			return a;
		}
				
		public static char LdargaChangeValueC(char a, char b)
		{
			LdargaChangeValueRefC(ref a,b);
			return a;
		}

		private static void LdargaChangeValueRefC(ref char a, char b)
		{
			a = b;
		}
	
		public static bool LdargaCheckValueB(bool a) 
		{
			return LdargaCheckValueRefB(ref a);
		}

		private static bool LdargaCheckValueRefB(ref bool a)
		{
			return a;
		}
				
		public static bool LdargaChangeValueB(bool a, bool b)
		{
			LdargaChangeValueRefB(ref a,b);
			return a;
		}

		private static void LdargaChangeValueRefB(ref bool a, bool b)
		{
			a = b;
		}
		}
}
