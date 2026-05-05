// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Platforms;

namespace Mosa.Utility.UnitTestBisector;

internal static class Program
{
	private static void Main(string[] args)
	{
		RegisterPlatforms();

		var system = new UnitTestBisectorSystem();
		var returnCode = system.Start(args);

		Environment.Exit(returnCode);
	}

	private static void RegisterPlatforms()
	{
		PlatformRegistrations.Register();
	}
}
