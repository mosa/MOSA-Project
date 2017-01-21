// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.ClassLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;

namespace Mosa.Utility.RSP
{
	public delegate void CallBack(ReplayPacket replayPacket);

	public sealed class GDBClient
	{
		private byte PacketSeperator = (byte)'$';

		private object sync = new object();
		private Stream stream = null;
		private byte[] receivedData = new byte[1];
		private Queue<byte> packetData = new Queue<byte>();
		private int packetSeperatorCount;
		private Queue<CommandPacket> commandPackets = new Queue<CommandPacket>();
		private Dictionary<CommandPacket, CallBack> callBacks = new Dictionary<CommandPacket, CallBack>();
		private CommandPacket currentCommandPacket = null;

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

		public GDBClient()
		{
		}

		public bool IsConnected
		{
			get
			{
				if (stream == null)
					return false;

				if (stream is NamedPipeClientStream)
					return (stream as NamedPipeClientStream).IsConnected;

				if (stream is GDBNetworkStream)
					return (stream as GDBNetworkStream).IsConnected;

				return false;
			}
		}

		private void AddPacketData(byte data)
		{
			lock (sync)
			{
				packetData.Enqueue(data);

				if (data == PacketSeperator)
				{
					packetSeperatorCount++;
				}
			}

			// if packetSeperatorCount > 0
			// todo
		}

		private void ReadAsyncCallback(IAsyncResult ar)
		{
			try
			{
				stream.EndRead(ar);

				AddPacketData(receivedData[0]);

				stream.BeginRead(receivedData, 0, 1, ReadAsyncCallback, null);
			}
			catch
			{
				// nothing for now
			}
		}

		public void SendCommandAsync(CommandPacket commandPacket, CallBack callBack)
		{
			lock (sync)
			{
				commandPackets.Enqueue(commandPacket);

				if (callBack != null)
				{
					callBacks.Add(commandPacket, callBack);
				}
			}

			// send now if possible
			SendPacket();
		}

		private void SendPacket()
		{
			lock (sync)
			{
				// waiting for current command to reply or wait for command to send
				if (currentCommandPacket != null || commandPackets.Count == 0)
					return;

				var commandPacket = commandPackets.Dequeue();

				SendPacketData(commandPacket);
			}
		}

		private void SendPacketData(CommandPacket commandPacket)
		{
			var data = PacketBinConverter.ToBinary(commandPacket);

			currentCommandPacket = commandPacket;

			stream.Write(data, 0, data.Length);
		}
	}
}
