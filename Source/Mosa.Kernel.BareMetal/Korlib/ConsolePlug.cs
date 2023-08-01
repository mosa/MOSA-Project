// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Runtime.Plug;

namespace Mosa.Kernel.BareMetal.Korlib;

public static class ConsolePlug
{
	private static ConsoleColor forgroundColor = ConsoleColor.White;
	private static ConsoleColor backgroundColor = ConsoleColor.Black;

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

	[Plug("System.Console::SetForegroundColor")]
	public static void SetForegroundColor(ConsoleColor color)
	{
		forgroundColor = color;
		ScreenConsole.SetForground(Convert(color));
	}

	[Plug("System.Console::SetBackgroundColor")]
	public static void SetBackgroundColor(ConsoleColor color)
	{
		backgroundColor = color;
		ScreenConsole.SetBackground(Convert(color));
	}

	[Plug("System.Console::GetForegroundColor")]
	public static ConsoleColor GetForegroundColor()
	{
		return forgroundColor;
	}

	[Plug("System.Console::GetBackgroundColor")]
	public static ConsoleColor GetBackgroundColor()
	{
		return backgroundColor;
	}

	[Plug("System.Console::SetCursorPosition")]
	public static void SetCursorPosition(int left, int top)
	{
		ScreenConsole.Goto((uint)top, (uint)left);
	}

	[Plug("System.Console::ResetColor")]
	public static void ResetColor()
	{
		SetBackgroundColor(ConsoleColor.Black);
		SetForegroundColor(ConsoleColor.White);
	}

	private static ConsoleColor Convert(ScreenColor color)
	{
		return color switch
		{
			ScreenColor.White => ConsoleColor.White,
			ScreenColor.Black => ConsoleColor.Black,
			ScreenColor.Red => ConsoleColor.Red,
			ScreenColor.Green => ConsoleColor.Green,
			ScreenColor.Yellow => ConsoleColor.Yellow,
			ScreenColor.Blue => ConsoleColor.Blue,
			ScreenColor.Magenta => ConsoleColor.Magenta,
			ScreenColor.Cyan => ConsoleColor.Cyan,
			ScreenColor.BrightBlack => ConsoleColor.DarkGray,
			ScreenColor.BrightRed => ConsoleColor.LightRed,
			ScreenColor.BrightGreen => ConsoleColor.LightGreen,
			ScreenColor.BrightYellow => ConsoleColor.Yellow,
			ScreenColor.BrightBlue => ConsoleColor.LightBlue,
			ScreenColor.BrightMagenta => ConsoleColor.LightMagenta,
			ScreenColor.BrightCyan => ConsoleColor.LightCyan,
			ScreenColor.BrightWhite => ConsoleColor.Gray,
			_ => ConsoleColor.White,
		};
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
