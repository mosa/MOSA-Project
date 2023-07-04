// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal;

/// <summary>
/// Log
/// </summary>
public static class Debug
{
	private static byte NewLine = (byte)'\n';

	public static bool IsEnabled { get; set; } = false;

	#region Public API

	public static void Setup(bool enable = false)
	{
		IsEnabled = enable;
	}

	public static void Initialize()
	{
	}

	public static void Enable() => IsEnabled = true;

	public static void Disable() => IsEnabled = true;

	public static void Kill()
	{
		WriteLine("##KILL##");
	}

	public static void WriteLine()
	{
		if (!IsEnabled)
			return;

		Write(NewLine);
	}

	public static void WriteLine(string message)
	{
		if (!IsEnabled)
			return;

		Write(message);
		Write(NewLine);
	}

	public static void Write(string message)
	{
		if (!IsEnabled)
			return;

		foreach (var c in message)
		{
			Write(c);
		}
	}

	public static void Write(char c)
	{
		Write((byte)c);
	}

	public static void Write(byte b)
	{
		if (!IsEnabled)
			return;

		Platform.Debug(b);
	}

	// Helpers

	public static void WriteLine(string message, ulong value)
	{
		if (!IsEnabled)
			return;

		Write(message);
		WriteValue(value);
		Write(NewLine);
	}

	public static void WriteLine(string message, long value)
	{
		WriteLine(message, (ulong)value);
	}

	public static void WriteLine(string message1, ulong value1, string message2, ulong value2)
	{
		if (!IsEnabled)
			return;

		Write(message1);
		WriteValue(value1);
		Write(message2);
		WriteValue(value2);
		Write(NewLine);
	}

	public static void WriteLineHex(string message, ulong value)
	{
		if (!IsEnabled)
			return;

		Write(message);
		WriteValueAsHex(value);
		Write(NewLine);
	}

	public static void WriteLineHex(string message, long value)
	{
		WriteLineHex(message, (ulong)value);
	}

	public static void WriteLineHex(string message1, ulong value1, string message2, ulong value2)
	{
		if (!IsEnabled)
			return;

		Write(message1);
		WriteValueAsHex(value1);
		Write(message2);
		WriteValueAsHex(value2);
		Write(NewLine);
	}

	// Helpers with Color

	public static void WriteLine(ConsoleColor color, string s)
	{
		//SetForground(color);
		WriteLine(s);
	}

	public static void Write(ConsoleColor color, string s)
	{
		//SetForground(color);
		Write(s);
	}

	public static void WriteLine(ConsoleColor color)
	{
		//SetForground(color);
		Write('\n');
	}

	public static void Write(ConsoleColor color)
	{
		//SetForground(color);
	}

	// Following 4 methods copied from Console.cs

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

	// Copied from Console.cs

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
