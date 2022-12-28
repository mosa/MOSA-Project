// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.x86;
using Mosa.Runtime.Plug;
using System;

namespace Mosa.Plug.Korlib.System.x86
{
	public static class EnvironmentPlug
	{
		[Plug("System.Environment::FailFast")]
		internal static void FailFast(string message)
		{
			Panic.Error(message);
		}
	}
}
