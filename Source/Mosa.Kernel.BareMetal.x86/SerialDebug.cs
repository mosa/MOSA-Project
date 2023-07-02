// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal.x86;

/// <summary>
/// SerialDebug
/// </summary>
public static class SerialDebug
{
	#region Private Members

	private static ushort SerialPort = Serial.COM2;
	private static byte NewLine = (byte)'\n';

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

		Serial.SetupPort(SerialPort);

		IsIntialize = true;

		Write("[Debug Mode]");
		Write(NewLine);
	}

	public static void Write(byte c)
	{
		if (!IsIntialize)
			Initalized();

		Serial.Write(Serial.COM1, c);
	}

	#endregion Public API

	private static void Write(string s)
	{
		foreach (var c in s)
		{
			Write((byte)c);
		}
	}
}
