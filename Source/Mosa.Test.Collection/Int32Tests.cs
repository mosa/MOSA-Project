/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Test.Collection
{
	public static class Int32Tests
	{
		public static int AddI4I4(int first, int second)
		{
			return (first + second);
		}

		public static int SubI4I4(int first, int second)
		{
			return (first - second);
		}

		public static int MulI4I4(int first, int second)
		{
			return (first * second);
		}

		public static int DivI4I4(int first, int second)
		{
			return (first / second);
		}

		public static int RemI4I4(int first, int second)
		{
			return (first % second);
		}

		public static int RetI4(int first)
		{
			return first;
		}

		public static int AndI4I4(int first, int second)
		{
			return (first & second);
		}

		public static int OrI4I4(int first, int second)
		{
			return (first | second);
		}

		public static int XorI4I4(int first, int second)
		{
			return (first ^ second);
		}

		public static int CompI4(int first)
		{
			return (~first);
		}

		public static int ShiftLeftI4I4(int first, byte second)
		{
			return (first << second);
		}

		public static int ShiftRightI4I4(int first, byte second)
		{
			return (first >> second);
		}

		public static bool CeqI4I4(int first, int second)
		{
			return (first == second);
		}

		public static bool CltI4I4(int first, int second)
		{
			return (first < second);
		}

		public static bool CgtI4I4(int first, int second)
		{
			return (first > second);
		}

		public static bool CleI4I4(int first, int second)
		{
			return (first <= second);
		}

		public static bool CgeI4I4(int first, int second)
		{
			return (first >= second);
		}

		public static bool Newarr()
		{
			int[] arr = new int[0];
			return arr != null;
		}

		public static bool Ldlen(int length)
		{
			int[] arr = new int[length];
			return arr.Length == length;
		}

		public static bool Ldelem(int index, int value)
		{
			int[] arr = new int[index + 1];
			arr[index] = value;
			return value == arr[index];
		}

		public static bool Stelem(int index, int value)
		{
			int[] arr = new int[index + 1];
			arr[index] = value;
			return true;
		}

		public static bool Ldelema(int index, int value)
		{
			int[] arr = new int[index + 1];
			SetValueInRefValue(ref arr[index], value);
			return arr[index] == value;
		}

		private static void SetValueInRefValue(ref int destination, int value)
		{
			destination = value;
		}
	}
}