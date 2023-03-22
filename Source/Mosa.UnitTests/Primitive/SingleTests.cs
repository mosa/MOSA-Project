// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.UnitTests.Primitive;

public static class SingleTests
{
	[MosaUnitTest(Series = "R4R4")]
	public static float AddR4R4(float first, float second)
	{
		return first + second;
	}

	[MosaUnitTest(Series = "R4R4")]
	public static float SubR4R4(float first, float second)
	{
		return first - second;
	}

	[MosaUnitTest(Series = "R4R4")]
	public static float MulR4R4(float first, float second)
	{
		return first * second;
	}

	[MosaUnitTest(Series = "R4R4NoZero")]
	public static float DivR4R4(float first, float second)
	{
		return first / second;
	}

	//[MosaUnitTest(Series = "R4R4NoZero")]
	public static float RemR4R4(float first, float second)
	{
		return first % second;
	}

	[MosaUnitTest(Series = "R4R4")]
	public static bool CeqR4R4(float first, float second)
	{
		return first.CompareTo(second) == 0;
	}

	[MosaUnitTest(Series = "R4R4")]
	public static bool CneqR4R4(float first, float second)
	{
		return first.CompareTo(second) != 0;
	}

	[MosaUnitTest(Series = "R4R4")]
	public static bool CltR4R4(float first, float second)
	{
		return first.CompareTo(second) < 0;
	}

	[MosaUnitTest(Series = "R4R4")]
	public static bool CgtR4R4(float first, float second)
	{
		return first.CompareTo(second) > 0;
	}

	[MosaUnitTest(Series = "R4R4")]
	public static bool CleR4R4(float first, float second)
	{
		return first.CompareTo(second) <= 0;
	}

	[MosaUnitTest(Series = "R4R4")]
	public static bool CgeR4R4(float first, float second)
	{
		return first.CompareTo(second) >= 0;
	}

	[MosaUnitTest]
	public static bool Newarr()
	{
		var arr = new float[0];
		return arr != null;
	}

	[MosaUnitTest(Series = "I4Small")]
	public static bool Ldlen(int length)
	{
		var arr = new float[length];
		return arr.Length == length;
	}

	[MosaUnitTest(Series = "I4SmallR4Simple")]
	public static bool Ldelem(int index, float value)
	{
		var arr = new float[index + 1];
		arr[index] = value;
		return value == arr[index];
	}

	[MosaUnitTest(Series = "I4SmallR4Simple")]
	public static bool Stelem(int index, float value)
	{
		var arr = new float[index + 1];
		arr[index] = value;
		return true;
	}

	[MosaUnitTest(Series = "I4SmallR4Simple")]
	public static bool Ldelema(int index, float value)
	{
		var arr = new float[index + 1];
		SetValueInRefValue(ref arr[index], value);
		return arr[index] == value;
	}

	private static void SetValueInRefValue(ref float destination, float value)
	{
		destination = value;
	}

	[MosaUnitTest(Series = "R4")]
	public static bool IsNaN(float value)
	{
		return Single.IsNaN(value);
	}
}
