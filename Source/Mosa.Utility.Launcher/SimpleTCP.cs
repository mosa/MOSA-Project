// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Mosa.Compiler.Common;

namespace Mosa.Utility.Launcher;

public class SimpleTCP
{
	public delegate void OnDataAvailableHandler();

	public delegate void OnStatusUpdateHandler(string status);

	#region Private Members

	private const byte NewLine = (byte)'\n';

	private TcpListener tcpListener;
	private TcpClient tcpClient;
	private NetworkStream stream;

	private readonly byte[] receiveBuffer = new byte[4096];
	private readonly Queue<byte> receivedData = new Queue<byte>();

	private static readonly byte[] Empty = new byte[0];

	private volatile int lines = 0;

	#endregion Private Members

	#region Public Properties

	public OnDataAvailableHandler OnDataAvailable { get; set; }

	public OnStatusUpdateHandler OnStatusUpdate { get; set; }

	public bool IsConnected
	{
		get
		{
			if (tcpClient == null)
				return false;

			return tcpClient.Connected;
		}
	}

	public bool HasData
	{
		get
		{
			lock (receivedData)
			{
				return receivedData.Count > 0;
			}
		}
	}

	public bool HasLine
	{
		get
		{
			lock (receivedData)
			{
				return lines > 0;
			}
		}
	}

	#endregion Public Properties

	#region Public Methods

	public bool Connect(string hostname, ushort port, int timeOutMilliseconds, int retryMilliseconds = 100)
	{
		var watchDog = new WatchDog(timeOutMilliseconds);

		while (!watchDog.IsTimedOut)
		{
			if (Connect(hostname, port))
				return true;

			Thread.Sleep(retryMilliseconds);
		}

		Output("Timeout!");
		return false;
	}

	public bool Connect(string hostname, ushort port)
	{
		try
		{
			Output("Connecting...");

			tcpClient = new TcpClient(hostname, port);
			tcpClient.NoDelay = true;

			stream = tcpClient.GetStream();
			SetReadCallBack();
		}
		catch (SocketException)
		{
			return false;
		}

		Output("Connected!");
		return true;
	}

	public bool Listen(ushort port)
	{
		try
		{
			Output("Listening...");

			var ipAddress = Dns.GetHostEntry("localhost").AddressList[0];

			tcpListener = new TcpListener(ipAddress, port);
			tcpListener.Start();
		}
		catch (SocketException)
		{
			//Output($"Exception: {e}");
			return false;
		}

		return true;
	}

	public bool WaitForConnection(int timeOutMilliseconds, int retryMilliseconds = 100)
	{
		Output("Waiting for connection...");

		var watchDog = new WatchDog(timeOutMilliseconds);

		while (!watchDog.IsTimedOut)
		{
			if (WaitForConnection())
				return true;

			Thread.Sleep(retryMilliseconds);
		}

		Output("Timeout!");
		return false;
	}

	public bool WaitForConnection()
	{
		try
		{
			tcpClient = tcpListener.AcceptTcpClient();
			tcpClient.NoDelay = true;

			stream = tcpClient.GetStream();
			SetReadCallBack();
		}
		catch (Exception e)
		{
			tcpClient = null;
			//Output($"Exception: {e}");

			return false;
		}

		Output("Connected!");
		return true;
	}

	public void Disconnect()
	{
		if (tcpClient != null)
		{
			tcpClient.Close();
			tcpClient = null;
		}

		if (tcpListener != null)
		{
			tcpListener.Stop();
			tcpListener = null;
		}
	}

	public void Send(byte[] data)
	{
		stream.Write(data);
	}

	public void Send(string s)
	{
		foreach (var c in s)
			stream.WriteByte((byte)c);
	}

	public void Send(char c)
	{
		stream.WriteByte((byte)c);
	}

	public int GetByte()
	{
		lock (receivedData)
		{
			if (receivedData.Count == 0)
				return -1;

			var b = receivedData.Dequeue();

			if (b == NewLine)
				lines--;

			return b;
		}
	}

	public byte[] GetBytes()
	{
		lock (receivedData)
		{
			var count = receivedData.Count;

			if (count == 0)
				return Empty;

			var bytes = new byte[count];

			for (var i = 0; i < count; i++)
			{
				var b = receivedData.Dequeue();

				bytes[i] = b;

				if (b == NewLine)
					lines--;
			}

			return bytes;
		}
	}

	public string GetFullLine()
	{
		var bytes = GetBytes();

		return Encoding.Default.GetString(bytes);
	}

	public string GetLine()
	{
		var sb = new StringBuilder();

		while (true)
		{
			var b = GetByte();

			if (b < 0)
				break;

			if (b == NewLine)
				break;

			sb.Append((char)b);
		}

		return sb.ToString();
	}

	#endregion Public Methods

	#region Private Methods

	private void Output(string status)
	{
		if (OnStatusUpdate == null)
			return;

		OnStatusUpdate(status);
	}

	private void TriggerOnData()
	{
		if (OnDataAvailable == null)
			return;

		OnDataAvailable();
	}

	private void SetReadCallBack()
	{
		if (!IsConnected)
			return;

		stream.BeginRead(receiveBuffer, 0, receiveBuffer.Length, ReadAsyncCallback, null);
	}

	private void ReadAsyncCallback(IAsyncResult ar)
	{
		try
		{
			lock (receivedData)
			{
				var bytes = stream.EndRead(ar);

				for (var i = 0; i < bytes; i++)
				{
					var b = receiveBuffer[i];
					receivedData.Enqueue(b);

					if (b == NewLine)
						lines++;
				}
			}
		}
		catch (Exception e)
		{
			//Output($"Exception: {e}");
			Disconnect();
		}
		finally
		{
			if (IsConnected)
			{
				TriggerOnData();
				SetReadCallBack();
			}
		}
	}

	#endregion Private Methods
}
