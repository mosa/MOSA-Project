using System.Runtime.InteropServices;

namespace System.Buffers;

public struct MemoryHandle : IDisposable
{
	private unsafe void* pointer;
	private GCHandle handle;
	private IPinnable? pinnable;

	[CLSCompliant(false)] public unsafe void* Pointer => pointer;

	[CLSCompliant(false)]
	public unsafe MemoryHandle(void* pointer, GCHandle handle = default, IPinnable? pinnable = null)
	{
		this.pointer = pointer;
		this.handle = handle;
		this.pinnable = pinnable;
	}

	public void Dispose()
	{
		handle.Free();
		
		if (pinnable != null)
		{
			pinnable.Unpin();
			pinnable = null;
		}

		unsafe
		{
			pointer = null;
		}
	}
}
