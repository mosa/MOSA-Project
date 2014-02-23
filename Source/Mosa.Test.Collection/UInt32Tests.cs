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
	public static class UInt32Tests
	{
		public static uint AddU4U4(uint first, uint second)
		{
			return (first + second);
		}

		public static uint SubU4U4(uint first, uint second)
		{
			return (first - second);
		}

		public static uint MulU4U4(uint first, uint second)
		{
			return (first * second);
		}

		public static uint DivU4U4(uint first, uint second)
		{
			return (first / second);
		}

		public static uint RemU4U4(uint first, uint second)
		{
			return (first % second);
		}

		public static uint RetU4(uint first)
		{
			return first;
		}

		public static uint AndU4U4(uint first, uint second)
		{
			return (first & second);
		}

		public static uint OrU4U4(uint first, uint second)
		{
			return (first | second);
		}

		public static uint XorU4U4(uint first, uint second)
		{
			return (first ^ second);
		}

		public static uint CompU4(uint first)
		{
			return (~first);
		}

		public static uint ShiftLeftU4U1(uint first, byte second)
		{
			return (first << second);
		}

		public static uint ShiftRightU4U1(uint first, byte second)
		{
			return (first >> second);
		}

		public static bool CeqU4U4(uint first, uint second)
		{
			return (first == second);
		}

		public static bool CltU4U4(uint first, uint second)
		{
			return (first < second);
		}

		public static bool CgtU4U4(uint first, uint second)
		{
			return (first > second);
		}

		public static bool CleU4U4(uint first, uint second)
		{
			return (first <= second);
		}

		public static bool CgeU4U4(uint first, uint second)
		{
			return (first >= second);
		}

		public static bool Newarr()
		{
			uint[] arr = new uint[0];
			return arr != null;
		}

		public static bool Ldlen(int length)
		{
			uint[] arr = new uint[length];
			return arr.Length == length;
		}

		public static uint Ldelem(int index, uint value)
		{
			uint[] arr = new uint[index + 1];
			arr[index] = value;
			return arr[index];
		}

		public static bool Stelem(int index, uint value)
		{
			uint[] arr = new uint[index + 1];
			arr[index] = value;
			return true;
		}

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