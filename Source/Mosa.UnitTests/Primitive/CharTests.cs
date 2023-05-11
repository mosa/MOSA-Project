// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Primitive;

public static class CharTests
{
	[MosaUnitTest("C", "C")]
	public static int AddCC(char first, char second)
	{
		return first + second;
	}

	[MosaUnitTest("C", "C")]
	public static int SubCC(char first, char second)
	{
		return first - second;
	}

	[MosaUnitTest("C", "C")]
	public static int MulCC(char first, char second)
	{
		return first * second;
	}

	[MosaUnitTest("C", "CNotZero")]
	public static int DivCC(char first, char second)
	{
		return first / second;
	}

	[MosaUnitTest("C", "CNotZero")]
	public static int RemCC(char first, char second)
	{
		return first % second;
	}

	[MosaUnitTest("C")]
	public static char RetC(char first)
	{
		return first;
	}

	[MosaUnitTest("C", "C")]
	public static int AndCC(char first, char second)
	{
		return first & second;
	}

	[MosaUnitTest("C", "C")]
	public static int OrCC(char first, char second)
	{
		return first | second;
	}

	[MosaUnitTest("C", "C")]
	public static int XorCC(char first, char second)
	{
		return first ^ second;
	}

	[MosaUnitTest("C")]
	public static int CompC(char first)
	{
		return ~first;
	}

	[MosaUnitTest("C", "U1")]
	public static int ShiftLeftCC(char first, byte second)
	{
		return first << second;
	}

	[MosaUnitTest("C", "U1")]
	public static int ShiftRightCC(char first, byte second)
	{
		return first >> second;
	}

	[MosaUnitTest("C", "C")]
	public static bool CeqCC(char first, char second)
	{
		return first == second;
	}

	[MosaUnitTest("C", "C")]
	public static bool CltCC(char first, char second)
	{
		return first < second;
	}

	[MosaUnitTest("C", "C")]
	public static bool CgtCC(char first, char second)
	{
		return first > second;
	}

	[MosaUnitTest("C", "C")]
	public static bool CleCC(char first, char second)
	{
		return first <= second;
	}

	[MosaUnitTest("C", "C")]
	public static bool CgeCC(char first, char second)
	{
		return first >= second;
	}

	[MosaUnitTest]
	public static bool Newarr()
	{
		var arr = new char[0];
		return arr != null;
	}

	[MosaUnitTest(Series = "I4Small")]
	public static bool Ldlen(int length)
	{
		var arr = new char[length];
		return arr.Length == length;
	}

	[MosaUnitTest(Series = "I4SmallC")]
	public static bool Ldelem(int index, char value)
	{
		var arr = new char[index + 1];
		arr[index] = value;
		return value == arr[index];
	}

	[MosaUnitTest(Series = "I4SmallC")]
	public static bool Stelem(int index, char value)
	{
		var arr = new char[index + 1];
		arr[index] = value;
		return true;
	}

	[MosaUnitTest(Series = "I4SmallC")]
	public static bool Ldelema(int index, char value)
	{
		var arr = new char[index + 1];
		SetValueInRefValue(ref arr[index], value);
		return arr[index] == value;
	}

	[MosaUnitTest]
	public static bool BitConversion()
	{
		var bytes = System.BitConverter.GetBytes('a');
		return bytes[0] == 97;
	}

	private static void SetValueInRefValue(ref char destination, char value)
	{
		destination = value;
	}

	[MosaUnitTest("I4UpTo8")]
	public static char ArrayAccess(int index)
	{
		var array = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };

		return array[index];
	}
}
