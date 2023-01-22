// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.UnitTests;

namespace Mosa.UnitTests.Primitive
{
	public static class Int16Tests
	{
		[MosaUnitTest(Series = "I2I2")]
		public static int AddI2I2(short first, short second)
		{
			return first + second;
		}

		[MosaUnitTest(Series = "I2I2")]
		public static int SubI2I2(short first, short second)
		{
			return first - second;
		}

		[MosaUnitTest(Series = "I2I2")]
		public static int MulI2I2(short first, short second)
		{
			return first * second;
		}

		[MosaUnitTest("I2", "I2NotZero")]
		public static int DivI2I2(short first, short second)
		{
			return first / second;
		}

		[MosaUnitTest("I2", "I2NotZero")]
		public static int RemI2I2(short first, short second)
		{
			return first % second;
		}

		[MosaUnitTest(Series = "I2")]
		public static short RetI2(short first)
		{
			return first;
		}

		[MosaUnitTest(Series = "I2I2")]
		public static int AndI2I2(short first, short second)
		{
			return first & second;
		}

		[MosaUnitTest(Series = "I2I2")]
		public static int OrI2I2(short first, short second)
		{
			return first | second;
		}

		[MosaUnitTest(Series = "I2I2")]
		public static int XorI2I2(short first, short second)
		{
			return first ^ second;
		}

		[MosaUnitTest(Series = "I2")]
		public static int CompI2(short first)
		{
			return ~first;
		}

		[MosaUnitTest(Series = "I2U1UpTo16")]
		public static int ShiftLeftI2I2(short first, byte second)
		{
			return first << second;
		}

		[MosaUnitTest(Series = "I2U1UpTo16")]
		public static int ShiftRightI2I2(short first, byte second)
		{
			return first >> second;
		}

		[MosaUnitTest(Series = "I2I2")]
		public static bool CeqI2I2(short first, short second)
		{
			return first == second;
		}

		[MosaUnitTest(Series = "I2I2")]
		public static bool CltI2I2(short first, short second)
		{
			return first < second;
		}

		[MosaUnitTest(Series = "I2I2")]
		public static bool CgtI2I2(short first, short second)
		{
			return first > second;
		}

		[MosaUnitTest(Series = "I2I2")]
		public static bool CleI2I2(short first, short second)
		{
			return first <= second;
		}

		[MosaUnitTest(Series = "I2I2")]
		public static bool CgeI2I2(short first, short second)
		{
			return first >= second;
		}

		[MosaUnitTest]
		public static bool Newarr()
		{
			short[] arr = new short[0];
			return arr != null;
		}

		[MosaUnitTest(Series = "I4Small")]
		public static bool Ldlen(int length)
		{
			short[] arr = new short[length];
			return arr.Length == length;
		}

		[MosaUnitTest(Series = "I4SmallI2")]
		public static short Ldelem(int index, short value)
		{
			short[] arr = new short[index + 1];
			arr[index] = value;
			return arr[index];
		}

		[MosaUnitTest(Series = "I4SmallI2")]
		public static bool Stelem(int index, short value)
		{
			short[] arr = new short[index + 1];
			arr[index] = value;
			return true;
		}

		[MosaUnitTest(Series = "I4SmallI2")]
		public static short Ldelema(int index, short value)
		{
			short[] arr = new short[index + 1];
			SetValueInRefValue(ref arr[index], value);
			return arr[index];
		}

		private static void SetValueInRefValue(ref short destination, short value)
		{
			destination = value;
		}
	}
}
