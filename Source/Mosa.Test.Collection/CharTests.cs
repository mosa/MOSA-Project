// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Test.Collection
{
	public static class CharTests
	{
		public static int AddCC(char first, char second)
		{
			return (first + second);
		}

		public static int SubCC(char first, char second)
		{
			return (first - second);
		}

		public static int MulCC(char first, char second)
		{
			return (first * second);
		}

		public static int DivCC(char first, char second)
		{
			return (first / second);
		}

		public static int RemCC(char first, char second)
		{
			return (first % second);
		}

		public static char RetC(char first)
		{
			return first;
		}

		public static int AndCC(char first, char second)
		{
			return (first & second);
		}

		public static int OrCC(char first, char second)
		{
			return (first | second);
		}

		public static int XorCC(char first, char second)
		{
			return (first ^ second);
		}

		public static int CompC(char first)
		{
			return (~first);
		}

		public static int ShiftLeftCC(char first, byte second)
		{
			return (first << second);
		}

		public static int ShiftRightCC(char first, byte second)
		{
			return (first >> second);
		}

		public static bool CeqCC(char first, char second)
		{
			return (first == second);
		}

		public static bool CltCC(char first, char second)
		{
			return (first < second);
		}

		public static bool CgtCC(char first, char second)
		{
			return (first > second);
		}

		public static bool CleCC(char first, char second)
		{
			return (first <= second);
		}

		public static bool CgeCC(char first, char second)
		{
			return (first >= second);
		}

		public static bool Newarr()
		{
			char[] arr = new char[0];
			return arr != null;
		}

		public static bool Ldlen(int length)
		{
			char[] arr = new char[length];
			return arr.Length == length;
		}

		public static bool Ldelem(int index, char value)
		{
			char[] arr = new char[index + 1];
			arr[index] = value;
			return value == arr[index];
		}

		public static bool Stelem(int index, char value)
		{
			char[] arr = new char[index + 1];
			arr[index] = value;
			return true;
		}

		public static bool Ldelema(int index, char value)
		{
			char[] arr = new char[index + 1];
			SetValueInRefValue(ref arr[index], value);
			return arr[index] == value;
		}

		private static void SetValueInRefValue(ref char destination, char value)
		{
			destination = value;
		}
	}
}