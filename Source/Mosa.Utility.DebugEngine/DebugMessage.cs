// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Utility.DebugEngine
{
	public delegate void SenderMessageDelegate(DebugMessage response);

	public class DebugMessage
	{
		public int ID { get; internal set; }

		public int Code { get; private set; }

		public List<byte> CommandData { get; private set; }

		public List<byte> ResponseData { get; internal set; }

		public object Sender { get; protected set; }

		public SenderMessageDelegate SenderMethod { get; protected set; }

		public int Checksum { get { return 0; } } // TODO

		public DebugMessage(int code, List<byte> data)
		{
			Code = code;
			CommandData = data;
		}

		public DebugMessage(int code, byte[] data)
		{
			Code = code;
			CommandData = new List<byte>(data.Length);

			foreach (byte b in data)
			{
				CommandData.Add(b);
			}
		}

		public DebugMessage(int code, IList<int> data)
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

		public DebugMessage(int code, List<byte> data, object sender, SenderMessageDelegate senderMethod)
			: this(code, data)
		{
			Sender = sender;
			SenderMethod = senderMethod;
		}

		public DebugMessage(int code, List<int> data, object sender, SenderMessageDelegate senderMethod)
			: this(code, data)
		{
			Sender = sender;
			SenderMethod = senderMethod;
		}

		public DebugMessage(int code, int[] data, object sender, SenderMessageDelegate senderMethod)
			: this(code, data)
		{
			Sender = sender;
			SenderMethod = senderMethod;
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
				case DebugCode.SendNumber: return "#: " + ((ResponseData[0] << 24) | (ResponseData[1] << 16) | (ResponseData[2] << 8) | ResponseData[3]).ToString();
				default: return "Code: " + Code.ToString();
			}
		}

		private byte[] CreateByteArray(List<byte> data)
		{
			var array = new byte[data.Count];
			data.CopyTo(array);
			return array;
		}
	}
}
