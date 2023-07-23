// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal;

/// <summary>
/// Log
/// </summary>
public static class Debug
{
	private const byte NewLine = (byte)'\n';

	public static bool IsEnabled { get; set; } = false;

	#region Public API

	public static void Setup(bool enable = false)
	{
		IsEnabled = enable;

		WriteLine("[Debug Mode]");
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

	public static void Fatal()
	{
		WriteLine("***Fatal***");
		Kill();

		while (true)
		{
		}
	}

	public static void Assert(bool condition, string message = null)
	{
		if (condition)
			return;

		Write(message);
		WriteLine();
		Kill();
	}

	public static void Assert(bool condition, string message, ulong value)
	{
		if (condition)
			return;

		Write("Assert failed: ");
		Write(message);
		Write(value);
		WriteLine();
		Kill();
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

		if (message == null)
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

		Platform.DebugWrite(b);
	}

	// Helpers

	public static void WriteLine(string message, ulong value)
	{
		if (!IsEnabled)
			return;

		Write(message);
		Write(value);
		Write(NewLine);
	}

	public static void WriteLine(string message, long value)
	{
		if (!IsEnabled)
			return;

		WriteLine(message, (ulong)value);
	}

	public static void WriteLine(string message1, ulong value1, string message2, ulong value2)
	{
		if (!IsEnabled)
			return;

		Write(message1);
		Write(value1);
		Write(message2);
		Write(value2);
		Write(NewLine);
	}

	public static void WriteLine(string message1, ulong value1, string message2, Hex8 value2)
	{
		if (!IsEnabled)
			return;

		Write(message1);
		Write(value1);
		Write(message2);
		Write(value2);
		Write(NewLine);
	}

	public static void Write(Hex value)
	{
		if (!IsEnabled)
			return;

		WriteValue(value.Value, 16, -1);
	}

	public static void Write(Hex8 value)
	{
		if (!IsEnabled)
			return;

		WriteValue(value.Value, 16, 8);
	}

	public static void WriteLine(Hex value)
	{
		if (!IsEnabled)
			return;

		WriteValue(value.Value, 16, -1);
		WriteLine();
	}

	public static void WriteLine(Hex8 value)
	{
		if (!IsEnabled)
			return;

		WriteValue(value.Value, 16, 8);
		WriteLine();
	}

	public static void WriteLine(string message, Hex value)
	{
		if (!IsEnabled)
			return;

		Write(message);
		Write(value);
		Write(NewLine);
	}

	public static void WriteLine(string message, Hex8 value)
	{
		if (!IsEnabled)
			return;

		Write(message);
		Write(value);
		Write(NewLine);
	}

	public static void WriteLine(string message1, Hex value1, string message2, Hex value2)
	{
		if (!IsEnabled)
			return;

		Write(message1);
		Write(value1);
		Write(message2);
		Write(value2);
		Write(NewLine);
	}

	public static void WriteLine(string message1, ulong value1, string message2, Hex value2)
	{
		if (!IsEnabled)
			return;

		Write(message1);
		Write(value1);
		Write(message2);
		Write(value2);
		Write(NewLine);
	}

	// Following 4 methods copied from Console.cs

	public static void Write(ulong value)
	{
		WriteValue(value, 10, -1);
	}

	public static void Write(ulong value, int length)
	{
		WriteValue(value, 10, length);
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

public struct Hex
{
	public ulong Value;

	public Hex(ulong value) => Value = value;

	public Hex(Pointer value) => Value = value.ToUInt64();
}

public struct Hex8
{
	public ulong Value;

	public Hex8(ulong value) => Value = value;

	public Hex8(Pointer value) => Value = value.ToUInt64();
}
