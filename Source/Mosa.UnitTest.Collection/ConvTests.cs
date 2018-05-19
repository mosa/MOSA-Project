// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.UnitTest.Collection
{

	public static class ConvTests 
	{
	
		//[MosaUnitTest(Series = "U1I1")]
		public static bool ConvU1I1(byte expect, sbyte a)
		{
			return expect == (byte)a;
		}

		//[MosaUnitTest(Series = "U1I2")]
		public static bool ConvU1I2(byte expect, short a)
		{
			return expect == (byte)a;
		}

		//[MosaUnitTest(Series = "U1I4")]
		public static bool ConvU1I4(byte expect, int a)
		{
			return expect == (byte)a;
		}

		//[MosaUnitTest(Series = "U1I8")]
		public static bool ConvU1I8(byte expect, long a)
		{
			return expect == (byte)a;
		}

		//[MosaUnitTest(Series = "U2I1")]
		public static bool ConvU2I1(ushort expect, sbyte a)
		{
			return expect == (ushort)a;
		}

		//[MosaUnitTest(Series = "U2I2")]
		public static bool ConvU2I2(ushort expect, short a)
		{
			return expect == (ushort)a;
		}

		//[MosaUnitTest(Series = "U2I4")]
		public static bool ConvU2I4(ushort expect, int a)
		{
			return expect == (ushort)a;
		}

		//[MosaUnitTest(Series = "U2I8")]
		public static bool ConvU2I8(ushort expect, long a)
		{
			return expect == (ushort)a;
		}

		//[MosaUnitTest(Series = "U4I1")]
		public static bool ConvU4I1(uint expect, sbyte a)
		{
			return expect == (uint)a;
		}

		//[MosaUnitTest(Series = "U4I2")]
		public static bool ConvU4I2(uint expect, short a)
		{
			return expect == (uint)a;
		}

		//[MosaUnitTest(Series = "U4I4")]
		public static bool ConvU4I4(uint expect, int a)
		{
			return expect == (uint)a;
		}

		//[MosaUnitTest(Series = "U4I8")]
		public static bool ConvU4I8(uint expect, long a)
		{
			return expect == (uint)a;
		}

		//[MosaUnitTest(Series = "U8I1")]
		public static bool ConvU8I1(ulong expect, sbyte a)
		{
			return expect == (ulong)a;
		}

		//[MosaUnitTest(Series = "U8I2")]
		public static bool ConvU8I2(ulong expect, short a)
		{
			return expect == (ulong)a;
		}

		//[MosaUnitTest(Series = "U8I4")]
		public static bool ConvU8I4(ulong expect, int a)
		{
			return expect == (ulong)a;
		}

		//[MosaUnitTest(Series = "U8I8")]
		public static bool ConvU8I8(ulong expect, long a)
		{
			return expect == (ulong)a;
		}

		//[MosaUnitTest(Series = "I1I1")]
		public static bool ConvI1I1(sbyte expect, sbyte a)
		{
			return expect == (sbyte)a;
		}

		//[MosaUnitTest(Series = "I1I2")]
		public static bool ConvI1I2(sbyte expect, short a)
		{
			return expect == (sbyte)a;
		}

		//[MosaUnitTest(Series = "I1I4")]
		public static bool ConvI1I4(sbyte expect, int a)
		{
			return expect == (sbyte)a;
		}

		//[MosaUnitTest(Series = "I1I8")]
		public static bool ConvI1I8(sbyte expect, long a)
		{
			return expect == (sbyte)a;
		}

		//[MosaUnitTest(Series = "I2I1")]
		public static bool ConvI2I1(short expect, sbyte a)
		{
			return expect == (short)a;
		}

		//[MosaUnitTest(Series = "I2I2")]
		public static bool ConvI2I2(short expect, short a)
		{
			return expect == (short)a;
		}

		//[MosaUnitTest(Series = "I2I4")]
		public static bool ConvI2I4(short expect, int a)
		{
			return expect == (short)a;
		}

		//[MosaUnitTest(Series = "I2I8")]
		public static bool ConvI2I8(short expect, long a)
		{
			return expect == (short)a;
		}

		//[MosaUnitTest(Series = "I4I1")]
		public static bool ConvI4I1(int expect, sbyte a)
		{
			return expect == (int)a;
		}

		//[MosaUnitTest(Series = "I4I2")]
		public static bool ConvI4I2(int expect, short a)
		{
			return expect == (int)a;
		}

		//[MosaUnitTest(Series = "I4I4")]
		public static bool ConvI4I4(int expect, int a)
		{
			return expect == (int)a;
		}

		//[MosaUnitTest(Series = "I4I8")]
		public static bool ConvI4I8(int expect, long a)
		{
			return expect == (int)a;
		}

		//[MosaUnitTest(Series = "I8I1")]
		public static bool ConvI8I1(long expect, sbyte a)
		{
			return expect == (long)a;
		}

		//[MosaUnitTest(Series = "I8I2")]
		public static bool ConvI8I2(long expect, short a)
		{
			return expect == (long)a;
		}

		//[MosaUnitTest(Series = "I8I4")]
		public static bool ConvI8I4(long expect, int a)
		{
			return expect == (long)a;
		}

		//[MosaUnitTest(Series = "I8I8")]
		public static bool ConvI8I8(long expect, long a)
		{
			return expect == (long)a;
		}

		//[MosaUnitTest(Series = "R4I1")]
		public static bool ConvR4I1(float expect, sbyte a)
		{
			return expect == (float)a;
		}

		//[MosaUnitTest(Series = "R4I2")]
		public static bool ConvR4I2(float expect, short a)
		{
			return expect == (float)a;
		}

		//[MosaUnitTest(Series = "R4I4")]
		public static bool ConvR4I4(float expect, int a)
		{
			return expect == (float)a;
		}

		//[MosaUnitTest(Series = "R4I8")]
		public static bool ConvR4I8(float expect, long a)
		{
			return expect == (float)a;
		}

		//[MosaUnitTest(Series = "R8I1")]
		public static bool ConvR8I1(double expect, sbyte a)
		{
			return expect == (double)a;
		}

		//[MosaUnitTest(Series = "R8I2")]
		public static bool ConvR8I2(double expect, short a)
		{
			return expect == (double)a;
		}

		//[MosaUnitTest(Series = "R8I4")]
		public static bool ConvR8I4(double expect, int a)
		{
			return expect == (double)a;
		}

		//[MosaUnitTest(Series = "R8I8")]
		public static bool ConvR8I8(double expect, long a)
		{
			return expect == (double)a;
		}
	}
}
