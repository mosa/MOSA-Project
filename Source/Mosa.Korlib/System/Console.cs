// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace System;

public static class Console
{
	private static ConsoleColor foregroundColor, backgroundColor;

	public static ConsoleColor ForegroundColor
	{
		get => foregroundColor;
		set
		{
			foregroundColor = value;
			UpdateForegroundColor(value);
		}
	}

	public static ConsoleColor BackgroundColor
	{
		get => backgroundColor;
		set
		{
			backgroundColor = value;
			UpdateBackgroundColor(value);
		}
	}

	public static void ResetColor()
	{
		BackgroundColor = ConsoleColor.Black;
		ForegroundColor = ConsoleColor.White;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void Clear();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void WriteLine();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void WriteLine(string value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void Write(string value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void Write(char value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void SetCursorPosition(int left, int top);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern string ReadLine();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void UpdateForegroundColor(ConsoleColor color);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void UpdateBackgroundColor(ConsoleColor color);
}
