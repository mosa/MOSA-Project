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
	public static class Int16Tests
	{
		public static int AddI2I2(short first, short second)
		{
			return (first + second);
		}

		public static int SubI2I2(short first, short second)
		{
			return (first - second);
		}

		public static int MulI2I2(short first, short second)
		{
			return (first * second);
		}

		public static int DivI2I2(short first, short second)
		{
			return (first / second);
		}

		public static int RemI2I2(short first, short second)
		{
			return (first % second);
		}

		public static short RetI2(short first)
		{
			return first;
		}

		public static int AndI2I2(short first, short second)
		{
			return (first & second);
		}

		public static int OrI2I2(short first, short second)
		{
			return (first | second);
		}

		public static int XorI2I2(short first, short second)
		{
			return (first ^ second);
		}

		public static int CompI2(short first)
		{
			return (~first);
		}

		public static int ShiftLeftI2I2(short first, byte second)
		{
			return (first << second);
		}

		public static int ShiftRightI2I2(short first, byte second)
		{
			return (first >> second);
		}

		public static bool CeqI2I2(short first, short second)
		{
			return (first == second);
		}

		public static bool CltI2I2(short first, short second)
		{
			return (first < second);
		}

		public static bool CgtI2I2(short first, short second)
		{
			return (first > second);
		}

		public static bool CleI2I2(short first, short second)
		{
			return (first <= second);
		}

		public static bool CgeI2I2(short first, short second)
		{
			return (first >= second);
		}

		public static bool Newarr()
		{
			short[] arr = new short[0];
			return arr != null;
		}

		public static bool Ldlen(int length)
		{
			short[] arr = new short[length];
			return arr.Length == length;
		}

		public static bool Ldelem(int index, short value)
		{
			short[] arr = new short[index + 1];
			arr[index] = value;
			return value == arr[index];
		}

		public static bool Stelem(int index, short value)
		{
			short[] arr = new short[index + 1];
			arr[index] = value;
			return true;
		}

		public static bool Ldelema(int index, short value)
		{
			short[] arr = new short[index + 1];
			SetValueInRefValue(ref arr[index], value);
			return arr[index] == value;
		}

		private static void SetValueInRefValue(ref short destination, short value)
		{
			destination = value;
		}
	}
}