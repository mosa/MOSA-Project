// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.x86;

/// <summary>
/// Unit Test Engine
/// </summary>
public static class UnitTestEngine
{
	private const byte MaxBuffer = 255;

	private static Pointer Buffer;

	private static bool Enabled;
	private static bool ReadySent;

	private static uint UsedBuffer;

	private static ushort ComPort;

	public static void Setup(ushort comPort)
	{
		ComPort = comPort;

		Serial.Setup(ComPort);

		Buffer = new Pointer(Address.DebuggerBuffer);

		Enabled = true;
		ReadySent = false;
	}

	private static void SendByte(byte b) => Serial.Write(ComPort, b);

	private static void SendByte(int i) => SendByte((byte)i);

	private static void SendInteger(int i)
	{
		SendByte(i & 0xFF);
		SendByte(i >> 8 & 0xFF);
		SendByte(i >> 16 & 0xFF);
		SendByte(i >> 24 & 0xFF);
	}

	private static void SendInteger(uint i) => SendInteger((int)i);

	private static void SendInteger(ulong i)
	{
		SendInteger((uint)(i & 0xFFFFFFFF));
		SendInteger((uint)((i >> 32) & 0xFFFFFFFF));
	}

	private const int HeaderSize = 4 + 1;

	public static void SendResponse(uint id, ulong data)
	{
		SendInteger(id);
		SendInteger(data);
	}

	private static uint GetUInt32(uint offset) => Buffer.Load32(offset);

	private static byte GetUInt8(uint offset) => Buffer.Load8(offset);

	public static void Process()
	{
		if (!Enabled)
			return;

		if (!ReadySent)
		{
			SendResponse(0, 0l);
			ReadySent = true;
		}

		UnitTestQueue.Process();

		for (var x = 0; x < 5; x++)
		{
			for (var i = 0; i < 75; i++)
			{
				while (ProcessSerial()) ;
			}

			UnitTestQueue.Process();
		}
	}

	private static bool ProcessSerial()
	{
		if (!Serial.IsDataReady(ComPort))
			return false;

		var b = Serial.Read(ComPort);

		if (UsedBuffer + 1 > MaxBuffer)
		{
			UsedBuffer = 0;
			return true;
		}

		Buffer.Store8(UsedBuffer++, b);

		if (UsedBuffer >= HeaderSize)
		{
			var length = GetUInt8(4);

			if (length > MaxBuffer)
			{
				UsedBuffer = 0;
				return true;
			}

			if (UsedBuffer == length + HeaderSize)
			{
				ProcessCommand();
				UsedBuffer = 0;
			}
		}

		return true;
	}

	private static void ProcessCommand()
	{
		var id = GetUInt32(0);

		Screen.Goto(13, 0);
		Screen.ClearRow();
		Screen.Write("[Data]");
		Screen.NextLine();
		Screen.ClearRow();
		Screen.Write("ID: ");
		Screen.Write(id, 10, 5);

		QueueUnitTest();
	}

	private static void QueueUnitTest()
	{
		var id = GetUInt32(0);
		var length = GetUInt8(4);

		var start = Buffer + HeaderSize;
		var end = start + length;

		UnitTestQueue.QueueUnitTest(id, start, end);
	}
}
