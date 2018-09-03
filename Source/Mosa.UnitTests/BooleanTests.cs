// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests
{
	public static class BooleanTests
	{
		[MosaUnitTest(Series = "B")]
		public static bool RetB(bool first)
		{
			return first;
		}

		[MosaUnitTest(Series = "BB")]
		public static bool AndBB(bool first, bool second)
		{
			return first & second;
		}

		[MosaUnitTest(Series = "BB")]
		public static bool OrBB(bool first, bool second)
		{
			return first | second;
		}

		[MosaUnitTest(Series = "BB")]
		public static bool XorBB(bool first, bool second)
		{
			return first ^ second;
		}

		[MosaUnitTest(Series = "B")]
		public static bool NotB(bool first)
		{
			return !first;
		}

		[MosaUnitTest]
		public static bool Newarr()
		{
			bool[] arr = new bool[0];
			return arr != null;
		}

		[MosaUnitTest(Series = "I4Small")]
		public static bool Ldlen(int length)
		{
			bool[] arr = new bool[length];
			return arr.Length == length;
		}

		[MosaUnitTest(Series = "I4SmallB")]
		public static bool Ldelem(int index, bool value)
		{
			bool[] arr = new bool[index + 1];
			arr[index] = value;
			return value == arr[index];
		}

		[MosaUnitTest(Series = "I4SmallB")]
		public static bool Stelem(int index, bool value)
		{
			bool[] arr = new bool[index + 1];
			arr[index] = value;
			return true;
		}

		[MosaUnitTest(Series = "I4SmallB")]
		public static bool Ldelema(int index, bool value)
		{
			bool[] arr = new bool[index + 1];
			SetValueInRefValue(ref arr[index], value);
			return arr[index] == value;
		}

		private static void SetValueInRefValue(ref bool destination, bool value)
		{
			destination = value;
		}
	}
}
