// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;

namespace Mosa.Utility.UnitTests.DebugEngine;

public sealed class DebugServerEngine
{
	private Stream stream;

	private readonly Dictionary<int, DebugMessage> pending = new Dictionary<int, DebugMessage>();
	private int nextID;

	private readonly List<byte> ReceiveBuffer = new List<byte>();
	private readonly List<byte> SendBuffer = new List<byte>(2048);

	private readonly byte[] ReceivedData = new byte[2000];

	private CallBack globalDispatch;

	public Stream Stream
	{
		get => stream;
		set
		{
			stream = value;

			if (IsConnected)
			{
				stream.BeginRead(ReceivedData, 0, ReceivedData.Length, ReadAsyncCallback, null);
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
				ReceiveBuffer.Clear();
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

	public void Send(List<DebugMessage> messages)
	{
		if (messages.Count == 0)
			return;

		lock (this)
		{
			if (!IsConnected)
				return;

			SendBuffer.Clear();

			foreach (var message in messages)
			{
				message.ID = ++nextID;

				Send(message.ID);
				Send(message.CommandData.Count);

				foreach (var b in message.CommandData)
				{
					Send(b);
				}

				pending.Add(message.ID, message);
			}

			stream.Write(SendBuffer.ToArray());
		}
	}

	private void Send(byte b)
	{
		SendBuffer.Add(b);
	}

	private void Send(int i)
	{
		Send((byte)(i & 0xFF));
		Send((byte)(i >> 8 & 0xFF));
		Send((byte)(i >> 16 & 0xFF));
		Send((byte)(i >> 24 & 0xFF));
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

	private void PostResponse(int id, List<byte> data)
	{
		DebugMessage message = null;

		lock (this)
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
		return (ReceiveBuffer[index + 3] << 24) | (ReceiveBuffer[index + 2] << 16) | (ReceiveBuffer[index + 1] << 8) | ReceiveBuffer[index];
	}

	private bool ParseResponse()
	{
		var id = GetInteger(0);
		var len = GetInteger(4);

		var data = new List<byte>();

		for (var i = 0; i < len; i++)
		{
			data.Add(ReceiveBuffer[i + 8]);
		}

		PostResponse(id, data);

		return true;
	}

	private void Push(byte b)
	{
		ReceiveBuffer.Add(b);

		if (ReceiveBuffer.Count >= 8)
		{
			var length = GetInteger(4);

			if (ReceiveBuffer.Count == length + 8)
			{
				ParseResponse();
				ReceiveBuffer.Clear();
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
				Push(ReceivedData[i]);
			}

			stream.BeginRead(ReceivedData, 0, ReceivedData.Length, ReadAsyncCallback, null);
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
