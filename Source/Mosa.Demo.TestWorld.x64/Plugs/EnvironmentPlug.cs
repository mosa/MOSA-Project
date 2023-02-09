// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Runtime.Plug;

namespace Mosa.Demo.TestWorld.x64.Plugs;

public static class EnvironmentPlug
{
	[Plug("System.Environment::FailFast")]
	internal static void FailFast(string message)
	{
		Console.WriteLine("PANIC: " + message);
		for (;;) ;
	}

	[Plug("System.Environment::GetProcessorCount")]
	internal static int GetProcessorCount() => 1; // TODO: APIC
}
