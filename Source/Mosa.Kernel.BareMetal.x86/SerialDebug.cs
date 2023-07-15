// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal.x86;

/// <summary>
/// SerialDebug
/// </summary>
public static class SerialDebug
{
	#region Private Members

	private const byte NewLine = (byte)'\n';

	private static ushort SerialPort = Serial.COM2;

	private static bool IsIntialize = false;

	#endregion Private Members

	#region Public API

	public static void Setup()
	{
		IsIntialize = false;
	}

	public static void Initalized()
	{
		if (IsIntialize)
			return;

		Serial.Setup(SerialPort);

		IsIntialize = true;
	}

	public static void Write(byte c)
	{
		if (!IsIntialize)
			Initalized();

		Serial.Write(Serial.COM1, c);
	}

	#endregion Public API

	#region Private Methods

	private static void Write(string s)
	{
		foreach (var c in s)
		{
			Write((byte)c);
		}
	}

	private static void WriteLine(string s)
	{
		Write(s);
		Write(NewLine);
	}

	#endregion Private Methods
}
