// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.UnitTests.Basic.Basic
{

	public static class LdargaTests 
	{
	
		[MosaUnitTest(Series = "U1")]
		public static byte LdargaCheckValueU1(byte a)
		{
			return LdargaCheckValueRefU1(ref a);
		}

		private static byte LdargaCheckValueRefU1(ref byte a)
		{
			return a;
		}

		[MosaUnitTest(Series = "U1U1")]
		public static byte LdargaChangeValueU1(byte a, byte b)
		{
			LdargaChangeValueRefU1(ref a,b);
			return a;
		}

		private static void LdargaChangeValueRefU1(ref byte a, byte b)
		{
			a = b;
		}
	
		[MosaUnitTest(Series = "U2")]
		public static ushort LdargaCheckValueU2(ushort a)
		{
			return LdargaCheckValueRefU2(ref a);
		}

		private static ushort LdargaCheckValueRefU2(ref ushort a)
		{
			return a;
		}

		[MosaUnitTest(Series = "U2U2")]
		public static ushort LdargaChangeValueU2(ushort a, ushort b)
		{
			LdargaChangeValueRefU2(ref a,b);
			return a;
		}

		private static void LdargaChangeValueRefU2(ref ushort a, ushort b)
		{
			a = b;
		}
	
		[MosaUnitTest(Series = "U4")]
		public static uint LdargaCheckValueU4(uint a)
		{
			return LdargaCheckValueRefU4(ref a);
		}

		private static uint LdargaCheckValueRefU4(ref uint a)
		{
			return a;
		}

		[MosaUnitTest(Series = "U4U4")]
		public static uint LdargaChangeValueU4(uint a, uint b)
		{
			LdargaChangeValueRefU4(ref a,b);
			return a;
		}

		private static void LdargaChangeValueRefU4(ref uint a, uint b)
		{
			a = b;
		}
	
		[MosaUnitTest(Series = "U8")]
		public static ulong LdargaCheckValueU8(ulong a)
		{
			return LdargaCheckValueRefU8(ref a);
		}

		private static ulong LdargaCheckValueRefU8(ref ulong a)
		{
			return a;
		}

		[MosaUnitTest(Series = "U8U8")]
		public static ulong LdargaChangeValueU8(ulong a, ulong b)
		{
			LdargaChangeValueRefU8(ref a,b);
			return a;
		}

		private static void LdargaChangeValueRefU8(ref ulong a, ulong b)
		{
			a = b;
		}
	
		[MosaUnitTest(Series = "I1")]
		public static sbyte LdargaCheckValueI1(sbyte a)
		{
			return LdargaCheckValueRefI1(ref a);
		}

		private static sbyte LdargaCheckValueRefI1(ref sbyte a)
		{
			return a;
		}

		[MosaUnitTest(Series = "I1I1")]
		public static sbyte LdargaChangeValueI1(sbyte a, sbyte b)
		{
			LdargaChangeValueRefI1(ref a,b);
			return a;
		}

		private static void LdargaChangeValueRefI1(ref sbyte a, sbyte b)
		{
			a = b;
		}
	
		[MosaUnitTest(Series = "I2")]
		public static short LdargaCheckValueI2(short a)
		{
			return LdargaCheckValueRefI2(ref a);
		}

		private static short LdargaCheckValueRefI2(ref short a)
		{
			return a;
		}

		[MosaUnitTest(Series = "I2I2")]
		public static short LdargaChangeValueI2(short a, short b)
		{
			LdargaChangeValueRefI2(ref a,b);
			return a;
		}

		private static void LdargaChangeValueRefI2(ref short a, short b)
		{
			a = b;
		}
	
		[MosaUnitTest(Series = "I4")]
		public static int LdargaCheckValueI4(int a)
		{
			return LdargaCheckValueRefI4(ref a);
		}

		private static int LdargaCheckValueRefI4(ref int a)
		{
			return a;
		}

		[MosaUnitTest(Series = "I4I4")]
		public static int LdargaChangeValueI4(int a, int b)
		{
			LdargaChangeValueRefI4(ref a,b);
			return a;
		}

		private static void LdargaChangeValueRefI4(ref int a, int b)
		{
			a = b;
		}
	
		[MosaUnitTest(Series = "I8")]
		public static long LdargaCheckValueI8(long a)
		{
			return LdargaCheckValueRefI8(ref a);
		}

		private static long LdargaCheckValueRefI8(ref long a)
		{
			return a;
		}

		[MosaUnitTest(Series = "I8I8")]
		public static long LdargaChangeValueI8(long a, long b)
		{
			LdargaChangeValueRefI8(ref a,b);
			return a;
		}

		private static void LdargaChangeValueRefI8(ref long a, long b)
		{
			a = b;
		}
	
		[MosaUnitTest(Series = "R4")]
		public static float LdargaCheckValueR4(float a)
		{
			return LdargaCheckValueRefR4(ref a);
		}

		private static float LdargaCheckValueRefR4(ref float a)
		{
			return a;
		}

		[MosaUnitTest(Series = "R4R4")]
		public static float LdargaChangeValueR4(float a, float b)
		{
			LdargaChangeValueRefR4(ref a,b);
			return a;
		}

		private static void LdargaChangeValueRefR4(ref float a, float b)
		{
			a = b;
		}
	
		[MosaUnitTest(Series = "R8")]
		public static double LdargaCheckValueR8(double a)
		{
			return LdargaCheckValueRefR8(ref a);
		}

		private static double LdargaCheckValueRefR8(ref double a)
		{
			return a;
		}

		[MosaUnitTest(Series = "R8R8")]
		public static double LdargaChangeValueR8(double a, double b)
		{
			LdargaChangeValueRefR8(ref a,b);
			return a;
		}

		private static void LdargaChangeValueRefR8(ref double a, double b)
		{
			a = b;
		}
	
		[MosaUnitTest(Series = "C")]
		public static char LdargaCheckValueC(char a)
		{
			return LdargaCheckValueRefC(ref a);
		}

		private static char LdargaCheckValueRefC(ref char a)
		{
			return a;
		}

		[MosaUnitTest(Series = "CC")]
		public static char LdargaChangeValueC(char a, char b)
		{
			LdargaChangeValueRefC(ref a,b);
			return a;
		}

		private static void LdargaChangeValueRefC(ref char a, char b)
		{
			a = b;
		}
	
		[MosaUnitTest(Series = "B")]
		public static bool LdargaCheckValueB(bool a)
		{
			return LdargaCheckValueRefB(ref a);
		}

		private static bool LdargaCheckValueRefB(ref bool a)
		{
			return a;
		}

		[MosaUnitTest(Series = "BB")]
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
