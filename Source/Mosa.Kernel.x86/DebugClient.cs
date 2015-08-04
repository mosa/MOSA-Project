// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.Internal.x86;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// Client Side Debugger
	/// </summary>
	public static class DebugClient
	{
		#region Codes

		public static class Codes
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
		}

		#endregion Codes

		private static bool enabled = false;

		private static ushort com = Serial.COM1;

		private static uint buffer = 0x1412000;
		private static uint index = 0;
		private static int length = -1;

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
			SendMagic();
			SendInteger(id);
			SendInteger(code);
			SendInteger(0);
			SendInteger(0); // TODO: not implemented
		}

		public static void SendResponse(int id, int code, int data)
		{
			SendMagic();
			SendInteger(id);
			SendInteger(code);
			SendInteger(4);
			SendInteger(0); // TODO: not implemented
			SendInteger(data);
		}

		public static void SendResponse(int id, int code, int len, int magic)
		{
			SendMagic();
			SendInteger(id);
			SendInteger(code);
			SendInteger(len);
			SendInteger(magic);
		}

		public static void SendAlive()
		{
			SendResponse(0, Codes.Alive);
		}

		public static void SendNumber(int i)
		{
			SendResponse(0, Codes.SendNumber, i);
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

		public static void Process()
		{
			if (!enabled)
				return;

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
				length = (int)GetInt32(12);
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
				case Codes.Ping: SendResponse(id, Codes.Ping); return;
				case Codes.ReadMemory: ReadMemory(); return;
				case Codes.ReadCR3: SendResponse(id, Codes.ReadCR3, (int)Native.GetCR3()); return;
				case Codes.Scattered32BitReadMemory: Scattered32BitReadMemory(); return;
				case Codes.WriteMemory: WriteMemory(); return;
				default: return;
			}
		}

		private static void ReadMemory()
		{
			int id = GetInt32(4);
			uint start = (uint)GetInt32(20);
			uint bytes = (uint)GetInt32(24);

			SendResponse(id, Codes.ReadMemory, (int)(bytes + 8), 0);

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

			SendResponse(id, Codes.Scattered32BitReadMemory, (int)(count * 8), 0);

			for (uint i = 0; i < count; i++)
			{
				uint address = GetUInt32((uint)((i * 4) + 20));
				SendInteger(address);
				SendInteger(Native.Get32(address));
			}
		}

		private static void WriteMemory()
		{
			int id = GetInt32(4);
			uint start = GetUInt32(20);
			uint bytes = GetUInt32(24);

			SendResponse(id, Codes.WriteMemory);

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
	}
}
