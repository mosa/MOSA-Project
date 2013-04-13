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
	public static class UInt64Tests
	{
		public static ulong AddU8U8(ulong first, ulong second)
		{
			return (first + second);
		}

		public static ulong SubU8U8(ulong first, ulong second)
		{
			return (first - second);
		}

		public static ulong MulU8U8(ulong first, ulong second)
		{
			return (first * second);
		}

		public static ulong DivU8U8(ulong first, ulong second)
		{
			return (first / second);
		}

		public static ulong RemU8U8(ulong first, ulong second)
		{
			return (first % second);
		}

		public static ulong RetU8(ulong first)
		{
			return first;
		}

		public static ulong AndU8U8(ulong first, ulong second)
		{
			return (first & second);
		}

		public static ulong OrU8U8(ulong first, ulong second)
		{
			return (first | second);
		}

		public static ulong XorU8U8(ulong first, ulong second)
		{
			return (first ^ second);
		}

		public static ulong CompU8(ulong first)
		{
			return (~first);
		}

		public static ulong ShiftLeftU8U8(ulong first, byte second)
		{
			return (first << second);
		}

		public static ulong ShiftRightU8U8(ulong first, byte second)
		{
			return (first >> second);
		}

		public static bool CeqU8U8(ulong first, ulong second)
		{
			return (first == second);
		}

		public static bool CltU8U8(ulong first, ulong second)
		{
			return (first < second);
		}

		public static bool CgtU8U8(ulong first, ulong second)
		{
			return (first > second);
		}

		public static bool CleU8U8(ulong first, ulong second)
		{
			return (first <= second);
		}

		public static bool CgeU8U8(ulong first, ulong second)
		{
			return (first >= second);
		}

		public static bool Newarr()
		{
			ulong[] arr = new ulong[0];
			return arr != null;
		}

		public static bool Ldlen(int length)
		{
			ulong[] arr = new ulong[length];
			return arr.Length == length;
		}

		public static bool Ldelem(int index, ulong value)
		{
			ulong[] arr = new ulong[index + 1];
			arr[index] = value;
			return value == arr[index];
		}

		public static bool Stelem(int index, ulong value)
		{
			ulong[] arr = new ulong[index + 1];
			arr[index] = value;
			return true;
		}

		public static bool Ldelema(int index, ulong value)
		{
			ulong[] arr = new ulong[index + 1];
			SetValueInRefValue(ref arr[index], value);
			return arr[index] == value;
		}

		private static void SetValueInRefValue(ref ulong destination, ulong value)
		{
			destination = value;
		}
	}
}