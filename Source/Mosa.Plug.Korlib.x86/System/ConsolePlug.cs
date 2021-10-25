// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.x86;
using Mosa.Runtime.Plug;
using System;

namespace Mosa.Plug.Korlib.System.x86
{
	public static class ConsolePlug
	{
		[Plug("System.Console::ResetColor")]
		internal static void ResetColor()
		{
			ConsoleManager.Controller.Boot.Color = (byte)ConsoleColor.White;
			ConsoleManager.Controller.Boot.BackgroundColor = (byte)ConsoleColor.Black;
		}

		[Plug("System.Console::Clear")]
		internal static void Clear()
		{
			ConsoleManager.Controller.Boot.Clear();
		}

		[Plug("System.Console::WriteLine")]
		internal static void WriteLine()
		{
			ConsoleManager.Controller.Boot.WriteLine();
		}

		[Plug("System.Console::WriteLine")]
		internal static void WriteLine(string value)
		{
			ConsoleManager.Controller.Boot.WriteLine(value);
		}

		[Plug("System.Console::Write")]
		internal static void Write(string value)
		{
			ConsoleManager.Controller.Boot.Write(value);
		}

		[Plug("System.Console::SetCursorPosition")]
		internal static void SetCursorPosition(int left, int top)
		{
			ConsoleManager.Controller.Boot.Goto((uint)left, (uint)top);
		}
	}
}