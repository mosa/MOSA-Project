using Microsoft.Win32.SafeHandles;

namespace System.IO.MemoryMappedFiles;

public sealed class MemoryMappedViewStream : UnmanagedMemoryStream
{
	public long PointerOffset
	{
		get
		{
			throw null;
		}
	}

	public SafeMemoryMappedViewHandle SafeMemoryMappedViewHandle
	{
		get
		{
			throw null;
		}
	}

	internal MemoryMappedViewStream()
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	public override void Flush()
	{
	}

	public override void SetLength(long value)
	{
	}
}
