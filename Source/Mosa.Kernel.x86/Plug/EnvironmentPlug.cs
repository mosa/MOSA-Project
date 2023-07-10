// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Kernel.x86;
using Mosa.Runtime.Plug;

namespace Mosa.Plug.Korlib.System.x86;

public static class EnvironmentPlug
{
	[Plug("System.Environment::Exit")]
	internal static void Exit(int exitCode)
	{
		throw new NotImplementedException();
	}

	[Plug("System.Environment::FailFast")]
	internal static void FailFast(string message)
	{
		Panic.Error(message);
	}

	[Plug("System.Environment::GetProcessorCount")]
	internal static int GetProcessorCount()
	{
		throw new NotImplementedException();
	}
}
