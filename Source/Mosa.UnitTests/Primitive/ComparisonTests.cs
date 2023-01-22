// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.UnitTests;

namespace Mosa.UnitTests.Primitive
{

	public static class ComparisonTests
	{

		[MosaUnitTest(Series = "U1U1")]
		public static bool CompareEqualU1(byte a, byte b)
		{
			return (a == b);
		}

		[MosaUnitTest(Series = "U1U1")]
		public static bool CompareNotEqualU1(byte a, byte b)
		{
			return (a != b);
		}

		[MosaUnitTest(Series = "U1U1")]
		public static bool CompareGreaterThanU1(byte a, byte b)
		{
			return (a > b);
		}

		[MosaUnitTest(Series = "U1U1")]
		public static bool CompareLessThanU1(byte a, byte b)
		{
			return (a < b);
		}

		[MosaUnitTest(Series = "U1U1")]
		public static bool CompareGreaterThanOrEqualU1(byte a, byte b)
		{
			return (a >= b);
		}

		[MosaUnitTest(Series = "U1U1")]
		public static bool CompareLessThanOrEqualU1(byte a, byte b)
		{
			return (a <= b);
		}

		[MosaUnitTest(Series = "U2U2")]
		public static bool CompareEqualU2(ushort a, ushort b)
		{
			return (a == b);
		}

		[MosaUnitTest(Series = "U2U2")]
		public static bool CompareNotEqualU2(ushort a, ushort b)
		{
			return (a != b);
		}

		[MosaUnitTest(Series = "U2U2")]
		public static bool CompareGreaterThanU2(ushort a, ushort b)
		{
			return (a > b);
		}

		[MosaUnitTest(Series = "U2U2")]
		public static bool CompareLessThanU2(ushort a, ushort b)
		{
			return (a < b);
		}

		[MosaUnitTest(Series = "U2U2")]
		public static bool CompareGreaterThanOrEqualU2(ushort a, ushort b)
		{
			return (a >= b);
		}

		[MosaUnitTest(Series = "U2U2")]
		public static bool CompareLessThanOrEqualU2(ushort a, ushort b)
		{
			return (a <= b);
		}

		[MosaUnitTest(Series = "U4U4")]
		public static bool CompareEqualU4(uint a, uint b)
		{
			return (a == b);
		}

		[MosaUnitTest(Series = "U4U4")]
		public static bool CompareNotEqualU4(uint a, uint b)
		{
			return (a != b);
		}

		[MosaUnitTest(Series = "U4U4")]
		public static bool CompareGreaterThanU4(uint a, uint b)
		{
			return (a > b);
		}

		[MosaUnitTest(Series = "U4U4")]
		public static bool CompareLessThanU4(uint a, uint b)
		{
			return (a < b);
		}

		[MosaUnitTest(Series = "U4U4")]
		public static bool CompareGreaterThanOrEqualU4(uint a, uint b)
		{
			return (a >= b);
		}

		[MosaUnitTest(Series = "U4U4")]
		public static bool CompareLessThanOrEqualU4(uint a, uint b)
		{
			return (a <= b);
		}

		[MosaUnitTest(Series = "U8U8")]
		public static bool CompareEqualU8(ulong a, ulong b)
		{
			return (a == b);
		}

		[MosaUnitTest(Series = "U8U8")]
		public static bool CompareNotEqualU8(ulong a, ulong b)
		{
			return (a != b);
		}

		[MosaUnitTest(Series = "U8U8")]
		public static bool CompareGreaterThanU8(ulong a, ulong b)
		{
			return (a > b);
		}

		[MosaUnitTest(Series = "U8U8")]
		public static bool CompareLessThanU8(ulong a, ulong b)
		{
			return (a < b);
		}

		[MosaUnitTest(Series = "U8U8")]
		public static bool CompareGreaterThanOrEqualU8(ulong a, ulong b)
		{
			return (a >= b);
		}

		[MosaUnitTest(Series = "U8U8")]
		public static bool CompareLessThanOrEqualU8(ulong a, ulong b)
		{
			return (a <= b);
		}

		[MosaUnitTest(Series = "I1I1")]
		public static bool CompareEqualI1(sbyte a, sbyte b)
		{
			return (a == b);
		}

		[MosaUnitTest(Series = "I1I1")]
		public static bool CompareNotEqualI1(sbyte a, sbyte b)
		{
			return (a != b);
		}

