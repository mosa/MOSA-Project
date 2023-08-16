// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Plug;
using Mosa.Runtime.x86;

namespace Mosa.BareBones.TestWorld.x86;

public static class EnvironmentPlug
{
	[Plug("System.Environment::FailFast")]
	public static void FailFast(string message)
	{
		for (; ; ) Native.Hlt();
	}
}
