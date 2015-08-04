// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Test.Collection
{
	public static class BooleanTests
	{
		public static bool RetB(bool first)
		{
			return first;
		}

		public static bool AndBB(bool first, bool second)
		{
			return (first & second);
		}

		public static bool OrBB(bool first, bool second)
		{
			return (first | second);
		}

		public static bool XorBB(bool first, bool second)
		{
			return (first ^ second);
		}

		public static bool NotB(bool first)
		{
			return (!first);
		}

		public static bool Newarr()
		{
			bool[] arr = new bool[0];
			return arr != null;
		}

		public static bool Ldlen(int length)
		{
			bool[] arr = new bool[length];
			return arr.Length == length;
		}

		public static bool Ldelem(int index, bool value)
		{
			bool[] arr = new bool[index + 1];
			arr[index] = value;
			return value == arr[index];
		}

		public static bool Stelem(int index, bool value)
		{
			bool[] arr = new bool[index + 1];
			arr[index] = value;
			return true;
		}

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
