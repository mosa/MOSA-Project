// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.x86;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// Client Side Debugger
	/// </summary>
	public static class DebugClient
	{
		#region Codes

		public static class DebugCode
		{
			public const int Connecting = 10;
			public const int Connected = 11;
			public const int Disconnected = 12;

			public const int UnknownData = 99;

			public const int Alive = 1000;
			public const int Ping = 1001;
			public const int InformationalMessage = 1002;
			public const int WarningMessage = 1003;
			public const int ErrorMessage = 1004;
			public const int SendNumber = 1005;
			public const int ReadMemory = 1010;
			public const int WriteMemory = 1011;
			public const int ReadCR3 = 1012;
			public const int Scattered32BitReadMemory = 1013;

			public const int StartUnitTest = 2000;
			public const int SetUnitTestMethodAddress = 2001;
			public const int SetUnitTestParameter = 2002;
			public const int SetUnitTestID = 2003;
			public const int AbortUnitTest = 2004;
		}

		#endregion Codes

		private static bool enabled = false;

		private static ushort com = Serial.COM1;

		private static uint buffer = 0x04000;
		private static uint index = 0;
		private static int length = -1;

		private static uint last = 0;
		private static bool busy = false;

		public static void Setup(ushort com)
		{
			enabled = true;
			Serial.SetupPort(com);
			DebugClient.com = com;
		}

		private static void SendByte(int i)
		{
			Serial.Write(com, (byte)i);
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

		private static void SendMagic()
		{
			SendByte('M');
			SendByte('O');
			SendByte('S');
			SendByte('A');
		}

		// MAGIC-ID-CODE-LEN-CHECKSUM-DATA

		public static void SendResponse(int id, int code)
		{
			busy = true;
			SendMagic();
			SendInteger(id);
			SendInteger(code);
			SendInteger(0);
			SendInteger(0); // TODO: not implemented
			busy = false;
		}

		public static void SendResponse(int id, int code, int data)
		{
			busy = true;
			SendMagic();
			SendInteger(id);
			SendInteger(code);
			SendInteger(4);
			SendInteger(0); // TODO: not implemented
			SendInteger(data);
			busy = false;
		}

		public static void SendResponse(int id, int code, int len, int magic)
		{
			busy = true;
			SendMagic();
			SendInteger(id);
			SendInteger(code);
			SendInteger(len);
			SendInteger(magic);
			busy = false;
		}

		public static void SendAlive()
		{
			busy = true;
			SendResponse(0, DebugCode.Alive);
			busy = false;
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
			return Native.Get8(buffer + offset);
		}

		private static int GetInt32(uint offset)
		{
			return (Native.Get8(buffer + offset + 3) << 24) | (Native.Get8(buffer + offset + 2) << 16) | (Native.Get8(buffer + offset + 1) << 8) | Native.Get8(buffer + offset + 0);
		}

		private static uint GetUInt32(uint offset)
		{
			return (uint)GetInt32(offset);
		}

		public static void Process(uint interrupt)
		{
			if (!enabled)
				return;

			if (!busy)
			{
				byte second = CMOS.Second;

				if (second % 10 != 5 & last != second)
				{
					last = CMOS.Second;
					SendAlive();
				}
			}

			if (!Serial.IsDataReady(com))
				return;

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
				return;
			}

			Native.Set8(buffer + index, b);
			index++;

			if (index >= 16 && length == -1)
			{
				length = GetInt32(12);
			}

			if (length > 4096 || index > 4096)
			{
				BadDataAbort();
				return;
			}

			if (length + 20 == index)
			{
				ProcessCommand();

				ResetBuffer();
			}
		}

		private static void ProcessCommand()
		{
			int id = GetInt32(4);
			int code = GetInt32(8);
			int len = GetInt32(12);
			int checksum = GetInt32(16);

			// TODO: validate checksum

			switch (code)
			{
				case DebugCode.Ping: SendResponse(id, DebugCode.Ping); return;
				case DebugCode.ReadMemory: ReadMemory(); return;
				case DebugCode.ReadCR3: SendResponse(id, DebugCode.ReadCR3, (int)Native.GetCR3()); return;
				case DebugCode.Scattered32BitReadMemory: Scattered32BitReadMemory(); return;
				case DebugCode.WriteMemory: WriteMemory(); return;

				case DebugCode.StartUnitTest: StartUnitTest(); return;
				case DebugCode.SetUnitTestMethodAddress: SetUnitTestMethodAddress(); return;
				case DebugCode.SetUnitTestParameter: SetUnitTestParameter(); return;
				case DebugCode.SetUnitTestID: SetUnitTestID(); return;
				case DebugCode.AbortUnitTest: AbortUnitTest(); return;

				default: return;
			}
		}

		private static void ReadMemory()
		{
			int id = GetInt32(4);
			uint start = (uint)GetInt32(20);
			uint bytes = (uint)GetInt32(24);

			SendResponse(id, DebugCode.ReadMemory, (int)(bytes + 8), 0);

			SendInteger(start); // starting address
			SendInteger(bytes); // bytes

			for (uint i = 0; i < bytes; i++)
			{
				SendByte((Native.Get8(start + i)));
			}
		}

		private static void Scattered32BitReadMemory()
		{
			int id = GetInt32(4);
			int count = GetInt32(12) / 4;

			SendResponse(id, DebugCode.Scattered32BitReadMemory, count * 8, 0);

			for (uint i = 0; i < count; i++)
			{
				uint address = GetUInt32((i * 4) + 20);
				SendInteger(address);
				SendInteger(Native.Get32(address));
			}
		}

		private static void WriteMemory()
		{
			int id = GetInt32(4);
			uint start = GetUInt32(20);
			uint bytes = GetUInt32(24);

			SendResponse(id, DebugCode.WriteMemory);

			uint at = 0;

			while (at + 4 < bytes)
			{
				uint value = GetUInt32(28 + at);

				Native.Set32(start + at, value);

				at = at + 4;
			}

			while (at < bytes)
			{
				byte value = GetByte(28 + at);

				Native.Set8(start + at, value);

				at = at + 1;
			}
		}

		private static void StartUnitTest()
		{
			UnitTestRunner.StartTest();
		}

		private static void SetUnitTestMethodAddress()
		{
			int id = GetInt32(4);
			uint address = GetUInt32(20);

			UnitTestRunner.SetUnitTestMethodAddress(address);

			SendResponse(id, DebugCode.SetUnitTestMethodAddress);
		}

		private static void SetUnitTestParameter()
		{
			int id = GetInt32(4);
			uint index = GetUInt32(20);
			uint value = GetUInt32(24);

			UnitTestRunner.SetUnitTestParameter(index, value);

			SendResponse(id, DebugCode.SetUnitTestParameter);
		}

		private static void SetUnitTestID()
		{
			int id = GetInt32(4);
			uint testid = GetUInt32(20);

			UnitTestRunner.SetUnitTestID(testid);

			SendResponse(id, DebugCode.SetUnitTestID);
		}

		private static void AbortUnitTest()
		{
			int id = GetInt32(4);

			UnitTestRunner.AbortUnitTest();

			SendResponse(id, DebugCode.AbortUnitTest);
		}
	}
}
