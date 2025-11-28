using System.Threading;
using System.Threading.Tasks;

namespace System.IO;

public class MemoryStream : Stream
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

	public override bool CanWrite
	{
		get
		{
			throw null;
		}
	}

	public virtual int Capacity
	{
		get
		{
			throw null;
		}
		set
		{
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

	public MemoryStream()
	{
	}

	public MemoryStream(byte[] buffer)
	{
	}

	public MemoryStream(byte[] buffer, bool writable)
	{
	}

	public MemoryStream(byte[] buffer, int index, int count)
	{
	}

	public MemoryStream(byte[] buffer, int index, int count, bool writable)
	{
	}

	public MemoryStream(byte[] buffer, int index, int count, bool writable, bool publiclyVisible)
	{
	}

	public MemoryStream(int capacity)
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

	public override void CopyTo(Stream destination, int bufferSize)
	{
	}

	public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
	{
		throw null;
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

	public override void Flush()
	{
	}

	public override Task FlushAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	public virtual byte[] GetBuffer()
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

	public override long Seek(long offset, SeekOrigin loc)
	{
		throw null;
	}

	public override void SetLength(long value)
	{
	}

	public virtual byte[] ToArray()
	{
		throw null;
	}

	public virtual bool TryGetBuffer(out ArraySegment<byte> buffer)
	{
		throw null;
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

	public virtual void WriteTo(Stream stream)
	{
	}
}