		[MosaUnitTest(Series = "I1I1")]
		public static bool CompareGreaterThanI1(sbyte a, sbyte b)
		{
			return (a > b);
		}

		[MosaUnitTest(Series = "I1I1")]
		public static bool CompareLessThanI1(sbyte a, sbyte b)
		{
			return (a < b);
		}

		[MosaUnitTest(Series = "I1I1")]
		public static bool CompareGreaterThanOrEqualI1(sbyte a, sbyte b)
		{
			return (a >= b);
		}

		[MosaUnitTest(Series = "I1I1")]
		public static bool CompareLessThanOrEqualI1(sbyte a, sbyte b)
		{
			return (a <= b);
		}

		[MosaUnitTest(Series = "I2I2")]
		public static bool CompareEqualI2(short a, short b)
		{
			return (a == b);
		}

		[MosaUnitTest(Series = "I2I2")]
		public static bool CompareNotEqualI2(short a, short b)
		{
			return (a != b);
		}

		[MosaUnitTest(Series = "I2I2")]
		public static bool CompareGreaterThanI2(short a, short b)
		{
			return (a > b);
		}

		[MosaUnitTest(Series = "I2I2")]
		public static bool CompareLessThanI2(short a, short b)
		{
			return (a < b);
		}

		[MosaUnitTest(Series = "I2I2")]
		public static bool CompareGreaterThanOrEqualI2(short a, short b)
		{
			return (a >= b);
		}

		[MosaUnitTest(Series = "I2I2")]
		public static bool CompareLessThanOrEqualI2(short a, short b)
		{
			return (a <= b);
		}

		[MosaUnitTest(Series = "I4I4")]
		public static bool CompareEqualI4(int a, int b)
		{
			return (a == b);
		}

		[MosaUnitTest(Series = "I4I4")]
		public static bool CompareNotEqualI4(int a, int b)
		{
			return (a != b);
		}

		[MosaUnitTest(Series = "I4I4")]
		public static bool CompareGreaterThanI4(int a, int b)
		{
			return (a > b);
		}

		[MosaUnitTest(Series = "I4I4")]
		public static bool CompareLessThanI4(int a, int b)
		{
			return (a < b);
		}

		[MosaUnitTest(Series = "I4I4")]
		public static bool CompareGreaterThanOrEqualI4(int a, int b)
		{
			return (a >= b);
		}

		[MosaUnitTest(Series = "I4I4")]
		public static bool CompareLessThanOrEqualI4(int a, int b)
		{
			return (a <= b);
		}

		[MosaUnitTest(Series = "I8I8")]
		public static bool CompareEqualI8(long a, long b)
		{
			return (a == b);
		}

		[MosaUnitTest(Series = "I8I8")]
		public static bool CompareNotEqualI8(long a, long b)
		{
			return (a != b);
		}

		[MosaUnitTest(Series = "I8I8")]
		public static bool CompareGreaterThanI8(long a, long b)
		{
			return (a > b);
		}

		[MosaUnitTest(Series = "I8I8")]
		public static bool CompareLessThanI8(long a, long b)
		{
			return (a < b);
		}

		[MosaUnitTest(Series = "I8I8")]
		public static bool CompareGreaterThanOrEqualI8(long a, long b)
		{
			return (a >= b);
		}

		[MosaUnitTest(Series = "I8I8")]
		public static bool CompareLessThanOrEqualI8(long a, long b)
		{
			return (a <= b);
		}

		[MosaUnitTest(Series = "CC")]
		public static bool CompareEqualC(char a, char b)
		{
			return (a == b);
		}

		[MosaUnitTest(Series = "CC")]
		public static bool CompareNotEqualC(char a, char b)
		{
			return (a != b);
		}

		[MosaUnitTest(Series = "CC")]
		public static bool CompareGreaterThanC(char a, char b)
		{
			return (a > b);
		}

		[MosaUnitTest(Series = "CC")]
		public static bool CompareLessThanC(char a, char b)
		{
			return (a < b);
		}

		[MosaUnitTest(Series = "CC")]
		public static bool CompareGreaterThanOrEqualC(char a, char b)
		{
			return (a >= b);
		}

		[MosaUnitTest(Series = "CC")]
		public static bool CompareLessThanOrEqualC(char a, char b)
		{
			return (a <= b);
		}
	}
}
