// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.x86;
using Mosa.Runtime.Plug;

namespace Mosa.Plug.Korlib.x86.System
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
