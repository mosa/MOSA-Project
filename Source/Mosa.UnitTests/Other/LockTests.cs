// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Other;

public static class LockTests
{
	[MosaUnitTest]
	public static bool Test1()
	{
		var o = new object();

		lock (o)
		{
		}

		return true;
	}

	[MosaUnitTest]
	public static bool Test2()
	{
		var o = new object();

		lock (o)
		{
			return false;
		}

#pragma warning disable CS0162 // Unreachable code detected
		return true;
#pragma warning restore CS0162 // Unreachable code detected
	}
}
