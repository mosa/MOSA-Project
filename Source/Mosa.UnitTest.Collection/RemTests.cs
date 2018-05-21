// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTest.Collection
{
	public static class RemTests
	{
		[MosaUnitTest(Series = "I4")]
		public static int RemI4_C1(int first)
		{
			return first % 1;
		}

		[MosaUnitTest(Series = "I4")]
		public static int RemI4_C2(int first)
		{
			return first % 2;
		}

		[MosaUnitTest(Series = "I4")]
		public static int RemI4_C4(int first)
		{
			return first % 4;
		}

		[MosaUnitTest(Series = "I4")]
		public static int RemI4_C8(int first)
		{
			return first % 8;
		}

		[MosaUnitTest(Series = "I4")]
		public static int RemI4_C16(int first)
		{
			return first % 16;
		}

		[MosaUnitTest(Series = "I4")]
		public static int RemI4_C32(int first)
		{
			return first % 32;
		}

		[MosaUnitTest(Series = "I4")]
		public static int RemI4_C64(int first)
		{
			return first % 64;
		}

		[MosaUnitTest(Series = "I4")]
		public static int RemI4_C128(int first)
		{
			return first % 128;
		}

		[MosaUnitTest(Series = "I4")]
		public static int RemI4_C256(int first)
		{
			return first % 256;
		}

		[MosaUnitTest(Series = "U4")]
		public static uint RemU4_C1(uint first)
		{
			return first % 1;
		}

		[MosaUnitTest(Series = "U4")]
		public static uint RemU4_C2(uint first)
		{
			return first % 2;
		}

		[MosaUnitTest(Series = "U4")]
		public static uint RemU4_C4(uint first)
		{
			return first % 4;
		}

		[MosaUnitTest(Series = "U4")]
		public static uint RemU4_C8(uint first)
		{
			return first % 8;
		}

		[MosaUnitTest(Series = "U4")]
		public static uint RemU4_C16(uint first)
		{
			return first % 16;
		}

		[MosaUnitTest(Series = "U4")]
		public static uint RemU4_C32(uint first)
		{
			return first % 32;
		}

		[MosaUnitTest(Series = "U4")]
		public static uint RemU4_C64(uint first)
		{
			return first % 64;
		}

		[MosaUnitTest(Series = "U4")]
		public static uint RemU4_C128(uint first)
		{
			return first % 128;
		}

		[MosaUnitTest(Series = "U4")]
		public static uint RemU4_C256(uint first)
		{
			return first % 256;
		}
	}
}
