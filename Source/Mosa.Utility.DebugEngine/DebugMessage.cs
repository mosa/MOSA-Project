﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Utility.DebugEngine
{
	public delegate void CallBack(DebugMessage response);

	public class DebugMessage
	{
		public int ID { get; internal set; }

		public byte Code { get; }

		public IList<byte> CommandData { get; }

		public List<byte> ResponseData { get; internal set; }

		public CallBack CallBack { get; set; }

		public object Other { get; set; }

		public DebugMessage(byte code, IList<byte> data)
		{
			Code = code;
			CommandData = data;
		}

		public DebugMessage(byte code, byte[] data)
		{
			Code = code;
			CommandData = new List<byte>(data.Length);

			foreach (byte b in data)
			{
				CommandData.Add(b);
			}
		}

		public DebugMessage(byte code, byte[] data, int length)
		{
			Code = code;
			CommandData = new List<byte>(data.Length);

			for (int i = 0; i < length; i++)
			{
				CommandData.Add(data[i]);
			}
		}

		public DebugMessage(byte code, IList<int> data)
		{
			Code = code;
			CommandData = new List<byte>(data.Count * 4);

			foreach (int i in data)
			{
				CommandData.Add((byte)(i & 0xFF));
				CommandData.Add((byte)((i >> 8) & 0xFF));
				CommandData.Add((byte)((i >> 16) & 0xFF));
				CommandData.Add((byte)((i >> 24) & 0xFF));
			}
		}

		public DebugMessage(byte code, IList<int> data, object other)
			: this(code, data)
		{
			Other = other;
		}

		public DebugMessage(byte code, IList<int> data, CallBack callback)
			: this(code, data)
		{
			CallBack = callback;
		}

		public DebugMessage(byte code, IList<byte> data, CallBack callback)
		: this(code, data)
		{
			CallBack = callback;
		}

		public DebugMessage(byte code, int[] data, CallBack callback)
			: this(code, data)
		{
			CallBack = callback;
		}

		public int GetInt32(int index)
		{
			return (ResponseData[index + 3] << 24) | (ResponseData[index + 2] << 16) | (ResponseData[index + 1] << 8) | ResponseData[index];
		}

		public uint GetUInt32(int index)
		{
			return (uint)GetInt32(index);
		}

		public override string ToString()
		{
			switch (Code)
			{
				case DebugCode.Connected: return "Connected";
				case DebugCode.Connecting: return "Connecting";
				case DebugCode.Disconnected: return "Disconnected";
				case DebugCode.UnknownData: return "Unknown Data: " + System.Text.Encoding.UTF8.GetString(CreateByteArray(ResponseData));
				case DebugCode.InformationalMessage: return "Informational Message: " + System.Text.Encoding.UTF8.GetString(CreateByteArray(ResponseData));
				case DebugCode.ErrorMessage: return "Error Message: " + System.Text.Encoding.UTF8.GetString(CreateByteArray(ResponseData));
				case DebugCode.WarningMessage: return "Warning Message: " + System.Text.Encoding.UTF8.GetString(CreateByteArray(ResponseData));
				case DebugCode.Ping: return "Ping ACK";
				case DebugCode.Alive: return "Alive";
				case DebugCode.ReadCR3: return "ReadCR3";
				case DebugCode.ReadMemory: return "ReadMemory";
				case DebugCode.Scattered32BitReadMemory: return "Scattered32BitReadMemory";
				case DebugCode.WriteMemory: return "WriteMemory";
				case DebugCode.CompressedWriteMemory: return "CompressedWriteMemory";
				case DebugCode.SendNumber: return "#: " + ((ResponseData[0] << 24) | (ResponseData[1] << 16) | (ResponseData[2] << 8) | ResponseData[3]).ToString();
				default: return "Code: " + Code.ToString();
			}
		}

		private static byte[] CreateByteArray(List<byte> data)
		{
			var array = new byte[data.Count];
			data.CopyTo(array);
			return array;
		}
	}
}
