// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Net;
using System.Net.Sockets;
using System.Text;
using Mosa.Compiler.Common;

namespace Mosa.Utility.Launcher;

public class SimpleTCP
{
	public delegate void OnDataAvailableHandler();

	public delegate void OnStatusUpdateHandler(string status);

	#region Public Properties

	public OnDataAvailableHandler OnDataAvailable { get; set; }

	public OnStatusUpdateHandler OnStatusUpdate { get; set; }

	public bool IsConnected => tcpClient is { Connected: true };

	public bool HasData
	{
		get
		{
			lock (receivedData)
				return receivedData.Count > 0;
		}
	}

	public bool HasLine
	{
		get
		{
			lock (receivedData)
				return lines > 0;
		}
	}

	#endregion Public Properties

	#region Private Members

	private const byte NewLine = (byte)'\n';

	private TcpListener tcpListener;
	private TcpClient tcpClient;
	private NetworkStream stream;

	private readonly byte[] receiveBuffer = new byte[4096];
	private readonly Queue<byte> receivedData = new Queue<byte>();

	private static readonly byte[] Empty = Array.Empty<byte>();

	private volatile int lines;

	#endregion Private Members

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

			tcpClient = new TcpClient(hostname, port)
			{
				NoDelay = true
			};

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
		catch (Exception)
		{
			tcpClient = null;
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

	public void Send(byte[] data) => stream.Write(data);

	public void Send(string s)
	{
		foreach (var c in s)
			stream.WriteByte((byte)c);
	}

	public void Send(char c) => stream.WriteByte((byte)c);

	public int GetByte()
	{
		lock (receivedData)
		{
			if (receivedData.Count == 0)
				return -1;

			var b = receivedData.Dequeue();
			if (b == NewLine)
				Interlocked.Decrement(ref lines);

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
					Interlocked.Decrement(ref lines);
			}

			return bytes;
		}
	}

	public string GetFullLine() => Encoding.Default.GetString(GetBytes());

	public string GetLine()
	{
		var sb = new StringBuilder();

		while (true)
		{
			var b = GetByte();
			if (b is < 0 or NewLine)
				break;

			sb.Append((char)b);
		}

		return sb.ToString();
	}

	#endregion Public Methods

	#region Private Methods

	private void Output(string status) => OnStatusUpdate?.Invoke(status);

	private void TriggerOnData() => OnDataAvailable?.Invoke();

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
						Interlocked.Increment(ref lines);
				}
			}
		}
		catch (Exception)
		{
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
