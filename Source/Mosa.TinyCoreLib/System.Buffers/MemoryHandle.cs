using System.Runtime.InteropServices;

namespace System.Buffers;

public struct MemoryHandle : IDisposable
{
	private object _dummy;

	private int _dummyPrimitive;

	[CLSCompliant(false)]
	public unsafe void* Pointer
	{
		get
		{
			throw null;
		}
	}

	[CLSCompliant(false)]
	public unsafe MemoryHandle(void* pointer, GCHandle handle = default(GCHandle), IPinnable? pinnable = null)
	{
		throw null;
	}

	public void Dispose()
	{
	}
}
