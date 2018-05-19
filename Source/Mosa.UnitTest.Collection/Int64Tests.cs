// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTest.Collection
{
	public static class Int64Tests
	{
		[MosaUnitTest(Series = "I8I8")]
		public static long AddI8I8(long first, long second)
		{
			return first + second;
		}

		[MosaUnitTest(Series = "I8I8")]
		public static long SubI8I8(long first, long second)
		{
			return first - second;
		}

		[MosaUnitTest(Series = "I8I8")]
		public static long MulI8I8(long first, long second)
		{
			return first * second;
		}

		[MosaUnitTest(Series = "I8I8")]
		public static long DivI8I8(long first, long second)
		{
			return first / second;
		}

		[MosaUnitTest(Series = "I8I8")]
		public static long RemI8I8(long first, long second)
		{
			return first % second;
		}

		[MosaUnitTest(Series = "I8")]
		public static long RetI8(long first)
		{
			return first;
		}

		[MosaUnitTest(Series = "I8I8")]
		public static long AndI8I8(long first, long second)
		{
			return first & second;
		}

		[MosaUnitTest(Series = "I8I8")]
		public static long OrI8I8(long first, long second)
		{
			return first | second;
		}

		[MosaUnitTest(Series = "I8I8")]
		public static long XorI8I8(long first, long second)
		{
			return first ^ second;
		}

		[MosaUnitTest(Series = "I8")]
		public static long CompI8(long first)
		{
			return ~first;
		}

		[MosaUnitTest(Series = "I8U1UpTo32")]
		public static long ShiftLeftI8U1(long first, byte second)
		{
			return first << second;
		}

		[MosaUnitTest(Series = "I8U1UpTo32")]
		public static long ShiftRightI8U1(long first, byte second)
		{
			return first >> second;
		}

		[MosaUnitTest(Series = "I8I8")]
		public static bool CeqI8I8(long first, long second)
		{
			return first == second;
		}

		[MosaUnitTest(Series = "I8I8")]
		public static bool CltI8I8(long first, long second)
		{
			return first < second;
		}

		[MosaUnitTest(Series = "I8I8")]
		public static bool CgtI8I8(long first, long second)
		{
			return first > second;
		}

		[MosaUnitTest(Series = "I8I8")]
		public static bool CleI8I8(long first, long second)
		{
			return first <= second;
		}

		[MosaUnitTest(Series = "I8I8")]
		public static bool CgeI8I8(long first, long second)
		{
			return first >= second;
		}

		[MosaUnitTest]
		public static bool Newarr()
		{
			long[] arr = new long[0];
			return arr != null;
		}

		[MosaUnitTest(Series = "I4Small")]
		public static bool Ldlen(int length)
		{
			long[] arr = new long[length];
			return arr.Length == length;
		}

		[MosaUnitTest(Series = "I4SmallI8")]
		public static long Ldelem(int index, long value)
		{
			long[] arr = new long[index + 1];
			arr[index] = value;
			return arr[index];
		}

		[MosaUnitTest(Series = "I4SmallI8")]
		public static bool Stelem(int index, long value)
		{
			long[] arr = new long[index + 1];
			arr[index] = value;
			return true;
		}

		[MosaUnitTest(Series = "I4SmallI8")]
		public static long Ldelema(int index, long value)
		{
			long[] arr = new long[index + 1];
			SetValueInRefValue(ref arr[index], value);
			return arr[index];
		}

		private static void SetValueInRefValue(ref long destination, long value)
		{
			destination = value;
		}
	}
}
