// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.x86;

namespace Mosa.Kernel.x86;

/// <summary>
/// Client Side Debugger
/// </summary>
public static class Debugger
{
	#region Codes

	public static class DebugCode
	{
		public const byte Ready = 101;
		public const byte ExecuteUnitTest = 200;
	}

	#endregion Codes

	private const int MaxBuffer = 1024 * 64 + 64;

	private static bool enabled;

	private static ushort com = Serial.COM1;

	private static uint index;

	private static bool ready;
	private static bool readysent;

	private static unsafe IDTStack* idt_stack;

	public static void Setup(ushort com)
	{
		Serial.SetupPort(com);
		Debugger.com = com;
		ready = false;
		readysent = false;
		enabled = true;
	}

	public static void Ready()
	{
		ready = true;
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

	// MAGIC-ID-CODE-LEN-DATA

	private const int HeaderSize = 1 + 1 + 4 + 4;

	private static void SendResponseStart(uint id, byte code, uint len)
	{
		// Magic
		SendByte('!');

		// ID
		SendInteger(id);

		// Code
		SendByte(code);

		// Length
		SendInteger(len);
	}

	private static void SendResponse(uint id, byte code)
	{
		SendResponseStart(id, code, 0);
	}

	private static void SendResponse(uint id, byte code, ulong data)
	{
		SendResponseStart(id, code, 8);
		SendInteger(data);
	}

	private static void SendReady()
	{
		SendResponse(0, DebugCode.Ready);
	}

	private static uint GetUInt32(uint offset)
	{
		return new Pointer(Address.DebuggerBuffer).Load32(offset);
	}

	private static uint GetID()
	{
		return GetUInt32(1);
	}

	private static uint GetLength()
	{
		return GetUInt32(6);
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

		if (ready)
		{
			SendTestUnitResponse();
			ProcessTestUnitQueue();
		}

		if (ready & !readysent)
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

			if (ready)
			{
				SendTestUnitResponse();
				ProcessTestUnitQueue();
			}
		}
	}

	private static bool ProcessSerial()
	{
		if (!Serial.IsDataReady(com))
			return false;

		byte b = Serial.Read(com);

		if (index == 0 && b != (byte)'!')
			return true;

		if (index + 1 > MaxBuffer)
		{
			index = 0;
			return true;
		}

		Intrinsic.Store8(new Pointer(Address.DebuggerBuffer), index++, b);

		if (index >= HeaderSize)
		{
			uint length = GetLength();

			if (length > MaxBuffer)
			{
				index = 0;
				return true;
			}

			if (index == length + HeaderSize)
			{
				ProcessCommand();
				index = 0;
			}
		}

		return true;
	}

	private static void ProcessCommand()
	{
		// [0]![1]ID[5]CODE[6]LEN[10]DATA[LEN]

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
		uint id = GetID();
		uint length = GetLength();

		var start = new Pointer(Address.DebuggerBuffer) + HeaderSize;
		var end = start + (int)length;

		UnitTestQueue.QueueUnitTest(id, start, end);
	}

	private static void SendTestUnitResponse()
	{
		if (UnitTestRunner.GetResult(out ulong result, out uint id))
		{
			SendResponse(id, DebugCode.ExecuteUnitTest, result);
		}
	}

	private static void ProcessTestUnitQueue()
	{
		UnitTestQueue.ProcessQueue();
	}
}
