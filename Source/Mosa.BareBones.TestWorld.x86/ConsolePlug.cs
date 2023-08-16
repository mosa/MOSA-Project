// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Runtime.Plug;
using Mosa.Runtime.x86;

namespace Mosa.BareBones.TestWorld.x86;

public static class ConsolePlug
{
	[Plug("System.Console::UpdateForegroundColor")]
	internal static void UpdateForegroundColor(ConsoleColor color)
	{
		for (; ; ) Native.Hlt();
	}

	[Plug("System.Console::UpdateBackgroundColor")]
	internal static void UpdateBackgroundColor(ConsoleColor color)
	{
		for (; ; ) Native.Hlt();
	}
}
