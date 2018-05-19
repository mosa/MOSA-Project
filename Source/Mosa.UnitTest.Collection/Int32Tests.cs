// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTest.Collection
{
	public static class Int32Tests
	{
		[MosaUnitTest(Series = "I4I4")]
		public static int AddI4I4(int first, int second)
		{
			return first + second;
		}

		[MosaUnitTest(Series = "I4I4")]
		public static int SubI4I4(int first, int second)
		{
			return first - second;
		}

		[MosaUnitTest(Series = "I4I4")]
		public static int MulI4I4(int first, int second)
		{
			return first * second;
		}

		[MosaUnitTest(Series = "I4I4")]
		public static int DivI4I4(int first, int second)
		{
			return first / second;
		}

		[MosaUnitTest(Series = "I4I4")]
		public static int RemI4I4(int first, int second)
		{
			return first % second;
		}

		[MosaUnitTest(Series = "I4")]
		public static int RetI4(int first)
		{
			return first;
		}

		[MosaUnitTest(Series = "I4I4")]
		public static int AndI4I4(int first, int second)
		{
			return first & second;
		}

		[MosaUnitTest(Series = "I4I4")]
		public static int OrI4I4(int first, int second)
		{
			return first | second;
		}

		[MosaUnitTest(Series = "I4I4")]
		public static int XorI4I4(int first, int second)
		{
			return first ^ second;
		}

		[MosaUnitTest(Series = "I4")]
		public static int CompI4(int first)
		{
			return ~first;
		}

		[MosaUnitTest(Series = "I4U1UpTo32")]
		public static int ShiftLeftI4I4(int first, byte second)
		{
			return first << second;
		}

		[MosaUnitTest(Series = "I4U1UpTo32")]
		public static int ShiftRightI4I4(int first, byte second)
		{
			return first >> second;
		}

		[MosaUnitTest(Series = "I4I4")]
		public static bool CeqI4I4(int first, int second)
		{
			return first == second;
		}

		[MosaUnitTest(Series = "I4I4")]
		public static bool CltI4I4(int first, int second)
		{
			return first < second;
		}

		[MosaUnitTest(Series = "I4I4")]
		public static bool CgtI4I4(int first, int second)
		{
			return first > second;
		}

		[MosaUnitTest(Series = "I4I4")]
		public static bool CleI4I4(int first, int second)
		{
			return first <= second;
		}

		[MosaUnitTest(Series = "I4I4")]
		public static bool CgeI4I4(int first, int second)
		{
			return first >= second;
		}

		[MosaUnitTest]
		public static bool Newarr()
		{
			int[] arr = new int[0];
			return arr != null;
		}

		[MosaUnitTest(Series = "I4Small")]
		public static bool Ldlen(int length)
		{
			int[] arr = new int[length];
			return arr.Length == length;
		}

		[MosaUnitTest(Series = "I4SmallI4")]
		public static int Ldelem(int index, int value)
		{
			int[] arr = new int[index + 1];
			arr[index] = value;
			return arr[index];
		}

		[MosaUnitTest(Series = "I4SmallI4")]
		public static bool Stelem(int index, int value)
		{
			int[] arr = new int[index + 1];
			arr[index] = value;
			return true;
		}

		[MosaUnitTest(Series = "I4SmallI4")]
		public static int Ldelema(int index, int value)
		{
			int[] arr = new int[index + 1];
			SetValueInRefValue(ref arr[index], value);
			return arr[index];
		}

		private static void SetValueInRefValue(ref int destination, int value)
		{
			destination = value;
		}
	}
}
