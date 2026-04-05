// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.UnitTests.TinyCoreLib;

public static class IndexTests
{
	// == Constructor tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_Index_Constructor_FromStart(int value)
	{
		var index = new Index(value, false);
		return index.Value == value && !index.IsFromEnd;
	}

	[MosaUnitTest(Series = "I4")]
	public static bool Test_Index_Constructor_FromEnd(int value)
	{
		var index = new Index(value, true);
		return index.Value == value && index.IsFromEnd;
	}

	// == FromStart/FromEnd tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_Index_FromStart_Static(int value)
	{
		var index = Index.FromStart(value);
		return index.Value == value && !index.IsFromEnd;
	}

	[MosaUnitTest(Series = "I4")]
	public static bool Test_Index_FromEnd_Static(int value)
	{
		var index = Index.FromEnd(value);
		return index.Value == value && index.IsFromEnd;
	}

	// == Start/End tests

	[MosaUnitTest]
	public static bool Test_Index_Start_Property()
	{
		var start = Index.Start;
		return start.Value == 0 && !start.IsFromEnd;
	}

	[MosaUnitTest]
	public static bool Test_Index_End_Property()
	{
		var end = Index.End;
		return end.Value == 0 && end.IsFromEnd;
	}

	// == GetOffset tests

	[MosaUnitTest(Series = "I4")]
	public static int Test_Index_GetOffset_FromStart(int value)
	{
		var index = new Index(value, false);
		return index.GetOffset(10);
	}

	[MosaUnitTest(Series = "I4")]
	public static int Test_Index_GetOffset_FromEnd(int value)
	{
		var index = new Index(value, true);
		return index.GetOffset(10);
	}

	// == Implicit conversion tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_Index_ImplicitConversion(int value)
	{
		Index index = value;
		return index.Value == value && !index.IsFromEnd;
	}

	// == Equals tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_Index_Equals_SameValue(int value)
	{
		var index1 = new Index(value, false);
		var index2 = new Index(value, false);
		return index1.Equals(index2) && index1.Equals((object)index2);
	}

	[MosaUnitTest(Series = "I4")]
	public static bool Test_Index_Equals_DifferentValue(int value)
	{
		var index1 = new Index(value, false);
		var index2 = new Index(value, true);
		return !index1.Equals(index2);
	}

	// == GetHashCode tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_Index_GetHashCode(int value)
	{
		var index1 = new Index(value, false);
		var index2 = new Index(value, false);
		return index1.GetHashCode() == index2.GetHashCode();
	}

	// == ToString tests

	[MosaUnitTest(Series = "I4")]
	public static string Test_Index_ToString_FromStart(int value)
	{
		var index = new Index(value, false);
		return index.ToString();
	}

	[MosaUnitTest(Series = "I4")]
	public static string Test_Index_ToString_FromEnd(int value)
	{
		var index = new Index(value, true);
		return index.ToString();
	}
}
