// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text;

namespace Mosa.Utility.RSP
{
	public delegate void CallBack(BaseCommand command);

	public sealed class GDBClient
	{
		private static readonly Encoding TextEncoding = Encoding.ASCII;

		private const char Prefix = '$';
		private const char Suffix = '#';

		private object sync = new object();
		private Stream stream = null;

		private byte[] receivedByte = new byte[1];
		private List<byte> receivedData = new List<byte>();

		private Queue<BaseCommand> commandQueue = new Queue<BaseCommand>();

		private BaseCommand currentCommand = null;

		private static byte[] breakData = new byte[1] { 3 };

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
			stream.BeginRead(receivedByte, 0, 1, ReadAsyncCallback, null);
		}

		private void ReadAsyncCallback(IAsyncResult ar)
		{
			try
			{
				stream.EndRead(ar);

				var data = receivedByte[0];

				lock (sync)
				{
					receivedData.Add(data);

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

		public void SendBreak()
		{
			lock (sync)
			{
				stream.Write(breakData, 0, 1);
			}
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

				Console.WriteLine("SENT: " + currentCommand.Pack);

				var data = ToBinary(currentCommand);
				stream.Write(data, 0, data.Length);
			}
		}

		private void IncomingPatcket()
		{
			int len = receivedData.Count;

			if (len == 0)
				return;

			if (len == 1 && receivedData[0] == '+')
			{
				receivedData.Clear();
				return;
			}

			if (len == 1 && receivedData[0] == '-')
			{
				// todo: re-transmit
				receivedData.Clear();
				return;
			}

			if (len >= 4 && receivedData[0] == '$' && receivedData[len - 3] == '#')
			{
				Console.WriteLine();

				var data = Rle.Decode(receivedData, 1, receivedData.Count - 3).ToArray();

				bool ok = false;

				if (data != null)
				{
					// Compare checksum
					byte receivedChecksum = HexToDecimal(receivedData[len - 2], receivedData[len - 1]);
					uint calculatedChecksum = Checksum.Calculate(data);

					if (receivedChecksum == calculatedChecksum)
					{
						ok = true;
					}
				}

				currentCommand.IsResponseOk = ok;
				currentCommand.ResponseData = data;

				if (ok)
				{
					currentCommand.Decode();
				}

				currentCommand.Callback?.Invoke(currentCommand);

				receivedData.Clear();
				currentCommand = null;

				return;
			}

			return;
		}

		private static byte[] ToBinary(BaseCommand packet)
		{
			var binPacket = new StringBuilder();
			binPacket.Append(Prefix);
			binPacket.Append(packet.Pack);
			binPacket.Append(Suffix);
			binPacket.Append(Checksum.Calculate(TextEncoding.GetBytes(packet.Pack)).ToString("x2"));

			return TextEncoding.GetBytes(binPacket.ToString());
		}

		private static byte HexToDecimal(byte v)
		{
			if (v >= '0' && v <= '9')
				return (byte)(v - (byte)'0');
			else
				return (byte)(v - (byte)'a' + 10);
		}

		public static byte HexToDecimal(byte h, byte l)
		{
			h = HexToDecimal(h);
			l = HexToDecimal(l);

			return (byte)(h * 16 + l);
		}
	}
}
