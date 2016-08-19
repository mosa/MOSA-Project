// Copyright (c) MOSA Project. Licensed under the New BSD License.

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

			public const int CompressedWriteMemory = 1021;

			public const int SoftReset = 1111;

			public const int ExecuteUnitTest = 2000;
			public const int AbortUnitTest = 2001;
		}

		#endregion Codes

		private const int MaxBuffer = (1024 * 64) + 64;

		private static bool enabled = false;

		private static ushort com = Serial.COM1;

		private static uint index = 0;
		private static int length = -1;

		private static uint last = 0;
		private static uint sendchecksum = 0;

		private static bool ready = false;
		private static bool readysent = false;

		private static uint current_irq = 0;

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
			sendchecksum = sendchecksum ^ b;
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

		private static void SendCheckSum()
		{
			SendRawByte((byte)(sendchecksum & 0xFF));
			SendRawByte((byte)(sendchecksum >> 8 & 0xFF));
			SendRawByte((byte)(sendchecksum >> 16 & 0xFF));
			SendRawByte((byte)(sendchecksum >> 24 & 0xFF));
		}

		// MAGIC-ID-CODE-LEN-DATA-CHECKSUM

		private static void SendResponseStart(int id, int code, int len)
		{
			// Magic
			SendByte('M');
			SendByte('O');
			SendByte('S');
			SendByte('A');

			// Clear checksum
			sendchecksum = 0;

			// ID
			SendInteger(id);

			// Code
			SendInteger(code);

			// Length
			SendInteger(len);
		}

		private static void SendResponse(int id, int code)
		{
			SendResponseStart(id, code, 0);
			SendCheckSum();
		}

		private static void SendResponse(int id, int code, int data)
		{
			SendResponseStart(id, code, 4);
			SendInteger(data);
			SendCheckSum();
		}

		private static void SendResponse(int id, int code, ulong data)
		{
			SendResponseStart(id, code, 8);
			SendInteger(data);
			SendCheckSum();
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
			length = -1;
		}

		private static byte GetByte(uint offset)
		{
			return Native.Get8(Address.DebuggerBuffer + offset);
		}

		private static int GetInt32(uint offset)
		{
			return (Native.Get8(Address.DebuggerBuffer + offset + 3) << 24) | (Native.Get8(Address.DebuggerBuffer + offset + 2) << 16) | (Native.Get8(Address.DebuggerBuffer + offset + 1) << 8) | Native.Get8(Address.DebuggerBuffer + offset + 0);
		}

		private static uint GetUInt32(uint offset)
		{
			return (uint)GetInt32(offset);
		}

		private static int GetMagic()
		{
			return GetInt32(0);
		}

		private static int GetID()
		{
			return GetInt32(4);
		}

		private static int GetCode()
		{
			return GetInt32(8);
		}

		private static uint GetLength()
		{
			return GetUInt32(12);
		}

		private static int GetCheckSum()
		{
			return GetInt32(16 + GetLength());
		}

		internal static void Process(uint irq)
		{
			if (!enabled)
				return;

			current_irq = irq;

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

			while (ProcessSerial()) ;

			if (ready)
			{
				ProcessTestUnitQueue();
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

			Native.Set8(Address.DebuggerBuffer + index, b);
			index++;

			if (index >= 16 && length == -1)
			{
				length = GetInt32(12);
			}

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

			return true;
		}

		private static void ProcessCommand()
		{
			// [0]MAGIC[4]ID[8]CODE[12]LEN[16]DATA[LEN]CHECKSUM

			// TODO: validate checksum
			//int checksum = GetCheckSum();

			switch (GetCode())
			{
				case DebugCode.Ping: SendResponse(GetID(), DebugCode.Ping); return;
				case DebugCode.ReadMemory: ReadMemory(); return;
				case DebugCode.ReadCR3: SendResponse(GetID(), DebugCode.ReadCR3, (int)Native.GetCR3()); return;
				case DebugCode.Scattered32BitReadMemory: Scattered32BitReadMemory(); return;
				case DebugCode.WriteMemory: WriteMemory(); return;
				case DebugCode.CompressedWriteMemory: CompressedWriteMemory(); return;
				case DebugCode.ClearMemory: ClearMemory(); return;
				case DebugCode.SoftReset: SoftReset(); return;
				case DebugCode.ExecuteUnitTest: QueueUnitTest(); return;

				default: return;
			}
		}

		private static void ReadMemory()
		{
			int id = GetID();
			uint start = (uint)GetInt32(16);
			uint bytes = (uint)GetInt32(20);

			SendResponseStart(id, DebugCode.ReadMemory, (int)(bytes + 8));

			SendInteger(start); // starting address
			SendInteger(bytes); // bytes

			for (uint i = 0; i < bytes; i++)
			{
				SendByte((Native.Get8(start + i)));
			}

			SendCheckSum();
		}

		private static void Scattered32BitReadMemory()
		{
			int id = GetID();
			int count = GetInt32(12) / 4;

			SendResponseStart(id, DebugCode.Scattered32BitReadMemory, count * 8);

			for (uint i = 0; i < count; i++)
			{
				uint address = GetUInt32((i * 4) + 16);
				SendInteger(address);
				SendInteger(Native.Get32(address));
			}

			SendCheckSum();
		}

		private static void WriteMemory()
		{
			int id = GetID();
			uint start = GetUInt32(16);
			uint bytes = GetUInt32(20);

			SendResponse(id, DebugCode.WriteMemory);

			uint at = 0;

			while (at + 4 < bytes)
			{
				uint value = GetUInt32(24 + at);

				Native.Set32(start + at, value);

				at = at + 4;
			}

			while (at < bytes)
			{
				byte value = GetByte(24 + at);

				Native.Set8(start + at, value);

				at = at + 1;
			}
		}

		private static void CompressedWriteMemory()
		{
			int id = GetID();
			uint start = GetUInt32(16);
			uint size = GetUInt32(20);

			//uint unsize = GetUInt32(24);

			SendResponse(id, DebugCode.CompressedWriteMemory);

			LZF.Decompress(Address.DebuggerBuffer + 24, size, start, 1024 * 32); // next more than 32Kb
		}

		private static void ClearMemory()
		{
			int id = GetID();
			uint start = GetUInt32(16);
			uint bytes = GetUInt32(20);

			uint at = 0;

			while (at + 4 < bytes)
			{
				Native.Set32(start + at, 0);

				at = at + 4;
			}

			while (at < bytes)
			{
				Native.Set8(start + at, 0);

				at = at + 1;
			}

			SendResponse(id, DebugCode.ClearMemory);
		}

		private static void QueueUnitTest()
		{
			int id = GetID();

			var start = Address.DebuggerBuffer + 16;
			uint end = start + (uint)length;

			UnitTestQueue.QueueUnitTest(id, start, end);
		}

		private static void SendTestUnitResponse()
		{
			ulong result;
			int id;

			if (UnitTestRunner.GetResult(out result, out id))
			{
				SendResponse(id, DebugCode.ExecuteUnitTest, result);
			}
		}

		private static void ProcessTestUnitQueue()
		{
			UnitTestQueue.ProcessQueue();
		}

		private static void SoftReset()
		{
			int id = GetID();
			uint address = GetUInt32(16);

			SendResponse(id, DebugCode.SoftReset);

			// Setup stack

			// IRET will pop in this order: EIP, CS, and EFLAGS registers
			Native.Set32(Address.InitialStack - 8, address);    // EIP
			Native.Set32(Address.InitialStack - 4, 0);          // CS
			Native.Set32(Address.InitialStack + 0, 0);          // EFLAGS

			// clear the interrupt
			if (current_irq != 0)
				PIC.SendEndOfInterrupt(current_irq);

			Native.FrameIRet(Address.InitialStack + 8, Address.InitialStack);
		}
	}
}
