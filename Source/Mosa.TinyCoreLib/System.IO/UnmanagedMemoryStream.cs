using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO;

public class UnmanagedMemoryStream : Stream
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

	public long Capacity
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

	[CLSCompliant(false)]
	public unsafe byte* PositionPointer
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected UnmanagedMemoryStream()
	{
	}

	[CLSCompliant(false)]
	public unsafe UnmanagedMemoryStream(byte* pointer, long length)
	{
	}

	[CLSCompliant(false)]
	public unsafe UnmanagedMemoryStream(byte* pointer, long length, long capacity, FileAccess access)
	{
	}

	public UnmanagedMemoryStream(SafeBuffer buffer, long offset, long length)
	{
	}

	public UnmanagedMemoryStream(SafeBuffer buffer, long offset, long length, FileAccess access)
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	public override void Flush()
	{
	}

	public override Task FlushAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	[CLSCompliant(false)]
	protected unsafe void Initialize(byte* pointer, long length, long capacity, FileAccess access)
	{
	}

	protected void Initialize(SafeBuffer buffer, long offset, long length, FileAccess access)
	{
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
