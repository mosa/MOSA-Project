using System.Threading;
using System.Threading.Tasks;

namespace System.IO;

public sealed class BufferedStream : Stream
{
	public int BufferSize
	{
		get
		{
			throw null;
		}
	}

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

	public Stream UnderlyingStream
	{
		get
		{
			throw null;
		}
	}

	public BufferedStream(Stream stream)
	{
	}

	public BufferedStream(Stream stream, int bufferSize)
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

	public override ValueTask DisposeAsync()
	{
		throw null;
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

	public override int Read(byte[] buffer, int offset, int count)
	{
		throw null;
	}

	public override int Read(Span<byte> destination)
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
