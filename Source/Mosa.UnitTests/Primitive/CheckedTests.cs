// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Primitive
{
	public static class CheckedTests
	{
		[MosaUnitTest(Series = "I4I4")]
		public static int AddI4I4(int first, int second)
		{
			try
			{
				return checked(first + second);
			}
			catch
			{
				return 0x05ADBEEF;
			}
		}

		[MosaUnitTest(Series = "I4I4")]
		public static int SubI4I4(int first, int second)
		{
			try
			{
				return checked(first - second);
			}
			catch
			{
				return 0x05ADBEEF;
			}
		}

		[MosaUnitTest(Series = "I4I4")]
		public static int MulI4I4(int first, int second)
		{
			try
			{
				return checked(first * second);
			}
			catch
			{
				return 0x05ADBEEF;
			}
		}

		[MosaUnitTest(Series = "I8I8")]
		public static long AddI8I8(long first, long second)
		{
			try
			{
				return checked(first + second);
			}
			catch
			{
				return 0x05ADBEEF;
			}
		}

		[MosaUnitTest(Series = "I8I8")]
		public static long SubI8I8(long first, long second)
		{
			try
			{
				return checked(first - second);
			}
			catch
			{
				return 0x05ADBEEF;
			}
		}

		[MosaUnitTest(Series = "I8I8")]
		public static long MulI8I8(long first, long second)
		{
			try
			{
				return checked(first * second);
			}
			catch
			{
				return 0x05ADBEEF;
			}
		}

		[MosaUnitTest(Series = "U4U4")]
		public static uint AddU4U4(uint first, uint second)
		{
			try
			{
				return checked(first + second);
			}
			catch
			{
				return 0x05ADBEEF;
			}
		}

		[MosaUnitTest(Series = "U4U4")]
		public static uint SubU4U4(uint first, uint second)
		{
			try
			{
				return checked(first - second);
			}
			catch
			{
				return 0x05ADBEEF;
			}
		}

		[MosaUnitTest(Series = "U4U4")]
		public static uint MulU4U4(uint first, uint second)
		{
			try
			{
				return checked(first * second);
			}
			catch
			{
				return 0x05ADBEEF;
			}
		}

		[MosaUnitTest(Series = "U8U8")]
		public static ulong AddU8U8(ulong first, ulong second)
		{
			try
			{
				return checked(first + second);
			}
			catch
			{
				return 0x05ADBEEF;
			}
		}

		[MosaUnitTest(Series = "U8U8")]
		public static ulong SubU8U8(ulong first, ulong second)
		{
			try
			{
				return checked(first - second);
			}
			catch
			{
				return 0x05ADBEEF;
			}
		}

		[MosaUnitTest(Series = "U8U8")]
		public static ulong MulU8U8(ulong first, ulong second)
		{
			try
			{
				return checked(first * second);
			}
			catch
			{
				return 0x05ADBEEF;
			}
		}

		[MosaUnitTest(Series = "U8")]
		public static int ConvI4(ulong first)
		{
			try
			{
				return checked((int)first);
			}
			catch
			{
				return 0x05ADBEEF;
			}
		}
	}
}
