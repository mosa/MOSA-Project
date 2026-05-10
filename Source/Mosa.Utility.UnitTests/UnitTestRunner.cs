// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.UnitTests;

public static class UnitTestRunner
{
	public static List<UnitTestInfo> Discover(string filter = null)
	{
		return Discovery.DiscoverUnitTests(filter);
	}

	public static List<UnitTest> Run(UnitTestEngine unitTestEngine, List<UnitTestInfo> discoveredUnitTests)
	{
		var unitTests = unitTestEngine.PrepareUnitTests(discoveredUnitTests);

		unitTestEngine.QueueUnitTests(unitTests);
		unitTestEngine.WaitUntilComplete();

		return unitTests;
	}
}
