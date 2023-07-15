// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using Mosa.Compiler.Framework.IR;
using Reko.Core.Hll.Pascal;

namespace Mosa.Utility.UnitTests.DebugEngine;

public sealed class DebugServerEngine
{
	private readonly object sync = new object();
	private Stream stream;

	private readonly Dictionary<int, DebugMessage> pending = new Dictionary<int, DebugMessage>();
	private int nextID;

	private readonly List<byte> buffer = new List<byte>();

	private CallBack globalDispatch;
	private readonly byte[] receivedData = new byte[2000];

	public Stream Stream
	{
		get => stream;
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

	public void Disconnect()
	{
		if (stream != null)
		{
			try
			{
				buffer.Clear();
				stream.Close();
			}
			finally
			{
				stream = null;
			}
		}
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

	public bool SendCommand2(List<DebugMessage> messages)
	{
		if (messages.Count == 0)
			return true;

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

		packet.Add(message.ID);

		if (message.CommandData == null)
		{
			packet.Add(0);  // length
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

	private void PostResponse(int id, List<byte> data)
	{
		DebugMessage message = null;

		lock (sync)
		{
			if (id == 0 || !pending.TryGetValue(id, out message))
			{
				// message without command
				message = new DebugMessage(data)
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
		var id = GetInteger(0);
		var len = GetInteger(4);

		var data = new List<byte>();

		for (var i = 0; i < len; i++)
		{
			data.Add(buffer[i + 8]);
		}

		PostResponse(id, data);

		return true;
	}

	private void Push(byte b)
	{
		buffer.Add(b);

		if (buffer.Count >= 8)
		{
			var id = GetInteger(0);
			var length = GetInteger(4);

			//Console.WriteLine($"Buffer: {buffer.Count} - Len: {length} - Byte: {b}");

			if (buffer.Count == length + 8)
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
			if (stream == null)
				return;

			var bytes = stream.EndRead(ar);

			if (bytes == 0)
			{
				stream = null;
				return;
			}

			for (var i = 0; i < bytes; i++)
			{
				Push(receivedData[i]);
			}

			stream.BeginRead(receivedData, 0, receivedData.Length, ReadAsyncCallback, null);
		}
		catch (IOException)
		{
			stream = null;
		}
		catch
		{
			// nothing for now
		}
	}
}
