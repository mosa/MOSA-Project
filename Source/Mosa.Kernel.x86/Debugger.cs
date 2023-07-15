// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.x86;

/// <summary>
/// Client Side Debugger
/// </summary>
public static class Debugger
{
	private static Pointer Buffer; // Pointer(Address.DebuggerBuffer)

	private const int MaxBuffer = 1024 * 64 + 64;

	private static bool enabled;

	private static ushort com = Serial.COM1;

	private static uint UsedBuffer;

	private static bool readysent;

	private static unsafe IDTStack* idt_stack;

	public static void Setup(ushort com)
	{
		Serial.SetupPort(com);
		Debugger.com = com;

		Buffer = new Pointer(Address.DebuggerBuffer);

		readysent = false;
		enabled = true;
	}

	private static void SendRawByte(byte b)
	{
		Serial.Write(com, b);
	}

	private static void SendByte(byte b)
	{
		SendRawByte(b);
	}

	private static void SendByte(int i)
	{
		SendByte((byte)i);
	}

	private static void SendInteger(int i)
	{
		SendByte(i & 0xFF);
		SendByte(i >> 8 & 0xFF);
		SendByte(i >> 16 & 0xFF);
		SendByte(i >> 24 & 0xFF);
	}

	private static void SendInteger(uint i)
	{
		SendInteger((int)i);
	}

	private static void SendInteger(ulong i)
	{
		SendInteger((uint)(i & 0xFFFFFFFF));
		SendInteger((uint)((i >> 32) & 0xFFFFFFFF));
	}

	private const int HeaderSize = 4 + 4;

	private static void SendResponseStart(uint id, uint len)
	{
		SendInteger(id);
		SendInteger(len);
	}

	private static void SendResponse(uint id)
	{
		SendResponseStart(id, 0);
	}

	private static void SendResponse(uint id, ulong data)
	{
		SendResponseStart(id, 8);
		SendInteger(data);
	}

	private static void SendReady()
	{
		SendResponse(0);
	}

	private static uint GetID()
	{
		return GetUInt32(0);
	}

	private static uint GetLength()
	{
		return GetUInt32(4);
	}

	private static uint GetUInt32(uint offset)
	{
		return Buffer.Load32(offset);
	}

	internal static unsafe void Process(IDTStack* stack)
	{
		idt_stack = stack;

		Process();
	}

	internal static void Process()
	{
		if (!enabled)
			return;

		SendTestUnitResponse();
		ProcessTestUnitQueue();

		if (!readysent)
		{
			readysent = true;
			SendReady();
		}

		for (var x = 0; x < 5; x++)
		{
			for (var i = 0; i < 75; i++)
			{
				while (ProcessSerial()) ;
			}

			SendTestUnitResponse();
			ProcessTestUnitQueue();
		}
	}

	private static bool ProcessSerial()
	{
		if (!Serial.IsDataReady(com))
			return false;

		var b = Serial.Read(com);

		if (UsedBuffer + 1 > MaxBuffer)
		{
			UsedBuffer = 0;
			return true;
		}

		Buffer.Store8(UsedBuffer++, b);

		if (UsedBuffer >= HeaderSize)
		{
			var length = GetLength();

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
		var id = GetID();

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
		var id = GetID();
		var length = GetLength();

		var start = Buffer + HeaderSize;
		var end = start + (int)length;

		UnitTestQueue.QueueUnitTest(id, start, end);
	}

	private static void SendTestUnitResponse()
	{
		if (UnitTestRunner.GetResult(out ulong result, out uint id))
		{
			SendResponse(id, result);
		}
	}

	private static void ProcessTestUnitQueue()
	{
		UnitTestQueue.ProcessQueue();
	}
}
