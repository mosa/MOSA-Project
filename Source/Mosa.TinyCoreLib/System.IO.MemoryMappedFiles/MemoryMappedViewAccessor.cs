using Microsoft.Win32.SafeHandles;

namespace System.IO.MemoryMappedFiles;

public sealed class MemoryMappedViewAccessor : UnmanagedMemoryAccessor
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

	internal MemoryMappedViewAccessor()
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	public void Flush()
	{
	}
}
