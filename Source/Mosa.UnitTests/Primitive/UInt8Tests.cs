// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Primitive
{
	public static class UInt8Tests
	{
		[MosaUnitTest(Series = "U1U1")]
		public static int AddU1U1(byte first, byte second)
		{
			return first + second;
		}

		[MosaUnitTest(Series = "U1U1")]
		public static int SubU1U1(byte first, byte second)
		{
			return first - second;
		}

		[MosaUnitTest(Series = "U1U1")]
		public static int MulU1U1(byte first, byte second)
		{
			return first * second;
		}

		[MosaUnitTest("U1", "U1NotZero")]
		public static int DivU1U1(byte first, byte second)
		{
			return first / second;
		}

		[MosaUnitTest("U1", "U1NotZero")]
		public static int RemU1U1(byte first, byte second)
		{
			return first % second;
		}

		[MosaUnitTest(Series = "U1")]
		public static byte RetU1(byte first)
		{
			return first;
		}

		[MosaUnitTest(Series = "U1U1")]
		public static int AndU1U1(byte first, byte second)
		{
			return first & second;
		}

		[MosaUnitTest(Series = "U1U1")]
		public static int OrU1U1(byte first, byte second)
		{
			return first | second;
		}

		[MosaUnitTest(Series = "U1U1")]
		public static int XorU1U1(byte first, byte second)
		{
			return first ^ second;
		}

		[MosaUnitTest(Series = "U1")]
		public static int CompU1(byte first)
		{
			return ~first;
		}

		[MosaUnitTest(Series = "U1U1UpTo16")]
		public static int ShiftLeftU1U1(byte first, byte second)
		{
			return first << second;
		}

		[MosaUnitTest(Series = "U1U1UpTo16")]
		public static int ShiftRightU1U1(byte first, byte second)
		{
			return first >> second;
		}

		[MosaUnitTest(Series = "U1U1")]
		public static bool CeqU1U1(byte first, byte second)
		{
			return first == second;
		}

		[MosaUnitTest(Series = "U1U1")]
		public static bool CltU1U1(byte first, byte second)
		{
			return first < second;
		}

		[MosaUnitTest(Series = "U1U1")]
		public static bool CgtU1U1(byte first, byte second)
		{
			return first > second;
		}

		[MosaUnitTest(Series = "U1U1")]
		public static bool CleU1U1(byte first, byte second)
		{
			return first <= second;
		}

		[MosaUnitTest(Series = "U1U1")]
		public static bool CgeU1U1(byte first, byte second)
		{
			return first >= second;
		}

		[MosaUnitTest]
		public static bool Newarr()
		{
			byte[] arr = new byte[0];
			return arr != null;
		}

		[MosaUnitTest(Series = "I4Small")]
		public static bool Ldlen(int length)
		{
			byte[] arr = new byte[length];
			return arr.Length == length;
		}

		[MosaUnitTest(Series = "I4SmallU1")]
		public static byte Ldelem(int index, byte value)
		{
			byte[] arr = new byte[index + 1];
			arr[index] = value;
			return arr[index];
		}

		[MosaUnitTest(Series = "I4SmallU1")]
		public static bool Stelem(int index, byte value)
		{
			byte[] arr = new byte[index + 1];
			arr[index] = value;
			return true;
		}

		[MosaUnitTest(Series = "I4SmallU1")]
		public static byte Ldelema(int index, byte value)
		{
			byte[] arr = new byte[index + 1];
			SetValueInRefValue(ref arr[index], value);
			return arr[index];
		}

		private static void SetValueInRefValue(ref byte destination, byte value)
		{
			destination = value;
		}
	}
}
