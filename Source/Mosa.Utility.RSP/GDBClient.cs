// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.RSP.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Mosa.Utility.RSP
{
	public delegate void CallBack(GDBCommand command);

	public sealed class GDBClient
	{
		private static readonly Encoding TextEncoding = Encoding.ASCII;

		private const char Prefix = '$';
		private const char Suffix = '#';

		private readonly object sync = new object();
		private GDBNetworkStream stream = null;

		private readonly byte[] receivedByte = new byte[1];
		private readonly List<byte> receivedData = new List<byte>();

		private readonly Queue<GDBCommand> commandQueue = new Queue<GDBCommand>();

		private GDBCommand currentCommand = null;

		private static readonly byte[] breakData = new byte[1] { 3 };

		public GDBNetworkStream Stream
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

		public GDBClient(GDBNetworkStream stream = null)
		{
			Stream = stream;
		}

		public bool IsConnected
		{
			get
			{
				if (stream == null)
					return false;

				return stream.IsConnected;
			}
		}

		private void SetReadCallBack()
		{
			if (stream.IsConnected)
			{
				stream.BeginRead(receivedByte, 0, 1, ReadAsyncCallback, null);
			}
		}

		private void ReadAsyncCallback(IAsyncResult ar)
		{
			lock (sync)
			{
				try
				{
					stream.EndRead(ar);

					var data = receivedByte[0];

					receivedData.Add(data);

					//Debug.Write((char)data);

					IncomingPatcket();
				}
				catch (Exception e)
				{
					// nothing for now
					Debug.WriteLine(e.ToString());
				}
				finally
				{
					SetReadCallBack();

					// try to send more packets
				}
			}

			SendPackets();
		}

		public void SendCommand(GDBCommand command)
		{
			lock (sync)
			{
				commandQueue.Enqueue(command);
			}

			// try to send packets
			SendPackets();
		}

		public void SendBreak()
		{
			lock (sync)
			{
				commandQueue.Clear();

				//Debug.WriteLine("SENT: BREAK");

				currentCommand = new GetReasonHalted();
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

				//Debug.WriteLine("SENT: " + currentCommand.Pack);

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
				//Debug.WriteLine("RECEIVED: " + Encoding.UTF8.GetString(receivedData.ToArray()));

				if (currentCommand == null)
				{
					receivedData.Clear();
					return;
				}

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

				var cmd = currentCommand;
				currentCommand = null;
				receivedData.Clear();

				if (cmd.Callback != null)
				{
					ThreadPool.QueueUserWorkItem(state => { cmd.Callback.Invoke(cmd); });
				}

				return;
			}

			return;
		}

		private static byte[] ToBinary(GDBCommand packet)
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

			return (byte)((h * 16) + l);
		}
	}
}
