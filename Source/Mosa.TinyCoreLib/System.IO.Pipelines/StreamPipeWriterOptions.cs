using System.Buffers;

namespace System.IO.Pipelines;

public class StreamPipeWriterOptions
{
	public bool LeaveOpen
	{
		get
		{
			throw null;
		}
	}

	public int MinimumBufferSize
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

	public StreamPipeWriterOptions(MemoryPool<byte>? pool = null, int minimumBufferSize = -1, bool leaveOpen = false)
	{
	}
}
