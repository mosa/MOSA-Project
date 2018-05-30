// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.ClassLib;
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
			public const int Connecting = 10;
			public const int Connected = 11;
			public const int Disconnected = 12;

			public const int UnknownData = 99;

			public const int Alive = 1000;
			public const int Ready = 1001;
			public const int Ping = 1002;
			public const int InformationalMessage = 1003;
			public const int WarningMessage = 1004;
			public const int ErrorMessage = 1005;
			public const int SendNumber = 1006;
			public const int ReadMemory = 1010;
			public const int WriteMemory = 1011;
			public const int ReadCR3 = 1012;
			public const int Scattered32BitReadMemory = 1013;
			public const int ClearMemory = 1014;
			public const int GetMemoryCRC = 1015;

			public const int CompressedWriteMemory = 1021;

			public const int HardJump = 1111;

			public const int ExecuteUnitTest = 2000;
			public const int AbortUnitTest = 2001;
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

		private static void SendInteger(ulong i)
		{
			SendInteger((uint)(i & 0xFFFFFFFF));
			SendInteger((uint)((i >> 32) & 0xFFFFFFFF));
		}

		// MAGIC-ID-CODE-LEN-DATA-CHECKSUM

		private static void SendResponseStart(uint id, int code, uint len)
		{
			// Magic
			SendByte('M');
			SendByte('O');
			SendByte('S');
			SendByte('A');

			// ID
			SendInteger(id);

			// Code
			SendInteger(code);

			// Length
			SendInteger(len);
		}

		private static void SendResponse(uint id, int code)
		{
			SendResponseStart(id, code, 0);
		}

		private static void SendResponse(uint id, int code, int data)
		{
			SendResponseStart(id, code, 4);
			SendInteger(data);
		}

		private static void SendResponse(uint id, int code, ulong data)
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

		private static void BadDataAbort()
		{
			ResetBuffer();
		}

		private static void ResetBuffer()
		{
			index = 0;
		}

		private static byte GetByte(uint offset)
		{
			return Intrinsic.Load8(Address.DebuggerBuffer, offset);
		}

		private static int GetInt32(uint offset)
		{
			return (Intrinsic.Load8(Address.DebuggerBuffer, offset + 3) << 24) | (Intrinsic.Load8(Address.DebuggerBuffer, offset + 2) << 16) | (Intrinsic.Load8(Address.DebuggerBuffer, offset + 1) << 8) | Intrinsic.Load8(Address.DebuggerBuffer, offset + 0);
		}

		private static uint GetUInt32(uint offset)
		{
			return (uint)GetInt32(offset);
		}

		private static int GetMagic()
		{
			return GetInt32(0);
		}

		private static uint GetID()
		{
			return GetUInt32(4);
		}

		private static int GetCode()
		{
			return GetInt32(8);
		}

		private static uint GetLength()
		{
			return GetUInt32(12);
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

			for (int x = 0; x < 25; x++)
			{
				for (int i = 0; i < 100; i++)
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

			bool bad = false;

			if (index == 0 && b != (byte)'M')
				bad = true;
			else if (index == 1 && b != (byte)'O')
				bad = true;
			else if (index == 2 && b != (byte)'S')
				bad = true;
			else if (index == 3 && b != (byte)'A')
				bad = true;

			if (bad)
			{
				BadDataAbort();
				return true;
			}

			Intrinsic.Store8(Address.DebuggerBuffer, index, b);
			index++;

			uint length = 0;

			if (index >= 16)
			{
				length = GetLength();

				if (length > MaxBuffer || index > MaxBuffer)
				{
					BadDataAbort();
					return true;
				}

				if (length + 20 == index)
				{
					ProcessCommand();
					ResetBuffer();
				}
			}

			return true;
		}

		private static void ProcessCommand()
		{
			// [0]MAGIC[4]ID[8]CODE[12]LEN[16]DATA[LEN]CHECKSUM

			int code = GetCode();
			uint id = GetID();
			uint len = GetLength();

			Screen.Goto(13, 0);
			Screen.ClearRow();
			Screen.Write("[Data]");
			Screen.NextLine();
			Screen.ClearRow();
			Screen.Write("ID: ");
			Screen.Write((uint)id, 10, 5);
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
				case DebugCode.GetMemoryCRC: GetMemoryCRC(); return;
				default: return;
			}
		}

		private static void ReadMemory()
		{
			uint id = GetID();
			uint start = (uint)GetInt32(16);
			uint bytes = (uint)GetInt32(20);

			SendResponseStart(id, DebugCode.ReadMemory, bytes + 8);

			SendInteger(start); // starting address
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
				uint address = GetUInt32((i * 4) + 16);
				SendInteger(address);
				SendInteger(Intrinsic.Load32(address));
			}
		}

		private static void WriteMemory()
		{
			uint id = GetID();
			uint address = GetUInt32(16);
			uint length = GetUInt32(20);

			SendResponse(id, DebugCode.WriteMemory);

			uint at = 0;

			while (at + 4 < length)
			{
				uint value = GetUInt32(24 + at);

				Intrinsic.Store32(address, at, value);

				at += 4;
			}

			while (at < length)
			{
				byte value = GetByte(24 + at);

				Intrinsic.Store8(address, at, value);

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
			Screen.Write(address, 16, 8);
			Screen.Write(" Len: ");
			Screen.Write(length, 10, 5);
		}

		private static void CompressedWriteMemory()
		{
			uint id = GetID();
			uint address = GetUInt32(16);
			uint length = GetUInt32(20);
			uint size = GetUInt32(24);
			uint uncompresscrc = GetUInt32(28);

			LZF.Decompress(Address.DebuggerBuffer + 32, length, address, size);

			uint computedcrc = ComputeMemoryCRC(address, size);

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
			Screen.Write(uncompresscrc, 16, 8);

			if (uncompresscrc == computedcrc)
				Screen.Write(" OK");
			else
				Screen.Write(" BAD");

			SendResponse(id, DebugCode.CompressedWriteMemory);
		}

		private static void ClearMemory()
		{
			uint id = GetID();
			uint start = GetUInt32(16);
			uint bytes = GetUInt32(20);

			uint at = 0;

			while (at + 4 < bytes)
			{
				Intrinsic.Store32(start, at, 0);

				at = at + 4;
			}

			while (at < bytes)
			{
				Intrinsic.Store8(start, at, 0);

				at = at + 1;
			}

			SendResponse(id, DebugCode.ClearMemory);
		}

		private static void GetMemoryCRC()
		{
			uint id = GetID();
			uint start = GetUInt32(16);
			uint length = GetUInt32(20);

			uint crc = ComputeMemoryCRC(start, length);

			SendResponseStart(id, DebugCode.GetMemoryCRC, 4);
			SendInteger(crc);
		}

		private static uint ComputeMemoryCRC(uint start, uint length)
		{
			uint crc = CRC.InitialCRC;

			for (uint i = start; i < start + length; i++)
			{
				byte b = Intrinsic.Load8(i);
				crc = CRC.Update(crc, b);
			}

			return crc;
		}

		private static void QueueUnitTest()
		{
			uint id = GetID();

			var start = Address.DebuggerBuffer + 16;
			uint end = start + GetLength() + 16;

			UnitTestQueue.QueueUnitTest(id, start, end);
		}

		private static void SendTestUnitResponse()
		{
			ulong result;
			uint id;

			if (UnitTestRunner.GetResult(out result, out id))
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
			uint address = GetUInt32(16);

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
