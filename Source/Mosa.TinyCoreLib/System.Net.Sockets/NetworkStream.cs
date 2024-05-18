using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Sockets;

public class NetworkStream : Stream
{
	public override bool CanRead
	{
		get
		{
			throw null;
		}
	}

	public override bool CanSeek
	{
		get
		{
			throw null;
		}
	}

	public override bool CanTimeout
	{
		get
		{
			throw null;
		}
	}

	public override bool CanWrite
	{
		get
		{
			throw null;
		}
	}

	public virtual bool DataAvailable
	{
		get
		{
			throw null;
		}
	}

	public override long Length
	{
		get
		{
			throw null;
		}
	}

	public override long Position
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected bool Readable
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override int ReadTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Socket Socket
	{
		get
		{
			throw null;
		}
	}

	protected bool Writeable
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override int WriteTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public NetworkStream(Socket socket)
	{
	}

	public NetworkStream(Socket socket, bool ownsSocket)
	{
	}

	public NetworkStream(Socket socket, FileAccess access)
	{
	}

	public NetworkStream(Socket socket, FileAccess access, bool ownsSocket)
	{
	}

	public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public void Close(int timeout)
	{
	}

	public void Close(TimeSpan timeout)
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	public override int EndRead(IAsyncResult asyncResult)
	{
		throw null;
	}

	public override void EndWrite(IAsyncResult asyncResult)
	{
	}

	~NetworkStream()
	{
	}

	public override void Flush()
	{
	}

	public override Task FlushAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		throw null;
	}

	public override int Read(Span<byte> buffer)
	{
		throw null;
	}

	public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		throw null;
	}

	public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public override int ReadByte()
	{
		throw null;
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw null;
	}

	public override void SetLength(long value)
	{
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
	}

	public override void Write(ReadOnlySpan<byte> buffer)
	{
	}

	public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		throw null;
	}

	public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public override void WriteByte(byte value)
	{
	}
}
