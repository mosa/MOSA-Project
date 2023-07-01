// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal.x86;

/// <summary>
/// Debug
/// </summary>
public static class Debug
{
	#region Private Members

	private static ushort SerialPort = Serial.COM1;
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
	}

	public static void Write(byte c)
	{
		if (!IsIntialize)
			Initalized();

		Serial.Write(Serial.COM1, c);
	}

	#endregion Public API
}
