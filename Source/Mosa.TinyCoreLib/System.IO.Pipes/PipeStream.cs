using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace System.IO.Pipes;

public abstract class PipeStream : Stream
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

	public virtual int InBufferSize
	{
		get
		{
			throw null;
		}
	}

	public bool IsAsync
	{
		get
		{
			throw null;
		}
	}

	public bool IsConnected
	{
		get
		{
			throw null;
		}
		protected set
		{
		}
	}

	protected bool IsHandleExposed
	{
		get
		{
			throw null;
		}
	}

	public bool IsMessageComplete
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

	public virtual int OutBufferSize
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

	public virtual PipeTransmissionMode ReadMode
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SafePipeHandle SafePipeHandle
	{
		get
		{
			throw null;
		}
	}

	public virtual PipeTransmissionMode TransmissionMode
	{
		get
		{
			throw null;
		}
	}

	protected PipeStream(PipeDirection direction, int bufferSize)
	{
	}

	protected PipeStream(PipeDirection direction, PipeTransmissionMode transmissionMode, int outBufferSize)
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

	protected internal virtual void CheckPipePropertyOperations()
	{
	}

	protected internal void CheckReadOperations()
	{
	}

	protected internal void CheckWriteOperations()
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

	public override void Flush()
	{
	}

	public override Task FlushAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	protected void InitializeHandle(SafePipeHandle? handle, bool isExposed, bool isAsync)
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

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw null;
	}

	public override void SetLength(long value)
	{
	}

	[SupportedOSPlatform("windows")]
	public void WaitForPipeDrain()
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
