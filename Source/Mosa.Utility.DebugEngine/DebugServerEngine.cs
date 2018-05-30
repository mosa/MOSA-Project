// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;

namespace Mosa.Utility.DebugEngine
{
	public sealed class DebugServerEngine
	{
		private readonly object sync = new object();
		private Stream stream;

		private readonly Dictionary<int, DebugMessage> pending = new Dictionary<int, DebugMessage>();
		private int nextID = 0;

		private List<byte> buffer = new List<byte>();

		private CallBack globalDispatch;
		private readonly byte[] receivedData = new byte[2000];

		private const int MaxBufferSize = (64 * 1024) + 64;

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
					stream.BeginRead(receivedData, 0, receivedData.Length, ReadAsyncCallback, null);
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

				var packet = CreatePacket(message);
				SendPacket(packet);

				return true;
			}
		}

		public bool SendCommand(List<DebugMessage> messages)
		{
			foreach (var message in messages)
			{
				if (!SendCommand(message))
					return false;
			}

			return true;
		}

		private static int packetCnt = 0;

		public bool SendCommand2(List<DebugMessage> messages)
		{
			lock (sync)
			{
				if (!IsConnected)
					return false;

				var packets = new Packet();

				foreach (var message in messages)
				{
					message.ID = ++nextID;
					pending.Add(message.ID, message);

					var packet = CreatePacket(message);

					packets.AppendPacket(packet);
				}

				SendPacket(packets);
				packetCnt += messages.Count;

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

		private void SendPacket(Packet packet)
		{
			var send = packet.Data.ToArray();

			stream.Write(send, 0, send.Length);
		}

		private Packet CreatePacket(DebugMessage message)
		{
			var packet = new Packet();

			packet.Add((byte)'M');
			packet.Add((byte)'O');
			packet.Add((byte)'S');
			packet.Add((byte)'A');

			packet.Add(message.ID);
			packet.Add(message.Code);

			if (message.CommandData == null)
			{
				packet.Add(0);
			}
			else
			{
				packet.Add(message.CommandData.Count); // length

				foreach (var b in message.CommandData)
				{
					packet.Add(b);
				}
			}

			return packet;
		}

		private void PostResponse(int id, int code, List<byte> data)
		{
			DebugMessage message = null;

			lock (sync)
			{
				if (id == 0 || !pending.TryGetValue(id, out message))
				{
					// message without command
					message = new DebugMessage(code, data)
					{
						ID = id
					};

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

			var data = new List<byte>();

			for (int i = 0; i < len; i++)
			{
				data.Add(buffer[i + 16]);
			}

			//Console.WriteLine("ID: " + id + " CODE: " + code + " LEN: " + len);

			PostResponse(id, code, data);

			return true;
		}

		// Message format:	// [0]MAGIC[4]ID[8]CODE[12]LEN[16]DATA[LEN]

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

			if (bad || buffer.Count > MaxBufferSize)
			{
				buffer.Clear();
				return;
			}

			buffer.Add(b);

			if (buffer.Count > 15)
			{
				int length = GetInteger(12);

				if (length > MaxBufferSize)
				{
					buffer.Clear();
					return;
				}

				if (buffer.Count == length + 16)
				{
					ParseResponse();
					buffer.Clear();
				}
			}
		}

		private void ReadAsyncCallback(IAsyncResult ar)
		{
			try
			{
				var bytes = stream.EndRead(ar);

				for (int i = 0; i < bytes; i++)
				{
					Push(receivedData[i]);
				}

				stream.BeginRead(receivedData, 0, receivedData.Length, ReadAsyncCallback, null);
			}
			catch
			{
				// nothing for now
			}
		}
	}
}
