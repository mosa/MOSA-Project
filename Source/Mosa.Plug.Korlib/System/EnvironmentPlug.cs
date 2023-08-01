using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Mosa.Runtime.Plug;

namespace Mosa.Plug.Korlib.System;

public static class EnvironmentPlug
{
	public const string NewLine = "\n";

	[DoesNotReturn]
	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void Exit(int exitCode);

	[DoesNotReturn]
	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void FailFast(string message);

	[Plug("System.Environment::GetProcessorCount")]
	internal static int GetProcessorCount() => 1; // TODO: APIC
}
