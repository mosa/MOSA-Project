using Microsoft.Win32.SafeHandles;

namespace System.Runtime.InteropServices;

public abstract class SafeBuffer : SafeHandleZeroOrMinusOneIsInvalid
{
	[CLSCompliant(false)]
	public ulong ByteLength
	{
		get
		{
			throw null;
		}
	}

	protected SafeBuffer(bool ownsHandle)
		: base(ownsHandle: false)
	{
	}

	[CLSCompliant(false)]
	public unsafe void AcquirePointer(ref byte* pointer)
	{
	}

	[CLSCompliant(false)]
	public void Initialize(uint numElements, uint sizeOfEachElement)
	{
	}

	[CLSCompliant(false)]
	public void Initialize(ulong numBytes)
	{
	}

	[CLSCompliant(false)]
	public void Initialize<T>(uint numElements) where T : struct
	{
	}

	[CLSCompliant(false)]
	public void ReadArray<T>(ulong byteOffset, T[] array, int index, int count) where T : struct
	{
	}

	[CLSCompliant(false)]
	public void ReadSpan<T>(ulong byteOffset, Span<T> buffer) where T : struct
	{
	}

	[CLSCompliant(false)]
	public T Read<T>(ulong byteOffset) where T : struct
	{
		throw null;
	}

	public void ReleasePointer()
	{
	}

	[CLSCompliant(false)]
	public void WriteArray<T>(ulong byteOffset, T[] array, int index, int count) where T : struct
	{
	}

	[CLSCompliant(false)]
	public void WriteSpan<T>(ulong byteOffset, ReadOnlySpan<T> data) where T : struct
	{
	}

	[CLSCompliant(false)]
	public void Write<T>(ulong byteOffset, T value) where T : struct
	{
	}
}
