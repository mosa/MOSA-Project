// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTest.Collection
{
	public static class UInt32Tests
	{
		[MosaUnitTest(Series = "U4U4")]
		public static uint AddU4U4(uint first, uint second)
		{
			return first + second;
		}

		[MosaUnitTest(Series = "U4U4")]
		public static uint SubU4U4(uint first, uint second)
		{
			return first - second;
		}

		[MosaUnitTest(Series = "U4U4")]
		public static uint MulU4U4(uint first, uint second)
		{
			return first * second;
		}

		[MosaUnitTest(Series = "U4U4")]
		public static uint DivU4U4(uint first, uint second)
		{
			return first / second;
		}

		[MosaUnitTest(Series = "U4U4")]
		public static uint RemU4U4(uint first, uint second)
		{
			return first % second;
		}

		[MosaUnitTest(Series = "U4")]
		public static uint RetU4(uint first)
		{
			return first;
		}

		[MosaUnitTest(Series = "U4U4")]
		public static uint AndU4U4(uint first, uint second)
		{
			return first & second;
		}

		[MosaUnitTest(Series = "U4U4")]
		public static uint OrU4U4(uint first, uint second)
		{
			return first | second;
		}

		[MosaUnitTest(Series = "U4U4")]
		public static uint XorU4U4(uint first, uint second)
		{
			return first ^ second;
		}

		[MosaUnitTest(Series = "U4")]
		public static uint CompU4(uint first)
		{
			return ~first;
		}

		[MosaUnitTest(Series = "U4U1UpTo32")]
		public static uint ShiftLeftU4U1(uint first, byte second)
		{
			return first << second;
		}

		[MosaUnitTest(Series = "U4U1UpTo32")]
		public static uint ShiftRightU4U1(uint first, byte second)
		{
			return first >> second;
		}

		[MosaUnitTest(Series = "U4U4")]
		public static bool CeqU4U4(uint first, uint second)
		{
			return first == second;
		}

		[MosaUnitTest(Series = "U4U4")]
		public static bool CltU4U4(uint first, uint second)
		{
			return first < second;
		}

		[MosaUnitTest(Series = "U4U4")]
		public static bool CgtU4U4(uint first, uint second)
		{
			return first > second;
		}

		[MosaUnitTest(Series = "U4U4")]
		public static bool CleU4U4(uint first, uint second)
		{
			return first <= second;
		}

		[MosaUnitTest(Series = "U4U4")]
		public static bool CgeU4U4(uint first, uint second)
		{
			return first >= second;
		}

		[MosaUnitTest]
		public static bool Newarr()
		{
			uint[] arr = new uint[0];
			return arr != null;
		}

		[MosaUnitTest(Series = "I4Small")]
		public static bool Ldlen(int length)
		{
			uint[] arr = new uint[length];
			return arr.Length == length;
		}

		[MosaUnitTest(Series = "I4SmallU4")]
		public static uint Ldelem(int index, uint value)
		{
			uint[] arr = new uint[index + 1];
			arr[index] = value;
			return arr[index];
		}

		[MosaUnitTest(Series = "I4SmallU4")]
		public static bool Stelem(int index, uint value)
		{
			uint[] arr = new uint[index + 1];
			arr[index] = value;
			return true;
		}

		[MosaUnitTest(Series = "I4SmallU4")]
		public static uint Ldelema(int index, uint value)
		{
			uint[] arr = new uint[index + 1];
			SetValueInRefValue(ref arr[index], value);
			return arr[index];
		}

		private static void SetValueInRefValue(ref uint destination, uint value)
		{
			destination = value;
		}
	}
}
