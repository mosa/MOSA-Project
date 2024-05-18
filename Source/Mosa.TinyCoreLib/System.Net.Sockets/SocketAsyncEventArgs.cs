using System.Collections.Generic;

namespace System.Net.Sockets;

public class SocketAsyncEventArgs : EventArgs, IDisposable
{
	public Socket? AcceptSocket
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public byte[]? Buffer
	{
		get
		{
			throw null;
		}
	}

	public IList<ArraySegment<byte>>? BufferList
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int BytesTransferred
	{
		get
		{
			throw null;
		}
	}

	public Exception? ConnectByNameError
	{
		get
		{
			throw null;
		}
	}

	public Socket? ConnectSocket
	{
		get
		{
			throw null;
		}
	}

	public int Count
	{
		get
		{
			throw null;
		}
	}

	public bool DisconnectReuseSocket
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SocketAsyncOperation LastOperation
	{
		get
		{
			throw null;
		}
	}

	public Memory<byte> MemoryBuffer
	{
		get
		{
			throw null;
		}
	}

	public int Offset
	{
		get
		{
			throw null;
		}
	}

	public IPPacketInformation ReceiveMessageFromPacketInfo
	{
		get
		{
			throw null;
		}
	}

	public EndPoint? RemoteEndPoint
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SendPacketsElement[]? SendPacketsElements
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TransmitFileOptions SendPacketsFlags
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int SendPacketsSendSize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SocketError SocketError
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SocketFlags SocketFlags
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public object? UserToken
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public event EventHandler<SocketAsyncEventArgs>? Completed
	{
		add
		{
		}
		remove
		{
		}
	}

	public SocketAsyncEventArgs()
	{
	}

	public SocketAsyncEventArgs(bool unsafeSuppressExecutionContextFlow)
	{
	}

	public void Dispose()
	{
	}

	~SocketAsyncEventArgs()
	{
	}

	protected virtual void OnCompleted(SocketAsyncEventArgs e)
	{
	}

	public void SetBuffer(byte[]? buffer, int offset, int count)
	{
	}

	public void SetBuffer(int offset, int count)
	{
	}

	public void SetBuffer(Memory<byte> buffer)
	{
	}
}
