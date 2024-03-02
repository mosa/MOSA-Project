// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Runtime.Plug;

namespace Mosa.Kernel.BareMetal.Korlib;

public static class ConsolePlug
{
	[Plug("System.Console::Clear")]
	public static void Clear()
	{
		ScreenConsole.ClearScreen();
	}

	[Plug("System.Console::WriteLine")]
	public static void WriteLine(string value)
	{
		ScreenConsole.WriteLine(value);
	}

	[Plug("System.Console::WriteLine")]
	public static void WriteLine()
	{
		ScreenConsole.WriteLine();
	}

	[Plug("System.Console::Write")]
	public static void Write(string value)
	{
		ScreenConsole.Write(value);
	}

	[Plug("System.Console::Write")]
	public static void Write(char value)
	{
		ScreenConsole.Write(value);
	}

	[Plug("System.Console::UpdateForegroundColor")]
	public static void UpdateForegroundColor(ConsoleColor color)
	{
		ScreenConsole.SetForground(Convert(color));
	}

	[Plug("System.Console::UpdateBackgroundColor")]
	public static void UpdateBackgroundColor(ConsoleColor color)
	{
		ScreenConsole.SetBackground(Convert(color));
	}

	[Plug("System.Console::SetCursorPosition")]
	public static void SetCursorPosition(int left, int top)
	{
		ScreenConsole.Goto((uint)top, (uint)left);
	}

	[Plug("System.Console::ReadLine")]
	public static string ReadLine()
	{
		var length = 0;
		var buffer = new char[1024];

		for (;;)
		{
			var key = Kernel.Keyboard.GetKeyPressed(true);
			if (key == null) continue;

			switch (key.Character)
			{
				case '\n': // Enter key
				{
					ScreenConsole.Write(ScreenConsole.Newline);
					return new string(buffer, 0, length);
				}
				case '\b': // Backspace key
				{
					if (length > 0)
					{
						ScreenConsole.Write(ScreenConsole.Backspace);
						ScreenConsole.Write(' ');
						ScreenConsole.Write(ScreenConsole.Backspace);
						length--;
					}
					break;
				}
				default: // Any other key
				{
					buffer[length++] = key.Character;
					ScreenConsole.Write(key.Character);
					break;
				}
			}
		}
	}

	private static ScreenColor Convert(ConsoleColor color)
	{
		return color switch
		{
			ConsoleColor.White => ScreenColor.White,
			ConsoleColor.Black => ScreenColor.Black,
			ConsoleColor.Blue => ScreenColor.Blue,
			ConsoleColor.Green => ScreenColor.Green,
			ConsoleColor.Cyan => ScreenColor.Cyan,
			ConsoleColor.Red => ScreenColor.Red,
			ConsoleColor.Magenta => ScreenColor.Magenta,
			ConsoleColor.Brown => ScreenColor.Magenta,
			ConsoleColor.Gray => ScreenColor.BrightWhite,
			ConsoleColor.DarkGray => ScreenColor.BrightBlack,
			ConsoleColor.LightBlue => ScreenColor.BrightBlue,
			ConsoleColor.LightGreen => ScreenColor.BrightGreen,
			ConsoleColor.LightCyan => ScreenColor.BrightCyan,
			ConsoleColor.LightRed => ScreenColor.BrightRed,
			ConsoleColor.LightMagenta => ScreenColor.BrightMagenta,
			ConsoleColor.Yellow => ScreenColor.Yellow,
			_ => ScreenColor.White,
		};
	}
}
