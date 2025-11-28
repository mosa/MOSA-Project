using System.Buffers;

namespace System.IO.Pipelines;

public class PipeOptions
{
	public static PipeOptions Default
	{
		get
		{
			throw null;
		}
	}

	public int MinimumSegmentSize
	{
		get
		{
			throw null;
		}
	}

	public long PauseWriterThreshold
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

	public PipeScheduler ReaderScheduler
	{
		get
		{
			throw null;
		}
	}

	public long ResumeWriterThreshold
	{
		get
		{
			throw null;
		}
	}

	public bool UseSynchronizationContext
	{
		get
		{
			throw null;
		}
	}

	public PipeScheduler WriterScheduler
	{
		get
		{
			throw null;
		}
	}

	public PipeOptions(MemoryPool<byte>? pool = null, PipeScheduler? readerScheduler = null, PipeScheduler? writerScheduler = null, long pauseWriterThreshold = -1L, long resumeWriterThreshold = -1L, int minimumSegmentSize = -1, bool useSynchronizationContext = true)
	{
	}
}
