// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal;
using Mosa.Runtime;
using Mosa.Runtime.x86;

namespace Mosa.UnitTests.Framework;

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
	private static byte TestParameterCount;
	private static byte TestResultType;
	private static Pointer TestMethodAddress;
	private static ulong TestResult;

	private static Pointer Queue;
	private static Pointer QueueNext;
	private static Pointer QueueCurrent;

	private static uint PendingCount;
	private static uint TestCount;

	public static void Setup(ushort comPort)
	{
		ComPort = comPort;

		Serial.Setup(ComPort);

		Buffer = new Pointer(Address.DebuggerBuffer);
		Stack = new Pointer(Address.UnitTestStack);
		Queue = new Pointer(Address.UnitTestQueue);

		Enabled = true;
		ReadySent = false;

		Ready = false;
		ResultReported = true;

		TestID = 0;
		TestParameterCount = 0;
		TestMethodAddress = Pointer.Zero;
		TestResult = 0;

		QueueNext = Queue;
		QueueCurrent = Queue;

		PendingCount = 0;
		TestCount = 0;

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

	public static void Process()
	{
		if (!Enabled)
			return;

		if (!ReadySent)
		{
			SendResponse(0, 0ul);
			ReadySent = true;
		}

		ProcessQueue();

		for (var i = 0; i < 75; i++)
		{
			while (ProcessSerial()) ;
		}

		ProcessQueue();
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
			var length = Buffer.Load8(4);

			if (length > MaxBuffer)
			{
				UsedBuffer = 0;
				return true;
			}

			if (UsedBuffer == length + HeaderSize)
			{
				QueueUnitTest();
				UsedBuffer = 0;
			}
		}

		return true;
	}

	private static void QueueUnitTest()
	{
		var id = Buffer.Load32(0);
		var length = Buffer.Load8(4);

		var start = Buffer + HeaderSize;
		var end = start + length;

		QueueUnitTest(id, start, end);
	}

	public static void EnterTestReadyLoop()
	{
		var stackPointer = new Pointer(Native.AllocateStackSpace(MaxParameters * 4));

		while (true)
		{
			if (Ready)
			{
				TestResult = 0;
				ResultReported = false;
				Ready = false;
				TestCount++;

				DisplayUpdate(true);

				for (var index = 0; index < TestParameterCount; index++)
				{
					var value = Stack.Load32(index * 4);
					stackPointer.Store32(index * 4, value);
				}

				switch (TestResultType)
				{
					case 0: Native.FrameCall(TestMethodAddress.ToUInt32()); break;
					case 1: TestResult = Native.FrameCallRetU4(TestMethodAddress.ToUInt32()); break;
					case 2: TestResult = Native.FrameCallRetU8(TestMethodAddress.ToUInt32()); break;
					case 3: TestResult = Native.FrameCallRetR8(TestMethodAddress.ToUInt32()); break;
					default: break;
				}

				SendResponse(TestID, TestResult);

				ResultReported = true;

				Native.Int(255);
			}
		}
	}

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
		++PendingCount;

		return true;
	}

	public static void ProcessQueue()
	{
		if (QueueNext == QueueCurrent)
			return;

		if (!(ResultReported && !Ready))
			return;

		var marker = QueueCurrent.Load32();

		if (marker == uint.MaxValue)
		{
			QueueCurrent = Queue;
		}

		TestID = QueueCurrent.Load32(4);
		TestMethodAddress = QueueCurrent.LoadPointer(8);    // fix for 64bit
		TestResultType = QueueCurrent.Load8(12);
		TestParameterCount = QueueCurrent.Load8(16);

		for (var index = 0u; index < TestParameterCount; index++)
		{
			var value = QueueCurrent.Load32(20 + index * 4);

			Stack.Store32(index * 4, value);
		}

		var len = QueueCurrent.Load32();

		QueueCurrent = QueueCurrent + len + 4;
		--PendingCount;

		DisplayUpdate();

		Ready = true;
	}

	public static void DisplayUpdate(bool test = false)
	{
		if (!test)
		{
			ScreenConsole.Goto(4, 0);
			ScreenConsole.Write("Total  : ");
			ScreenConsole.WriteValue(TestCount, 7);

			ScreenConsole.Goto(5, 0);
			ScreenConsole.Write("Pending: ");
			ScreenConsole.WriteValue(PendingCount, 7);
		}
		else
		{
			ScreenConsole.Goto(6, 0);
			ScreenConsole.Write("Active : ");
			ScreenConsole.WriteValue(TestID, 7);
			ScreenConsole.Write(" @ ");
			ScreenConsole.WriteValueAsHex(TestMethodAddress.ToUInt32(), 8);
		}
	}
}
