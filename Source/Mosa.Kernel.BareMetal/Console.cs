// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal;

public static class Console
{
	public const byte Escape = 0x1b;
	public const byte Newline = 0x0A;
	public const byte Formfeed = 0x0C;
	public const byte Backspace = 0x08;

	#region Public API

	public static void GotoTop()
	{
		Write(Escape);
		Write("[0;0H");
	}

	public static void Goto(uint row, uint col)
	{
		Write(Escape);
		Write("[");
		WriteValue(col);
		Write(";");
		WriteValue(row);
		Write("H");
	}

	public static void Write(byte c)
	{
		Platform.ConsoleWrite(c);
	}

	public static void Write(char c)
	{
		Write((byte)c);
	}

	public static void Write(string s)
	{
		foreach (var c in s)
		{
			Write((byte)c);
		}
	}

	public static void WriteLine(string s)
	{
		Write(s);
		Write(Newline);
	}

	public static void WriteLine(ConsoleColor color, string s)
	{
		SetForground(color);
		WriteLine(s);
	}

	public static void Write(ConsoleColor color, string s)
	{
		SetForground(color);
		Write(s);
	}

	public static void WriteLine(ConsoleColor color)
	{
		SetForground(color);
		Write(Newline);
	}

	public static void Write(ConsoleColor color)
	{
		SetForground(color);
	}

	public static void WriteLine()
	{
		Write(Newline);
	}

	public static void ClearScreen()
	{
		Write(Escape);
		Write("[2J");
	}

	public static void SetForground(ConsoleColor color)
	{
		var c = (byte)color;

		Write(Escape);
		Write("[");
		Write((byte)((byte)'0' + c / 10 % 10));
		Write((byte)((byte)'0' + c % 10));
		Write("m");
	}

	public static void SetBackground(ConsoleColor color)
	{
		var c = (byte)color + 10;

		Write(Escape);
		Write("[");
		Write((byte)((byte)'0' + c / 10 % 10));
		Write((byte)((byte)'0' + c % 10));
		Write("m");
	}

	public static void WriteValue(ulong value)
	{
		WriteValue(value, 10, -1);
	}

	public static void WriteValueAsHex(ulong value)
	{
		WriteValue(value, 16, -1);
	}

	public static void WriteValue(ulong value, int length)
	{
		WriteValue(value, 10, length);
	}

	public static void WriteValueAsHex(ulong value, int length)
	{
		WriteValue(value, 16, length);
	}

	#endregion Public API

	#region Private API

	private static void WriteValue(ulong value, byte @base, int length)
	{
		int minlength = length >= 0 ? length : (int)GetValueLength(value, @base);

		for (int i = minlength - 1; i >= 0; i--)
		{
			WriteValueDigit(value, i, @base);
		}
	}

	private static uint GetValueLength(ulong value, byte @base)
	{
		uint length = 0;

		do
		{
			value /= @base;
			length++;
		} while (value != 0);

		return length;
	}

	private static void WriteValueDigit(ulong val, int position, byte @base)
	{
		int index = -1;
		ulong digit;

		do
		{
			digit = val % @base;
			val /= @base;
			index++;
		}
		while (index != position);

		if (digit < 10)
			Write((char)('0' + digit));
		else
			Write((char)('A' + digit - 10));
	}

	#endregion Private API
}
