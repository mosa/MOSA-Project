using System.Buffers;

namespace System.IO.Pipelines;

public class StreamPipeReaderOptions
{
	public int BufferSize
	{
		get
		{
			throw null;
		}
	}

	public bool LeaveOpen
	{
		get
		{
			throw null;
		}
	}

	public int MinimumReadSize
	{
		get
		{
			throw null;
		}
	}

	public MemoryPool<byte> Pool
	{
		get
		{
			throw null;
		}
	}

	public bool UseZeroByteReads
	{
		get
		{
			throw null;
		}
	}

	public StreamPipeReaderOptions(MemoryPool<byte>? pool, int bufferSize, int minimumReadSize, bool leaveOpen)
	{
	}

	public StreamPipeReaderOptions(MemoryPool<byte>? pool = null, int bufferSize = -1, int minimumReadSize = -1, bool leaveOpen = false, bool useZeroByteReads = false)
	{
	}
}
