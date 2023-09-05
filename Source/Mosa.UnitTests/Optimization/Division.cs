// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Optimization;

public static class Division
{
	[MosaUnitTest(Series = "U4")]
	public static uint DivisionBy3(uint a)
	{
		return a / 3;
	}

	[MosaUnitTest(Series = "U4")]
	public static uint DivisionBy7(uint a)
	{
		return a / 7;
	}

	[MosaUnitTest(Series = "U4")]
	public static uint DivisionBy11(uint a)
	{
		return a / 11;
	}

	[MosaUnitTest(Series = "U4")]
	public static uint DivisionBy13(uint a)
	{
		return a / 13;
	}
}
