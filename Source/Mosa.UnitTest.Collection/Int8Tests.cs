// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTest.Collection
{
	public static class Int8Tests
	{
		public static int AddI1I1(sbyte first, sbyte second)
		{
			return (first + second);
		}

		public static int SubI1I1(sbyte first, sbyte second)
		{
			return (first - second);
		}

		public static int MulI1I1(sbyte first, sbyte second)
		{
			return (first * second);
		}

		public static int DivI1I1(sbyte first, sbyte second)
		{
			return (first / second);
		}

		public static int RemI1I1(sbyte first, sbyte second)
		{
			return (first % second);
		}

		public static sbyte RetI1(sbyte first)
		{
			return first;
		}

		public static int AndI1I1(sbyte first, sbyte second)
		{
			return (first & second);
		}

		public static int OrI1I1(sbyte first, sbyte second)
		{
			return (first | second);
		}

		public static int XorI1I1(sbyte first, sbyte second)
		{
			return (first ^ second);
		}

		public static int CompI1(sbyte first)
		{
			return (~first);
		}

		public static int ShiftLeftI1I1(sbyte first, byte second)
		{
			return (first << second);
		}

		public static int ShiftRightI1I1(sbyte first, byte second)
		{
			return (first >> second);
		}

		public static bool CeqI1I1(sbyte first, sbyte second)
		{
			return (first == second);
		}

		public static bool CltI1I1(sbyte first, sbyte second)
		{
			return (first < second);
		}

		public static bool CgtI1I1(sbyte first, sbyte second)
		{
			return (first > second);
		}

		public static bool CleI1I1(sbyte first, sbyte second)
		{
			return (first <= second);
		}

		public static bool CgeI1I1(sbyte first, sbyte second)
		{
			return (first >= second);
		}

		public static bool Newarr()
		{
			sbyte[] arr = new sbyte[0];
			return arr != null;
		}

		public static bool Ldlen(int length)
		{
			sbyte[] arr = new sbyte[length];
			return arr.Length == length;
		}

		public static sbyte Ldelem(int index, sbyte value)
		{
			sbyte[] arr = new sbyte[index + 1];
			arr[index] = value;
			return arr[index];
		}

		public static bool Stelem(int index, sbyte value)
		{
			sbyte[] arr = new sbyte[index + 1];
			arr[index] = value;
			return true;
		}

		public static sbyte Ldelema(int index, sbyte value)
		{
			sbyte[] arr = new sbyte[index + 1];
			SetValueInRefValue(ref arr[index], value);
			return arr[index];
		}

		private static void SetValueInRefValue(ref sbyte destination, sbyte value)
		{
			destination = value;
		}
	}
}
