// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests
{
	public static class ConvTests
	{

		[MosaUnitTest("U1", "U1")]
		public static bool ConvU1U1(byte expect, byte a)
		{
			return expect == (byte)a;
		}

		[MosaUnitTest("U1", "U2")]
		public static bool ConvU1U2(byte expect, ushort a)
		{
			return expect == (byte)a;
		}

		[MosaUnitTest("U1", "U4")]
		public static bool ConvU1U4(byte expect, uint a)
		{
			return expect == (byte)a;
		}

		[MosaUnitTest("U1", "U8")]
		public static bool ConvU1U8(byte expect, ulong a)
		{
			return expect == (byte)a;
		}

		[MosaUnitTest("U1", "I1")]
		public static bool ConvU1I1(byte expect, sbyte a)
		{
			return expect == (byte)a;
		}

		[MosaUnitTest("U1", "I2")]
		public static bool ConvU1I2(byte expect, short a)
		{
			return expect == (byte)a;
		}

		[MosaUnitTest("U1", "I4")]
		public static bool ConvU1I4(byte expect, int a)
		{
			return expect == (byte)a;
		}

		[MosaUnitTest("U1", "I8")]
		public static bool ConvU1I8(byte expect, long a)
		{
			return expect == (byte)a;
		}

		[MosaUnitTest("U2", "U1")]
		public static bool ConvU2U1(ushort expect, byte a)
		{
			return expect == (ushort)a;
		}

		[MosaUnitTest("U2", "U2")]
		public static bool ConvU2U2(ushort expect, ushort a)
		{
			return expect == (ushort)a;
		}

		[MosaUnitTest("U2", "U4")]
		public static bool ConvU2U4(ushort expect, uint a)
		{
			return expect == (ushort)a;
		}

		[MosaUnitTest("U2", "U8")]
		public static bool ConvU2U8(ushort expect, ulong a)
		{
			return expect == (ushort)a;
		}

		[MosaUnitTest("U2", "I1")]
		public static bool ConvU2I1(ushort expect, sbyte a)
		{
			return expect == (ushort)a;
		}

		[MosaUnitTest("U2", "I2")]
		public static bool ConvU2I2(ushort expect, short a)
		{
			return expect == (ushort)a;
		}

		[MosaUnitTest("U2", "I4")]
		public static bool ConvU2I4(ushort expect, int a)
		{
			return expect == (ushort)a;
		}

		[MosaUnitTest("U2", "I8")]
		public static bool ConvU2I8(ushort expect, long a)
		{
			return expect == (ushort)a;
		}

		[MosaUnitTest("U4", "U1")]
		public static bool ConvU4U1(uint expect, byte a)
		{
			return expect == (uint)a;
		}

		[MosaUnitTest("U4", "U2")]
		public static bool ConvU4U2(uint expect, ushort a)
		{
			return expect == (uint)a;
		}

		[MosaUnitTest("U4", "U4")]
		public static bool ConvU4U4(uint expect, uint a)
		{
			return expect == (uint)a;
		}

		[MosaUnitTest("U4", "U8")]
		public static bool ConvU4U8(uint expect, ulong a)
		{
			return expect == (uint)a;
		}

		[MosaUnitTest("U4", "I1")]
		public static bool ConvU4I1(uint expect, sbyte a)
		{
			return expect == (uint)a;
		}

		[MosaUnitTest("U4", "I2")]
		public static bool ConvU4I2(uint expect, short a)
		{
			return expect == (uint)a;
		}

		[MosaUnitTest("U4", "I4")]
		public static bool ConvU4I4(uint expect, int a)
		{
			return expect == (uint)a;
		}

		[MosaUnitTest("U4", "I8")]
		public static bool ConvU4I8(uint expect, long a)
		{
			return expect == (uint)a;
		}

		[MosaUnitTest("U8", "U1")]
		public static bool ConvU8U1(ulong expect, byte a)
		{
			return expect == (ulong)a;
		}

		[MosaUnitTest("U8", "U2")]
		public static bool ConvU8U2(ulong expect, ushort a)
		{
			return expect == (ulong)a;
		}

		[MosaUnitTest("U8", "U4")]
		public static bool ConvU8U4(ulong expect, uint a)
		{
			return expect == (ulong)a;
		}

		[MosaUnitTest("U8", "U8")]
		public static bool ConvU8U8(ulong expect, ulong a)
		{
			return expect == (ulong)a;
		}

		[MosaUnitTest("U8", "I1")]
		public static bool ConvU8I1(ulong expect, sbyte a)
		{
			return expect == (ulong)a;
		}

		[MosaUnitTest("U8", "I2")]
		public static bool ConvU8I2(ulong expect, short a)
		{
			return expect == (ulong)a;
		}

		[MosaUnitTest("U8", "I4")]
		public static bool ConvU8I4(ulong expect, int a)
		{
			return expect == (ulong)a;
		}

		[MosaUnitTest("U8", "I8")]
		public static bool ConvU8I8(ulong expect, long a)
		{
			return expect == (ulong)a;
		}

		[MosaUnitTest("I1", "U1")]
		public static bool ConvI1U1(sbyte expect, byte a)
		{
			return expect == (sbyte)a;
		}

		[MosaUnitTest("I1", "U2")]
		public static bool ConvI1U2(sbyte expect, ushort a)
		{
			return expect == (sbyte)a;
		}

		[MosaUnitTest("I1", "U4")]
		public static bool ConvI1U4(sbyte expect, uint a)
		{
			return expect == (sbyte)a;
		}

		[MosaUnitTest("I1", "U8")]
		public static bool ConvI1U8(sbyte expect, ulong a)
		{
			return expect == (sbyte)a;
		}

		[MosaUnitTest("I1", "I1")]
		public static bool ConvI1I1(sbyte expect, sbyte a)
		{
			return expect == (sbyte)a;
		}

		[MosaUnitTest("I1", "I2")]
		public static bool ConvI1I2(sbyte expect, short a)
		{
			return expect == (sbyte)a;
		}

		[MosaUnitTest("I1", "I4")]
		public static bool ConvI1I4(sbyte expect, int a)
		{
			return expect == (sbyte)a;
		}

		[MosaUnitTest("I1", "I8")]
		public static bool ConvI1I8(sbyte expect, long a)
		{
			return expect == (sbyte)a;
		}

		[MosaUnitTest("I2", "U1")]
		public static bool ConvI2U1(short expect, byte a)
		{
			return expect == (short)a;
		}

		[MosaUnitTest("I2", "U2")]
		public static bool ConvI2U2(short expect, ushort a)
		{
			return expect == (short)a;
		}

		[MosaUnitTest("I2", "U4")]
		public static bool ConvI2U4(short expect, uint a)
		{
			return expect == (short)a;
		}

		[MosaUnitTest("I2", "U8")]
		public static bool ConvI2U8(short expect, ulong a)
		{
			return expect == (short)a;
		}

		[MosaUnitTest("I2", "I1")]
		public static bool ConvI2I1(short expect, sbyte a)
		{
			return expect == (short)a;
		}

		[MosaUnitTest("I2", "I2")]
		public static bool ConvI2I2(short expect, short a)
		{
			return expect == (short)a;
		}

		[MosaUnitTest("I2", "I4")]
		public static bool ConvI2I4(short expect, int a)
		{
			return expect == (short)a;
		}

		[MosaUnitTest("I2", "I8")]
		public static bool ConvI2I8(short expect, long a)
		{
			return expect == (short)a;
		}

		[MosaUnitTest("I4", "U1")]
		public static bool ConvI4U1(int expect, byte a)
		{
			return expect == (int)a;
		}

		[MosaUnitTest("I4", "U2")]
		public static bool ConvI4U2(int expect, ushort a)
		{
			return expect == (int)a;
		}

		[MosaUnitTest("I4", "U4")]
		public static bool ConvI4U4(int expect, uint a)
		{
			return expect == (int)a;
		}

		[MosaUnitTest("I4", "U8")]
		public static bool ConvI4U8(int expect, ulong a)
		{
			return expect == (int)a;
		}

		[MosaUnitTest("I4", "I1")]
		public static bool ConvI4I1(int expect, sbyte a)
		{
			return expect == (int)a;
		}

		[MosaUnitTest("I4", "I2")]
		public static bool ConvI4I2(int expect, short a)
		{
			return expect == (int)a;
		}

		[MosaUnitTest("I4", "I4")]
		public static bool ConvI4I4(int expect, int a)
		{
			return expect == (int)a;
		}

		[MosaUnitTest("I4", "I8")]
		public static bool ConvI4I8(int expect, long a)
		{
			return expect == (int)a;
		}

		[MosaUnitTest("I8", "U1")]
		public static bool ConvI8U1(long expect, byte a)
		{
			return expect == (long)a;
		}

		[MosaUnitTest("I8", "U2")]
		public static bool ConvI8U2(long expect, ushort a)
		{
			return expect == (long)a;
		}

		[MosaUnitTest("I8", "U4")]
		public static bool ConvI8U4(long expect, uint a)
		{
			return expect == (long)a;
		}

		[MosaUnitTest("I8", "U8")]
		public static bool ConvI8U8(long expect, ulong a)
		{
			return expect == (long)a;
		}

		[MosaUnitTest("I8", "I1")]
		public static bool ConvI8I1(long expect, sbyte a)
		{
			return expect == (long)a;
		}

		[MosaUnitTest("I8", "I2")]
		public static bool ConvI8I2(long expect, short a)
		{
			return expect == (long)a;
		}

		[MosaUnitTest("I8", "I4")]
		public static bool ConvI8I4(long expect, int a)
		{
			return expect == (long)a;
		}

		[MosaUnitTest("I8", "I8")]
		public static bool ConvI8I8(long expect, long a)
		{
			return expect == (long)a;
		}
	}
}
