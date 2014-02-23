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
	public static class Int64Tests
	{
		public static long AddI8I8(long first, long second)
		{
			return (first + second);
		}

		public static long SubI8I8(long first, long second)
		{
			return (first - second);
		}

		public static long MulI8I8(long first, long second)
		{
			return (first * second);
		}

		public static long DivI8I8(long first, long second)
		{
			return (first / second);
		}

		public static long RemI8I8(long first, long second)
		{
			return (first % second);
		}

		public static long RetI8(long first)
		{
			return first;
		}

		public static long AndI8I8(long first, long second)
		{
			return (first & second);
		}

		public static long OrI8I8(long first, long second)
		{
			return (first | second);
		}

		public static long XorI8I8(long first, long second)
		{
			return (first ^ second);
		}

		public static long CompI8(long first)
		{
			return (~first);
		}

		public static long ShiftLeftI8U1(long first, byte second)
		{
			return (first << second);
		}

		public static long ShiftRightI8U1(long first, byte second)
		{
			return (first >> second);
		}

		public static bool CeqI8I8(long first, long second)
		{
			return (first == second);
		}

		public static bool CltI8I8(long first, long second)
		{
			return (first < second);
		}

		public static bool CgtI8I8(long first, long second)
		{
			return (first > second);
		}

		public static bool CleI8I8(long first, long second)
		{
			return (first <= second);
		}

		public static bool CgeI8I8(long first, long second)
		{
			return (first >= second);
		}

		public static bool Newarr()
		{
			long[] arr = new long[0];
			return arr != null;
		}

		public static bool Ldlen(int length)
		{
			long[] arr = new long[length];
			return arr.Length == length;
		}

		public static long Ldelem(int index, long value)
		{
			long[] arr = new long[index + 1];
			arr[index] = value;
			return arr[index];
		}

		public static bool Stelem(int index, long value)
		{
			long[] arr = new long[index + 1];
			arr[index] = value;
			return true;
		}

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