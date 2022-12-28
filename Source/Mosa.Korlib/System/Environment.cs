using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System;

public static class Environment
{
	public static int ProcessorCount => GetProcessorCount();

	public const string NewLine = "\n";

	[DoesNotReturn]
	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void Exit(int exitCode);

	[DoesNotReturn]
	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void FailFast(string message);

	[DoesNotReturn]
	public static void FailFast(string message, Exception exception)
	{
		FailFast(message + NewLine + "Exception: " + exception.Message);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern int GetProcessorCount();
}
