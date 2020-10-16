// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.x86;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// Client Side Debugger
	/// </summary>
	public static class Debugger
	{
		#region Codes

		public static class DebugCode
		{
			public const byte Connecting = 10;
			public const byte Connected = 11;
			public const byte Disconnected = 12;

			public const byte UnknownData = 99;

			public const byte Alive = 100;
			public const byte Ready = 101;
			public const byte Ping = 102;
			public const byte InformationalMessage = 103;
			public const byte WarningMessage = 104;
			public const byte ErrorMessage = 105;
			public const byte SendNumber = 106;
			public const byte ReadMemory = 110;
			public const byte WriteMemory = 111;
			public const byte ReadCR3 = 112;
			public const byte Scattered32BitReadMemory = 113;
			public const byte ClearMemory = 114;
			public const byte GetMemoryCRC = 115;
			public const byte CompressedWriteMemory = 121;

			public const byte HardJump = 211;

			public const byte ExecuteUnitTest = 200;
			public const byte AbortUnitTest = 201;
		}

		#endregion Codes

		private const int MaxBuffer = (1024 * 64) + 64;

		private static bool enabled = false;

		private static ushort com = Serial.COM1;

		private static uint index = 0;

		private static uint last = 0;

		private static bool ready = false;
		private static bool readysent = false;

		private unsafe static IDTStack* idt_stack;

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

		private static void SendPointer32(Pointer pointer)
		{
			SendInteger(pointer.ToInt32());
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

		private static void SendResponse(uint id, byte code, int data)
		{
			SendResponseStart(id, code, 4);
			SendInteger(data);
		}

		private static void SendResponse(uint id, byte code, ulong data)
		{
			SendResponseStart(id, code, 8);
			SendInteger(data);
		}

		private static void SendAlive()
		{
			SendResponse(0, DebugCode.Alive);
		}

		private static void SendReady()
		{
			SendResponse(0, DebugCode.Ready);
		}

		private static byte GetByte(uint offset)
		{
			return new Pointer(Address.DebuggerBuffer).Load8(offset);
		}

		private static uint GetUInt32(uint offset)
		{
			return new Pointer(Address.DebuggerBuffer).Load32(offset);
		}

		private static byte GetDataByte(uint offset)
		{
			return new Pointer(Address.DebuggerBuffer).Load8(HeaderSize + offset);
		}

		private static uint GetDataUInt32(uint offset)
		{
			return new Pointer(Address.DebuggerBuffer).Load32(HeaderSize + offset);
		}

		private static Pointer GetDataPointer(uint offset)
		{
			return new Pointer(Address.DebuggerBuffer).LoadPointer(HeaderSize + offset);
		}

		private static uint GetID()
		{
			return GetUInt32(1);
		}

		private static byte GetCode()
		{
			return GetByte(5);
		}

		private static uint GetLength()
		{
			return GetUInt32(6);
		}

		internal unsafe static void Process(IDTStack* stack)
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

			byte second = CMOS.Second;

			if (second % 10 != 5 & last != second)
			{
				last = CMOS.Second;
				SendAlive();
			}

			if (ready & !readysent)
			{
				readysent = true;
				SendReady();
			}

			for (int x = 0; x < 5; x++)
			{
				for (int i = 0; i < 75; i++)
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

			int code = GetCode();
			uint id = GetID();
			uint len = GetLength();

			Screen.Goto(13, 0);
			Screen.ClearRow();
			Screen.Write("[Data]");
			Screen.NextLine();
			Screen.ClearRow();
			Screen.Write("ID: ");
			Screen.Write(id, 10, 5);
			Screen.Write(" Code: ");
			Screen.Write((uint)code, 10, 4);
			Screen.Write(" Len: ");
			Screen.Write(len, 10, 5);

			switch (code)
			{
				case DebugCode.Ping: SendResponse(GetID(), DebugCode.Ping); return;
				case DebugCode.ReadMemory: ReadMemory(); return;
				case DebugCode.ReadCR3: SendResponse(GetID(), DebugCode.ReadCR3, (int)Native.GetCR3()); return;
				case DebugCode.Scattered32BitReadMemory: Scattered32BitReadMemory(); return;
				case DebugCode.WriteMemory: WriteMemory(); return;
				case DebugCode.CompressedWriteMemory: CompressedWriteMemory(); return;
				case DebugCode.ClearMemory: ClearMemory(); return;
				case DebugCode.HardJump: HardJump(); return;
				case DebugCode.ExecuteUnitTest: QueueUnitTest(); return;
				default: return;
			}
		}

		private static void ReadMemory()
		{
			uint id = GetID();
			var start = GetDataPointer(0);
			uint bytes = GetDataUInt32(4);

			SendResponseStart(id, DebugCode.ReadMemory, bytes + 8);

			SendPointer32(start); // starting address
			SendInteger(bytes); // bytes

			for (uint i = 0; i < bytes; i++)
			{
				SendByte(Intrinsic.Load8(start, i));
			}
		}

		private static void Scattered32BitReadMemory()
		{
			uint id = GetID();
			uint count = GetLength() / 4;

			SendResponseStart(id, DebugCode.Scattered32BitReadMemory, count * 8);

			for (uint i = 0; i < count; i++)
			{
				var address = GetDataPointer(i * 4);
				SendPointer32(address);
				SendInteger(address.Load32());
			}
		}

		private static void WriteMemory()
		{
			uint id = GetID();
			var address = GetDataPointer(0);
			uint length = GetDataUInt32(4);

			SendResponse(id, DebugCode.WriteMemory);

			uint at = 0;

			while (at + 4 < length)
			{
				uint value = GetDataUInt32(8 + at);

				address.Store32(at, value);

				at += 4;
			}

			while (at < length)
			{
				byte value = GetDataByte(8 + at);

				address.Store8(at, value);

				at++;
			}

			Screen.Goto(15, 0);
			Screen.ClearRow();
			Screen.Write("[WriteMemory]");
			Screen.NextLine();
			Screen.ClearRow();
			Screen.Write("ID: ");
			Screen.Write(id, 10, 5);
			Screen.Write(" Address: ");
			Screen.Write((uint)address.ToInt32(), 16, 8);
			Screen.Write(" Len: ");
			Screen.Write(length, 10, 5);
		}

		private static void CompressedWriteMemory()
		{
			uint id = GetID();
			uint address = GetDataUInt32(0);
			uint length = GetDataUInt32(4);
			uint size = GetDataUInt32(8);

			//uint uncompresscrc = GetDataUInt32(12);

			LZF.Decompress(new Pointer(Address.DebuggerBuffer + HeaderSize), length, new Pointer(address), size);

			Screen.Goto(15, 0);
			Screen.ClearRow();
			Screen.Write("[CompressedWriteMemory]");
			Screen.NextLine();
			Screen.ClearRow();
			Screen.Write("ID: ");
			Screen.Write(id, 10, 5);
			Screen.Write(" Address: ");
			Screen.Write(address, 16, 8);
			Screen.Write(" Len: ");
			Screen.Write(length, 10, 5);
			Screen.Write(" Size: ");
			Screen.Write(size, 10, 5);
			Screen.Write(" CRC: ");

			//Screen.Write(uncompresscrc, 16, 8);

			//if (uncompresscrc == computedcrc)
			//	Screen.Write(" OK");
			//else
			//	Screen.Write(" BAD");

			SendResponse(id, DebugCode.CompressedWriteMemory);
		}

		private static void ClearMemory()
		{
			uint id = GetID();
			var start = GetDataPointer(0);
			uint bytes = GetDataUInt32(4);

			uint at = 0;

			while (at + 4 < bytes)
			{
				start.Store32(at, 0);

				at += 4;
			}

			while (at < bytes)
			{
				start.Store8(at, 0);

				at++;
			}

			SendResponse(id, DebugCode.ClearMemory);
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

		private unsafe static void HardJump()
		{
			uint id = GetID();
			uint address = GetDataUInt32(0);

			SendResponse(id, DebugCode.HardJump);

			idt_stack->EIP = address;

			Screen.Goto(15, 0);
			Screen.ClearRow();
			Screen.Write("[HardJump]");
			Screen.NextLine();
			Screen.ClearRow();
			Screen.Write("ID: ");
			Screen.Write(id, 10, 5);
			Screen.Write(" Address: ");
			Screen.Write(address, 16, 8);
		}
	}
}
