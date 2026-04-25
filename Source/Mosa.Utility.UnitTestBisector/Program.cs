// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

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
        PlatformRegistry.Add(new Compiler.x86.Architecture());
        PlatformRegistry.Add(new Compiler.x64.Architecture());
        PlatformRegistry.Add(new Compiler.ARM32.Architecture());
    }
}
