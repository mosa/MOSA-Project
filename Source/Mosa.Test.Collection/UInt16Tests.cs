// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Test.Collection
{
	public static class UInt16Tests
	{
		public static int AddU2U2(ushort first, ushort second)
		{
			return (first + second);
		}

		public static int SubU2U2(ushort first, ushort second)
		{
			return (first - second);
		}

		public static int MulU2U2(ushort first, ushort second)
		{
			return (first * second);
		}

		public static int DivU2U2(ushort first, ushort second)
		{
			return (first / second);
		}

		public static int RemU2U2(ushort first, ushort second)
		{
			return (first % second);
		}

		public static ushort RetU2(ushort first)
		{
			return first;
		}

		public static int AndU2U2(ushort first, ushort second)
		{
			return (first & second);
		}

		public static int OrU2U2(ushort first, ushort second)
		{
			return (first | second);
		}

		public static int XorU2U2(ushort first, ushort second)
		{
			return (first ^ second);
		}

		public static int CompU2(ushort first)
		{
			return (~first);
		}

		public static int ShiftLeftU2U2(ushort first, byte second)
		{
			return (first << second);
		}

		public static int ShiftRightU2U2(ushort first, byte second)
		{
			return (first >> second);
		}

		public static bool CeqU2U2(ushort first, ushort second)
		{
			return (first == second);
		}

		public static bool CltU2U2(ushort first, ushort second)
		{
			return (first < second);
		}

		public static bool CgtU2U2(ushort first, ushort second)
		{
			return (first > second);
		}

		public static bool CleU2U2(ushort first, ushort second)
		{
			return (first <= second);
		}

		public static bool CgeU2U2(ushort first, ushort second)
		{
			return (first >= second);
		}

		public static bool Newarr()
		{
			ushort[] arr = new ushort[0];
			return arr != null;
		}

		public static bool Ldlen(int length)
		{
			ushort[] arr = new ushort[length];
			return arr.Length == length;
		}

		public static ushort Ldelem(int index, ushort value)
		{
			ushort[] arr = new ushort[index + 1];
			arr[index] = value;
			return arr[index];
		}

		public static bool Stelem(int index, ushort value)
		{
			ushort[] arr = new ushort[index + 1];
			arr[index] = value;
			return true;
		}

		public static ushort Ldelema(int index, ushort value)
		{
			ushort[] arr = new ushort[index + 1];
			SetValueInRefValue(ref arr[index], value);
			return arr[index];
		}

		private static void SetValueInRefValue(ref ushort destination, ushort value)
		{
			destination = value;
		}
	}
}
