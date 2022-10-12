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
		public static sbyte ConvU8ToI1(ulong first)
		{
			try
			{
				return checked((sbyte)first);
			}
			catch
			{
				return 0x05;
			}
		}

		[MosaUnitTest(Series = "U8")]
		public static short ConvU8ToI2(ulong first)
		{
			try
			{
				return checked((short)first);
			}
			catch
			{
				return 0x05AD;
			}
		}

		[MosaUnitTest(Series = "U8")]
		public static int ConvU8ToI4(ulong first)
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

		[MosaUnitTest(Series = "U8")]
		public static long ConvU8ToI8(ulong first)
		{
			try
			{
				return checked((long)first);
			}
			catch
			{
				return 0x05ADBEEF;
			}
		}

		[MosaUnitTest(Series = "I8")]
		public static byte ConvI8ToU1(long first)
		{
			try
			{
				return checked((byte)first);
			}
			catch
			{
				return 0x05;
			}
		}

		[MosaUnitTest(Series = "I8")]
		public static ushort ConvI8ToU2(long first)
		{
			try
			{
				return checked((ushort)first);
			}
			catch
			{
				return 0x05AD;
			}
		}

		[MosaUnitTest(Series = "I8")]
		public static uint ConvI8ToU4(long first)
		{
			try
			{
				return checked((uint)first);
			}
			catch
			{
				return 0x05ADBEEF;
			}
		}

		[MosaUnitTest(Series = "R4")]
		public static int ConvR4ToI4(float first)
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

		[MosaUnitTest(Series = "R4")]
		public static uint ConvR4ToU4(float first)
		{
			try
			{
				return checked((uint)first);
			}
			catch
			{
				return 0x05ADBEEF;
			}
		}

		[MosaUnitTest(Series = "R4")]
		public static long ConvR4ToI8(float first)
		{
			try
			{
				return checked((long)first);
			}
			catch
			{
				return 0x05ADBEEF;
			}
		}

		[MosaUnitTest(Series = "R4")]
		public static ulong ConvR4ToU8(float first)
		{
			try
			{
				return checked((ulong)first);
			}
			catch
			{
				return 0x05ADBEEF;
			}
		}

		[MosaUnitTest(Series = "R8")]
		public static int ConvR8ToI4(double first)
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

		[MosaUnitTest(Series = "R8")]
		public static uint ConvR8ToU4(double first)
		{
			try
			{
				return checked((uint)first);
			}
			catch
			{
				return 0x05ADBEEF;
			}
		}

		[MosaUnitTest(Series = "R8")]
		public static long ConvR8ToI8(double first)
		{
			try
			{
				return checked((long)first);
			}
			catch
			{
				return 0x05ADBEEF;
			}
		}

		[MosaUnitTest(Series = "R8")]
		public static ulong ConvR8ToU8(double first)
		{
			try
			{
				return checked((ulong)first);
			}
			catch
			{
				return 0x05ADBEEF;
			}
		}
	}
}
