// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Runtime.Plug;

namespace Mosa.Demo.SVGAWorld.x86.Plugs;

public static class EnvironmentPlug
{
	[Plug("System.Environment::Exit")]
	internal static void Exit(int exitCode)
	{
		Boot.PCService.Shutdown();
	}

	[Plug("System.Environment::FailFast")]
	internal static void FailFast(string message)
	{
		Display.Driver.Disable();
		Console.BackgroundColor = ConsoleColor.Black;
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine("***FAIL FAST*** " + message);
		for (; ; );
	}
}
