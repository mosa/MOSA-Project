// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Primitive;

public static class UInt16Tests
{
	[MosaUnitTest(Series = "U2U2")]
	public static int AddU2U2(ushort first, ushort second)
	{
		return first + second;
	}

	[MosaUnitTest(Series = "U2U2")]
	public static int SubU2U2(ushort first, ushort second)
	{
		return first - second;
	}

	[MosaUnitTest(Series = "U2U2")]
	public static int MulU2U2(ushort first, ushort second)
	{
		return first * second;
	}

	[MosaUnitTest("U2", "U2NotZero")]
	public static int DivU2U2(ushort first, ushort second)
	{
		return first / second;
	}

	[MosaUnitTest("U2", "U2NotZero")]
	public static int RemU2U2(ushort first, ushort second)
	{
		return first % second;
	}

	[MosaUnitTest(Series = "U2")]
	public static ushort RetU2(ushort first)
	{
		return first;
	}

	[MosaUnitTest(Series = "U2U2")]
	public static int AndU2U2(ushort first, ushort second)
	{
		return first & second;
	}

	[MosaUnitTest(Series = "U2U2")]
	public static int OrU2U2(ushort first, ushort second)
	{
		return first | second;
	}

	[MosaUnitTest(Series = "U2U2")]
	public static int XorU2U2(ushort first, ushort second)
	{
		return first ^ second;
	}

	[MosaUnitTest(Series = "U2")]
	public static int CompU2(ushort first)
	{
		return ~first;
	}

	[MosaUnitTest(Series = "U2U1UpTo16")]
	public static int ShiftLeftU2U2(ushort first, byte second)
	{
		return first << second;
	}

	[MosaUnitTest(Series = "U2U1UpTo16")]
	public static int ShiftRightU2U2(ushort first, byte second)
	{
		return first >> second;
	}

	[MosaUnitTest(Series = "U2U2")]
	public static bool CeqU2U2(ushort first, ushort second)
	{
		return first == second;
	}

	[MosaUnitTest(Series = "U2U2")]
	public static bool CltU2U2(ushort first, ushort second)
	{
		return first < second;
	}

	[MosaUnitTest(Series = "U2U2")]
	public static bool CgtU2U2(ushort first, ushort second)
	{
		return first > second;
	}

	[MosaUnitTest(Series = "U2U2")]
	public static bool CleU2U2(ushort first, ushort second)
	{
		return first <= second;
	}

	[MosaUnitTest(Series = "U2U2")]
	public static bool CgeU2U2(ushort first, ushort second)
	{
		return first >= second;
	}

	[MosaUnitTest]
	public static bool Newarr()
	{
		var arr = new ushort[0];
		return arr != null;
	}

	[MosaUnitTest(Series = "I4Small")]
	public static bool Ldlen(int length)
	{
		var arr = new ushort[length];
		return arr.Length == length;
	}

	[MosaUnitTest(Series = "I4SmallU2")]
	public static ushort Ldelem(int index, ushort value)
	{
		var arr = new ushort[index + 1];
		arr[index] = value;
		return arr[index];
	}

	[MosaUnitTest(Series = "I4SmallU2")]
	public static bool Stelem(int index, ushort value)
	{
		var arr = new ushort[index + 1];
		arr[index] = value;
		return true;
	}

	[MosaUnitTest(Series = "I4SmallU2")]
	public static ushort Ldelema(int index, ushort value)
	{
		var arr = new ushort[index + 1];
		SetValueInRefValue(ref arr[index], value);
		return arr[index];
	}

	private static void SetValueInRefValue(ref ushort destination, ushort value)
	{
		destination = value;
	}
}
