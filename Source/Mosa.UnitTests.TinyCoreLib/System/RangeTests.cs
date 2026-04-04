// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.UnitTests.TinyCoreLib;

public static class RangeTests
{
	// == Constructor tests

	[MosaUnitTest("I4Small", "I4Small")]
	public static bool Test_Range_Constructor(int startVal, int endVal)
	{
		var start = Index.FromStart(startVal);
		var end = Index.FromStart(endVal);
		var range = new Range(start, end);
		return range.Start.Equals(start) && range.End.Equals(end);
	}

	// == All property tests

	[MosaUnitTest]
	public static bool Test_Range_All_Property()
	{
		var all = Range.All;
		return all.Start.Equals(Index.Start) && all.End.Equals(Index.End);
	}

	// == StartAt tests

	[MosaUnitTest(Series = "I4Small")]
	public static bool Test_Range_StartAt(int startVal)
	{
		var start = Index.FromStart(startVal);
		var range = Range.StartAt(start);
		return range.Start.Equals(start) && range.End.Equals(Index.End);
	}

	// == EndAt tests

	[MosaUnitTest(Series = "I4Small")]
	public static bool Test_Range_EndAt(int endVal)
	{
		var end = Index.FromStart(endVal);
		var range = Range.EndAt(end);
		return range.Start.Equals(Index.Start) && range.End.Equals(end);
	}

	// == GetOffsetAndLength tests

	[MosaUnitTest(Series = "I4Small")]
	public static bool Test_Range_GetOffsetAndLength_Valid(int start)
	{
		var range = new Range(Index.FromStart(start), Index.FromStart(11));
		var (offset, length) = range.GetOffsetAndLength(12);
		return offset == start && length == 11 - start;
	}

	[MosaUnitTest(Series = "I4Small")]
	public static bool Test_Range_GetOffsetAndLength_FromEnd(int start)
	{
		var range = new Range(Index.FromStart(start), Index.FromEnd(1));
		var (offset, length) = range.GetOffsetAndLength(12);
		return offset == start && length == 12 - 1 - start;
	}

	// == Equals tests

	[MosaUnitTest("I4Small", "I4Small")]
	public static bool Test_Range_Equals_SameValue(int startVal, int endVal)
	{
		var range1 = new Range(Index.FromStart(startVal), Index.FromStart(endVal));
		var range2 = new Range(Index.FromStart(startVal), Index.FromStart(endVal));
		return range1.Equals(range2) && range1.Equals((object)range2);
	}

	[MosaUnitTest(Series = "I4Small")]
	public static bool Test_Range_Equals_DifferentValue(int startVal)
	{
		var range1 = new Range(Index.FromStart(startVal), Index.FromStart(startVal + 2));
		var range2 = new Range(Index.FromStart(startVal + 1), Index.FromStart(startVal + 2));
		return !range1.Equals(range2);
	}

	// == GetHashCode tests

	[MosaUnitTest("I4Small", "I4Small")]
	public static bool Test_Range_GetHashCode(int startVal, int endVal)
	{
		var range1 = new Range(Index.FromStart(startVal), Index.FromStart(endVal));
		var range2 = new Range(Index.FromStart(startVal), Index.FromStart(endVal));
		return range1.GetHashCode() == range2.GetHashCode();
	}

	// == ToString tests

	[MosaUnitTest("I4Small", "I4Small")]
	public static string Test_Range_ToString(int startVal, int endVal)
	{
		var range = new Range(Index.FromStart(startVal), Index.FromStart(endVal));
		return range.ToString();
	}
}
