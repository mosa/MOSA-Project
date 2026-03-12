// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Utility.UnitTests;

internal static class Program
{
	private static void Main(string[] args)
	{
		RegisterPlatforms();

		var unitTestSystem = new UnitTestSystem();

		var returncode = unitTestSystem.Start(args);

		Environment.Exit(returncode);
	}

	private static void RegisterPlatforms()
	{
		PlatformRegistry.Add(new Compiler.x86.Architecture());
		PlatformRegistry.Add(new Compiler.x64.Architecture());
		PlatformRegistry.Add(new Compiler.ARM32.Architecture());
		//PlatformRegistry.Add(new Platform.ARM64.Architecture());
	}
}
