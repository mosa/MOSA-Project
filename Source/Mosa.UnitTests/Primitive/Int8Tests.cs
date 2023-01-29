// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Primitive;

public static class Int8Tests
{
	[MosaUnitTest(Series = "I1I1")]
	public static int AddI1I1(sbyte first, sbyte second)
	{
		return first + second;
	}

	[MosaUnitTest(Series = "I1I1")]
	public static int SubI1I1(sbyte first, sbyte second)
	{
		return first - second;
	}

	[MosaUnitTest(Series = "I1I1")]
	public static int MulI1I1(sbyte first, sbyte second)
	{
		return first * second;
	}

	[MosaUnitTest("I1", "I1NotZero")]
	public static int DivI1I1(sbyte first, sbyte second)
	{
		return first / second;
	}

	[MosaUnitTest("I1", "I1NotZero")]
	public static int RemI1I1(sbyte first, sbyte second)
	{
		return first % second;
	}

	[MosaUnitTest(Series = "I1")]
	public static sbyte RetI1(sbyte first)
	{
		return first;
	}

	[MosaUnitTest(Series = "I1I1")]
	public static int AndI1I1(sbyte first, sbyte second)
	{
		return first & second;
	}

	[MosaUnitTest(Series = "I1I1")]
	public static int OrI1I1(sbyte first, sbyte second)
	{
		return first | second;
	}

	[MosaUnitTest(Series = "I1I1")]
	public static int XorI1I1(sbyte first, sbyte second)
	{
		return first ^ second;
	}

	[MosaUnitTest(Series = "I1")]
	public static int CompI1(sbyte first)
	{
		return ~first;
	}

	[MosaUnitTest(Series = "I1U1UpTo16")]
	public static int ShiftLeftI1I1(sbyte first, byte second)
	{
		return first << second;
	}

	[MosaUnitTest(Series = "I1U1UpTo16")]
	public static int ShiftRightI1I1(sbyte first, byte second)
	{
		return first >> second;
	}

	[MosaUnitTest(Series = "I1I1")]
	public static bool CeqI1I1(sbyte first, sbyte second)
	{
		return first == second;
	}

	[MosaUnitTest(Series = "I1I1")]
	public static bool CltI1I1(sbyte first, sbyte second)
	{
		return first < second;
	}

	[MosaUnitTest(Series = "I1I1")]
	public static bool CgtI1I1(sbyte first, sbyte second)
	{
		return first > second;
	}

	[MosaUnitTest(Series = "I1I1")]
	public static bool CleI1I1(sbyte first, sbyte second)
	{
		return first <= second;
	}

	[MosaUnitTest(Series = "I1I1")]
	public static bool CgeI1I1(sbyte first, sbyte second)
	{
		return first >= second;
	}

	[MosaUnitTest]
	public static bool Newarr()
	{
		sbyte[] arr = new sbyte[0];
		return arr != null;
	}

	[MosaUnitTest(Series = "I4Small")]
	public static bool Ldlen(int length)
	{
		sbyte[] arr = new sbyte[length];
		return arr.Length == length;
	}

	[MosaUnitTest(Series = "I4SmallI1")]
	public static sbyte Ldelem(int index, sbyte value)
	{
		sbyte[] arr = new sbyte[index + 1];
		arr[index] = value;
		return arr[index];
	}

	[MosaUnitTest(Series = "I4SmallI1")]
	public static bool Stelem(int index, sbyte value)
	{
		sbyte[] arr = new sbyte[index + 1];
		arr[index] = value;
		return true;
	}

	[MosaUnitTest(Series = "I4SmallI1")]
	public static sbyte Ldelema(int index, sbyte value)
	{
		sbyte[] arr = new sbyte[index + 1];
		SetValueInRefValue(ref arr[index], value);
		return arr[index];
	}

	private static void SetValueInRefValue(ref sbyte destination, sbyte value)
	{
		destination = value;
	}
}
