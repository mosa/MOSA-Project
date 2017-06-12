// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;

namespace Mosa.Utility.RSP
{
	public delegate void CallBack(BaseCommand command);

	public sealed class GDBClient
	{
		private object sync = new object();
		private Stream stream = null;

		private byte[] receivedData = new byte[1];
		private List<byte> receivedPacketData = new List<byte>();

		private Queue<BaseCommand> commandQueue = new Queue<BaseCommand>();

		private BaseCommand currentCommand = null;

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
					SetReadCallBack();
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

		private void SetReadCallBack()
		{
			stream.BeginRead(receivedData, 0, 1, ReadAsyncCallback, null);
		}

		private void ReadAsyncCallback(IAsyncResult ar)
		{
			try
			{
				stream.EndRead(ar);

				var data = receivedData[0];

				lock (sync)
				{
					receivedPacketData.Add(data);

					Console.Write((char)data);

					IncomingPatcket();
				}

				SetReadCallBack();
			}
			catch
			{
				// nothing for now
			}

			// try to send more packets
			SendPackets();
		}

		public void SendCommandAsync(BaseCommand command)
		{
			lock (sync)
			{
				commandQueue.Enqueue(command);
			}

			// try to send more packets
			SendPackets();
		}

		private void SendPackets()
		{
			lock (sync)
			{
				// waiting for current command to reply
				if (currentCommand != null)
					return;

				if (commandQueue.Count == 0)
					return;

				currentCommand = commandQueue.Dequeue();

				var data = PacketBinConverter.ToBinary(currentCommand);
				stream.Write(data, 0, data.Length);
			}
		}

		private void IncomingPatcket()
		{
			int len = receivedPacketData.Count;

			if (len == 0)
				return;

			if (len == 1 && receivedPacketData[0] == '+')
			{
				receivedPacketData.Clear();
				return;
			}

			if (len == 1 && receivedPacketData[0] == '-')
			{
				// todo: re-transmit
				receivedPacketData.Clear();
				return;
			}

			if (len > 4 && receivedPacketData[0] == '$' && receivedPacketData[len - 3] == '#')
			{
				currentCommand.ResponseData = new List<byte>(receivedPacketData);

				currentCommand.Callback?.Invoke(currentCommand);

				receivedPacketData.Clear();
				currentCommand = null;

				return;
			}

			return;
		}
	}
}
