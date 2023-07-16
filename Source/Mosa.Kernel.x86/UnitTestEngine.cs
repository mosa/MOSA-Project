// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.x86;

namespace Mosa.Kernel.x86;

/// <summary>
/// Unit Test Engine
/// </summary>
public static class UnitTestEngine
{
	private const byte MaxBuffer = 255;
	private const uint MaxParameters = 8; // max 32-bit parameters
	private const uint QueueSize = 0x00100000;

	private static Pointer Buffer;
	private static Pointer Stack;

	private static bool Enabled;
	private static bool ReadySent;
	private static uint UsedBuffer;

	private static ushort ComPort;

	private static bool Ready;
	private static bool ResultReported;

	private static uint TestID;
	private static uint TestParameters;
	private static uint TestMethodAddress;
	private static uint TestResultType;

	private static ulong TestResult;

	private static Pointer Queue;
	private static Pointer QueueNext;
	private static Pointer QueueCurrent;
	private static uint Count;

	public static void Setup(ushort comPort)
	{
		ComPort = comPort;

		Serial.Setup(ComPort);

		Buffer = new Pointer(Address.DebuggerBuffer);

		Enabled = true;
		ReadySent = false;

		Stack = new Pointer(Address.UnitTestStack);

		Ready = false;
		ResultReported = true;

		TestID = 0;
		TestParameters = 0;
		TestMethodAddress = 0;
		TestResult = 0;

		Queue = new Pointer(Address.UnitTestQueue);
		QueueNext = Queue;
		QueueCurrent = Queue;

		Count = 0;

		QueueNext.Store32(0);
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

		ProcessQueue();

		for (var x = 0; x < 5; x++)
		{
			for (var i = 0; i < 75; i++)
			{
				while (ProcessSerial()) ;
			}

			ProcessQueue();
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

		QueueUnitTest(id, start, end);
	}

	public static void EnterTestReadyLoop()
	{
		var testCount = 0u;

		Screen.Write("Waiting for unit tests...");
		Screen.NextLine();
		Screen.NextLine();

		// allocate space on stack for parameters
		var esp = Native.AllocateStackSpace(MaxParameters * 4);

		Screen.Write("Stack @ ");
		Screen.Write(esp, 16, 8);

		Screen.NextLine();
		Screen.NextLine();

		var row = Screen.Row;

		while (true)
		{
			if (Ready)
			{
				Screen.Goto(row, 0);
				Screen.ClearRow();

				Screen.Write("Test #: ");
				Screen.Write(++testCount, 10, 7);

				TestResult = 0;
				ResultReported = false;
				Ready = false;

				// copy parameters into stack
				for (var index = 0; index < TestParameters; index++)
				{
					var value = Stack.Load32(index * 4);

					new Pointer(esp).Store32(index * 4, value);
				}

				switch (TestResultType)
				{
					case 0: Native.FrameCall(TestMethodAddress); break;
					case 1: TestResult = Native.FrameCallRetU4(TestMethodAddress); break;
					case 2: TestResult = Native.FrameCallRetU8(TestMethodAddress); break;
					case 3: TestResult = Native.FrameCallRetR8(TestMethodAddress); break;
					default: break;
				}

				SendResponse(TestID, TestResult);

				ResultReported = true;

				Native.Int(255);
			}
		}
	}

	public static void SetUnitTestMethodParameter(uint index, uint value) => Stack.Store32(index * 4, value);

	public static void SetUnitTestMethodParameterCount(uint number) => TestParameters = number;

	public static void SetUnitTestMethodAddress(uint address) => TestMethodAddress = address;

	public static void SetUnitTestResultType(uint type) => TestResultType = type;

	public static void StartTest(uint id)
	{
		TestID = id;
		Ready = true;
	}

	public static bool IsReady() => ResultReported && !Ready;

	public static bool QueueUnitTest(uint id, Pointer start, Pointer end)
	{
		var len = (uint)start.GetOffset(end);

		if (QueueNext + len + 32 > Queue + QueueSize)
		{
			if (Queue + len + 32 >= QueueCurrent)
				return false; // no space

			QueueNext.Store32(uint.MaxValue); // mark jump to front

			// cycle to front
			QueueNext = Queue;
		}

		QueueNext.Store32(len + 4);
		QueueNext += 4;

		QueueNext.Store32(id);
		QueueNext += 4;

		for (var i = start; i < end; i += 4)
		{
			uint value = i.Load32();
			QueueNext.Store32(value);
			QueueNext += 4;
		}

		QueueNext.Store32(0); // mark end
		++Count;

		return true;
	}

	public static void ProcessQueue()
	{
		if (QueueNext == QueueCurrent)
			return;

		if (!UnitTestEngine.IsReady())
			return;

		var marker = QueueCurrent.Load32();

		if (marker == uint.MaxValue)
		{
			QueueCurrent = Queue;
		}

		var len = QueueCurrent.Load32();
		var id = QueueCurrent.Load32(4);
		var address = QueueCurrent.Load32(8);
		var type = QueueCurrent.Load32(12);
		var paramcnt = QueueCurrent.Load32(16);

		UnitTestEngine.SetUnitTestMethodAddress(address);
		UnitTestEngine.SetUnitTestResultType(type);
		UnitTestEngine.SetUnitTestMethodParameterCount(paramcnt);

		for (var index = 0u; index < paramcnt; index++)
		{
			var value = QueueCurrent.Load32(20 + index * 4);
			UnitTestEngine.SetUnitTestMethodParameter(index, value);
		}

		QueueCurrent = QueueCurrent + len + 4;
		--Count;

		Screen.Goto(17, 0);
		Screen.ClearRow();
		Screen.Write("[Unit Test]");
		Screen.NextLine();
		Screen.ClearRow();
		Screen.Write("ID: ");
		Screen.Write(id, 10, 5);
		Screen.Write(" Address: ");
		Screen.Write(address, 16, 8);
		Screen.Write(" Param: ");
		Screen.Write(paramcnt, 10, 2);
		Screen.Write(" - Cnt: ");
		Screen.Write(Count, 10, 4);

		UnitTestEngine.StartTest(id);
	}
}
