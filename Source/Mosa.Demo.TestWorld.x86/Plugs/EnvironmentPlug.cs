// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Plug;

namespace Mosa.Demo.TestWorld.x86.Plugs;

public static class EnvironmentPlug
{
	[Plug("System.Environment::GetProcessorCount")]
	internal static int GetProcessorCount() => 1; // TODO: APIC
}
