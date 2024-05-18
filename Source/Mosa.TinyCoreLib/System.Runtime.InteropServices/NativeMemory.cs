namespace System.Runtime.InteropServices;

public static class NativeMemory
{
	[CLSCompliant(false)]
	public unsafe static void* AlignedAlloc(UIntPtr byteCount, UIntPtr alignment)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static void AlignedFree(void* ptr)
	{
	}

	[CLSCompliant(false)]
	public unsafe static void* AlignedRealloc(void* ptr, UIntPtr byteCount, UIntPtr alignment)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static void* Alloc(UIntPtr byteCount)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static void* Alloc(UIntPtr elementCount, UIntPtr elementSize)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static void* AllocZeroed(UIntPtr byteCount)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static void* AllocZeroed(UIntPtr elementCount, UIntPtr elementSize)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static void Free(void* ptr)
	{
	}

	[CLSCompliant(false)]
	public unsafe static void* Realloc(void* ptr, UIntPtr byteCount)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static void Clear(void* ptr, UIntPtr byteCount)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static void Copy(void* source, void* destination, UIntPtr byteCount)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static void Fill(void* ptr, UIntPtr byteCount, byte value)
	{
		throw null;
	}
}
