// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Primitive
{
	public static class UInt64Tests
	{
		[MosaUnitTest(Series = "U8U8")]
		public static ulong AddU8U8(ulong first, ulong second)
		{
			return first + second;
		}

		[MosaUnitTest(Series = "U8U8")]
		public static ulong SubU8U8(ulong first, ulong second)
		{
			return first - second;
		}

		[MosaUnitTest(Series = "U8U8")]
		public static ulong MulU8U8(ulong first, ulong second)
		{
			return first * second;
		}

		[MosaUnitTest("U8", "U8NotZero")]
		public static ulong DivU8U8(ulong first, ulong second)
		{
			return first / second;
		}

		[MosaUnitTest("U8", "U8NotZero")]
		public static ulong RemU8U8(ulong first, ulong second)
		{
			return first % second;
		}

		[MosaUnitTest(Series = "U8")]
		public static ulong RetU8(ulong first)
		{
			return first;
		}

		[MosaUnitTest(Series = "U8U8")]
		public static ulong AndU8U8(ulong first, ulong second)
		{
			return first & second;
		}

		[MosaUnitTest(Series = "U8U8")]
		public static ulong OrU8U8(ulong first, ulong second)
		{
			return first | second;
		}

		[MosaUnitTest(Series = "U8U8")]
		public static ulong XorU8U8(ulong first, ulong second)
		{
			return first ^ second;
		}

		[MosaUnitTest(Series = "U8")]
		public static ulong CompU8(ulong first)
		{
			return ~first;
		}

		[MosaUnitTest(Series = "U8U1UpTo64")]
		public static ulong ShiftLeftU8U1(ulong first, byte second)
		{
			return first << second;
		}

		[MosaUnitTest(Series = "U8U1UpTo64")]
		public static ulong ShiftRightU8U1(ulong first, byte second)
		{
			return first >> second;
		}

		[MosaUnitTest(Series = "U8U8")]
		public static bool CeqU8U8(ulong first, ulong second)
		{
			return first == second;
		}

		[MosaUnitTest(Series = "U8U8")]
		public static bool CltU8U8(ulong first, ulong second)
		{
			return first < second;
		}

		[MosaUnitTest(Series = "U8U8")]
		public static bool CgtU8U8(ulong first, ulong second)
		{
			return first > second;
		}

		[MosaUnitTest(Series = "U8U8")]
		public static bool CleU8U8(ulong first, ulong second)
		{
			return first <= second;
		}

		[MosaUnitTest(Series = "U8U8")]
		public static bool CgeU8U8(ulong first, ulong second)
		{
			return first >= second;
		}

		[MosaUnitTest]
		public static bool Newarr()
		{
			ulong[] arr = new ulong[0];
			return arr != null;
		}

		[MosaUnitTest(Series = "I4Small")]
		public static bool Ldlen(int length)
		{
			ulong[] arr = new ulong[length];
			return arr.Length == length;
		}

		[MosaUnitTest(Series = "I4SmallU8")]
		public static ulong Ldelem(int index, ulong value)
		{
			ulong[] arr = new ulong[index + 1];
			arr[index] = value;
			return arr[index];
		}

		[MosaUnitTest(Series = "I4SmallU8")]
		public static bool Stelem(int index, ulong value)
		{
			ulong[] arr = new ulong[index + 1];
			arr[index] = value;
			return true;
		}

		[MosaUnitTest(Series = "I4SmallU8")]
		public static ulong Ldelema(int index, ulong value)
		{
			ulong[] arr = new ulong[index + 1];
			SetValueInRefValue(ref arr[index], value);
			return arr[index];
		}

		private static void SetValueInRefValue(ref ulong destination, ulong value)
		{
			destination = value;
		}
	}
}
