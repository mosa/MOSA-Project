// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;

namespace Mosa.Utility.DebugEngine
{
	public sealed class DebugServerEngine
	{
		private object sync = new object();
		private Stream stream;

		private Dictionary<int, DebugMessage> pending = new Dictionary<int, DebugMessage>();
		private int nextID = 0;

		private List<byte> buffer = new List<byte>();

		private CallBack globalDispatch;
		private byte[] receivedData = new byte[1];

		private int length = -1;

		private const int MaxBufferSize = 0x20000;

		public Stream Stream
		{
			get
			{
				return stream;
			}
			set
			{
				stream = value;

				if (IsConnected)
				{
					stream.BeginRead(receivedData, 0, 1, ReadAsyncCallback, null);
				}
			}
		}

		public DebugServerEngine()
		{
			stream = null;
		}

		public void SetGlobalDispatch(CallBack dispatch)
		{
			globalDispatch = dispatch;
		}

		public bool SendCommand(DebugMessage message)
		{
			lock (sync)
			{
				if (!IsConnected)
					return false;

				message.ID = ++nextID;
				pending.Add(message.ID, message);
				SendCommandMessage(message);

				return true;
			}
		}

		public bool IsConnected
		{
			get
			{
				if (stream == null)
					return false;

				if (stream is NamedPipeClientStream)
					return (stream as NamedPipeClientStream).IsConnected;

				if (stream is DebugNetworkStream)
					return (stream as DebugNetworkStream).IsConnected;

				return false;
			}
		}

		private void SendByte(int b)
		{
			stream.WriteByte((byte)b);
		}

		private void SendInteger(int i)
		{
			SendByte(i & 0xFF);
			SendByte(i >> 8 & 0xFF);
			SendByte(i >> 16 & 0xFF);
			SendByte(i >> 24 & 0xFF);
		}

		private void SendMagic()
		{
			SendByte('M');
			SendByte('O');
			SendByte('S');
			SendByte('A');
		}

		private void SendCommandMessage(DebugMessage message)
		{
			SendMagic();
			SendInteger(message.ID);
			SendInteger(message.Code);
			if (message.CommandData == null)
			{
				SendInteger(0);
				SendInteger(message.Checksum);
			}
			else
			{
				SendInteger(message.CommandData.Count);
				SendInteger(message.Checksum);
				foreach (var b in message.CommandData)
					SendByte(b);
			}
		}

		private void PostResponse(int id, int code, List<byte> data)
		{
			DebugMessage message = null;

			lock (sync)
			{
				if (id == 0 || !pending.TryGetValue(id, out message))
				{
					// message without command
					message = new DebugMessage(code, data);
					message.ID = id;

					// need to set a default notifier for this
				}
				else
				{
					pending.Remove(message.ID);
				}

				message.ResponseData = data;
			}

			if (message != null)
			{
				globalDispatch?.Invoke(message);

				message.CallBack?.Invoke(message);
			}
		}

		private int GetInteger(int index)
		{
			return (buffer[index + 3] << 24) | (buffer[index + 2] << 16) | (buffer[index + 1] << 8) | buffer[index];
		}

		private bool ParseResponse()
		{
			int id = GetInteger(4);
			int code = GetInteger(8);
			int len = GetInteger(12);
			int checksum = GetInteger(16);

			var data = new List<byte>();
			for (int i = 0; i < len; i++)
				data.Add(buffer[i + 20]);

			PostResponse(id, code, data);

			return true;
		}

		// Message format:	[MAGIC]-ID-CODE-LEN-CHECKSUM-DATA

		private void BadDataAbort()
		{
			var data = buffer;

			buffer = new List<byte>();

			PostResponse(0, DebugCode.UnknownData, data);

			length = -1;
		}

		private void Push(byte b)
		{
			bool bad = false;

			if (buffer.Count == 0 && b != (byte)'M')
				bad = true;
			else if (buffer.Count == 1 && b != (byte)'O')
				bad = true;
			else if (buffer.Count == 2 && b != (byte)'S')
				bad = true;
			else if (buffer.Count == 3 && b != (byte)'A')
				bad = true;

			if (bad)
			{
				BadDataAbort();
				return;
			}

			buffer.Add(b);

			if (buffer.Count >= 16 && length == -1)
			{
				length = GetInteger(12);
			}

			if (length > MaxBufferSize || buffer.Count > MaxBufferSize)
			{
				BadDataAbort();
				return;
			}

			if (length + 20 == buffer.Count)
			{
				bool valid = ParseResponse();
				buffer.Clear();
				length = -1;
			}
		}

		private void ReadAsyncCallback(IAsyncResult ar)
		{
			try
			{
				stream.EndRead(ar);

				Push(receivedData[0]);

				stream.BeginRead(receivedData, 0, 1, ReadAsyncCallback, null);
			}
			catch
			{
				// nothing for now
			}
		}
	}
}
