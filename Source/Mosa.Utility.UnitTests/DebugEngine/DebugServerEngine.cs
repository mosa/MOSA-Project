// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;

namespace Mosa.Utility.UnitTests.DebugEngine;

public delegate void CallBack(int id, ulong value);

public sealed class DebugServerEngine
{
	private Stream stream;

	private readonly List<byte> SendBuffer = new List<byte>(2048);

	private readonly byte[] ReceivedData = new byte[2000];

	private readonly byte[] Received = new byte[12];
	private int ReceivedLen = 0;

	private CallBack Dispatch;

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
		ReceivedLen = 0;
	}

	public void Disconnect()
	{
		if (stream != null)
		{
			try
			{
				stream.Close();
			}
			finally
			{
				stream = null;
			}
		}
	}

	public void SetDispatch(CallBack dispatch)
	{
		Dispatch = dispatch;
	}

	public void Send(List<UnitTest> unittests)
	{
		if (unittests.Count == 0)
			return;

		lock (this)
		{
			if (!IsConnected)
				return;

			SendBuffer.Clear();

			foreach (var unittest in unittests)
			{
				Send(unittest.UnitTestID);
				Send((byte)(unittest.SerializedUnitTest.Count * 4));

				foreach (var b in unittest.SerializedUnitTest)
				{
					Send(b);
				}
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

	private void Push(byte b)
	{
		Received[ReceivedLen++] = b;

		if (ReceivedLen == 12)
		{
			var id = (Received[3] << 24) | (Received[2] << 16) | (Received[1] << 8) | Received[0];
			var data = 0ul;

			for (var i = 7; i >= 0; i--)
			{
				data = (data << 8) | Received[i + 4];
			}

			ReceivedLen = 0;

			Dispatch.Invoke(id, data);
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
