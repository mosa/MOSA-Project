/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text;

namespace Mosa.Utility.DebugEngine
{
	public sealed class DebugEngine
	{
		private object sync = new object();
		private int lastID = 0;

		private Queue<DebugMessage> commands = new Queue<DebugMessage>();
		private Dictionary<int, DebugMessage> pending = new Dictionary<int, DebugMessage>();

		private NamedPipeClientStream pipeClient;

		//public DebugMessage SendCommand(int code, byte[] data)
		//{
		//    DebugMessage message = new DebugMessage(code, data);
		//    return SendCommand(message);
		//}

		public DebugMessage SendCommand(DebugMessage message)
		{
			lock (sync)
			{
				message.ID = ++lastID;
				pending.Add(message.ID, message);
				commands.Enqueue(message);
				return message;
			}

			ProcessCommandQueue(); // HACK
		}

		private void SendByte(int b)
		{
			pipeClient.WriteByte((byte)b);
		}

		private void SendInteger(int i)
		{
			SendByte(i >> 24 & 0xFF);
			SendByte(i >> 16 & 0xFF);
			SendByte(i >> 8 & 0xFF);
			SendByte(i & 0xFF);
		}

		private void SendMagic()
		{
			SendByte('M');
			SendByte('O');
			SendByte('S');
			SendByte('A');
		}

		public void ProcessCommandQueue()
		{
			while (true)
			{
				lock (sync)
				{
					if (!pipeClient.IsConnected)
						return;

					if (commands.Count == 0)
						return;

					DebugMessage message = commands.Dequeue();

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
						SendInteger(message.CommandData.Length);
						SendInteger(message.Checksum);
						foreach (var b in message.CommandData)
							SendByte(b);
					}

				}
			}
		}

		private void PostResponse(int id, int code, byte[] data)
		{
			lock (sync)
			{
				DebugMessage message;

				if (id == 0 || !pending.TryGetValue(id, out message))
				{
					// message without command
					message = new DebugMessage(code, data);
					message.ID = ++lastID;

					// need to set a default notifier for this
				}
				else
				{
					pending.Remove(message.ID);
				}

				message.ResponseData = data;
				message.NotifySender();
			}
		}

		private int GetInteger(int index)
		{
			return (buffer[index] << 24) | (buffer[index + 1] << 16) | (buffer[index + 2] << 8) | buffer[index + 3];
		}

		private bool ParseResponse()
		{
			int id = GetInteger(4);
			int code = GetInteger(8);
			int len = GetInteger(12);
			int checksum = GetInteger(16);

			byte[] data = new byte[len];
			for (int i = 0; i < len; i++)
				data[i] = buffer[i + 20];

			PostResponse(id, code, data);

			return true;
		}

		// Message format:	[MAGIC]-ID-CODE-LEN-CHECKSUM-DATA

		byte[] buffer = new byte[4096 + 5 * 4 + 1];
		int index = 0;
		int length = -1;

		private void BadDataAbort()
		{
			byte[] data = new byte[index];
			for (int i = 0; i < index; i++)
				data[i] = buffer[i];

			PostResponse(0, Codes.UnknownData, data);

			ResetBuffer();
		}

		private void ResetBuffer()
		{
			index = 0;
			length = -1;
		}

		private void Push(byte b)
		{
			buffer[index++] = b;

			if (index == 1 && buffer[0] != (byte)'M')
				BadDataAbort();
			else if (index == 2 && buffer[1] != (byte)'O')
				BadDataAbort();
			else if (index == 3 && buffer[2] != (byte)'S')
				BadDataAbort();
			else if (index == 4 && buffer[3] != (byte)'A')
				BadDataAbort();

			if (index >= 16 && length == -1)
			{
				length = GetInteger(12);
			}

			if (length > 4096 || index > 4096)
			{
				BadDataAbort();
				return;
			}

			if (length + 20 == index)
			{
				bool valid = ParseResponse();
				ResetBuffer();
			}
		}

		private void ReceiveLoop()
		{
			PostResponse(0, Codes.Connected, null);

			while (pipeClient.IsConnected)
			{
				int b = pipeClient.ReadByte();

				if (b < 0)
					break;

				Push((byte)b);

				ProcessCommandQueue(); // HACK
			}

			PostResponse(0, Codes.Disconnected, null);
		}

		public void ThreadStart()
		{
			while (true)
			{
				using (pipeClient = new NamedPipeClientStream(".", @"MOSA", PipeDirection.InOut))
				{
					PostResponse(0, Codes.Connecting, null);
					pipeClient.Connect();

					ReceiveLoop();
				}
			}
		}

	}
}
