// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.UnitTests.TinyCoreLib;

public static class EqualityComparerTests
{
	[MosaUnitTest]
	public static bool Test_EqualityComparer_Default_Int()
	{
		var comparer = EqualityComparer<int>.Default;
		return comparer != null;
	}

	[MosaUnitTest]
	public static bool Test_EqualityComparer_Default_String()
	{
		var comparer = EqualityComparer<string>.Default;
		return comparer != null;
	}

	[MosaUnitTest(Series = "I4")]
	public static bool Test_EqualityComparer_Equals_SameValues(int value)
	{
		var comparer = EqualityComparer<int>.Default;
		return comparer.Equals(value, value);
	}

	[MosaUnitTest(Series = "I4")]
	public static bool Test_EqualityComparer_Equals_DifferentValues(int value)
	{
		var comparer = EqualityComparer<int>.Default;
		return !comparer.Equals(value, value + 1);
	}

	[MosaUnitTest(Series = "I4")]
	public static bool Test_EqualityComparer_GetHashCode_SameValue(int value)
	{
		var comparer = EqualityComparer<int>.Default;
		int hash1 = comparer.GetHashCode(value);
		int hash2 = comparer.GetHashCode(value);
		return hash1 == hash2;
	}

	[MosaUnitTest(Series = "I4")]
	public static bool Test_EqualityComparer_GetHashCode_DifferentValues(int value)
	{
		var comparer = EqualityComparer<int>.Default;
		int hash1 = comparer.GetHashCode(value);
		int hash2 = comparer.GetHashCode(value + 1);
		return hash1 != hash2;
	}

	[MosaUnitTest]
	public static bool Test_EqualityComparer_Null_Handling()
	{
		var comparer = EqualityComparer<string?>.Default;
		return comparer.Equals(null, null) && !comparer.Equals(null, "MOSA");
	}
}
