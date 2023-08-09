// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Kernel.x86;
using Mosa.Runtime.Plug;

namespace Mosa.Plug.Korlib.System.x86;

public static class ConsolePlug
{
	[Plug("System.Console::Clear")]
	internal static void Clear()
	{
		Screen.Clear();
	}

	[Plug("System.Console::WriteLine")]
	internal static void WriteLine()
	{
		Screen.WriteLine();
	}

	[Plug("System.Console::WriteLine")]
	internal static void WriteLine(string value)
	{
		Screen.WriteLine(value);
	}

	[Plug("System.Console::Write")]
	internal static void Write(string value)
	{
		Screen.Write(value);
	}

	[Plug("System.Console::Write")]
	internal static void Write(char value)
	{
		Screen.Write(value);
	}

	[Plug("System.Console::SetCursorPosition")]
	internal static void SetCursorPosition(int left, int top)
	{
		Screen.Goto((uint)left, (uint)top);
	}

	[Plug("System.Console::UpdateForegroundColor")]
	internal static void UpdateForegroundColor(ConsoleColor color)
	{
		Screen.Color = (byte)color;
	}

	[Plug("System.Console::UpdateBackgroundColor")]
	internal static void UpdateBackgroundColor(ConsoleColor color)
	{
		Screen.BackgroundColor = (byte)color;
	}
}
