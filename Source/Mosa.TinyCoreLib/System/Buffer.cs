namespace System;

public static class Buffer
{
	public static void BlockCopy(Array src, int srcOffset, Array dst, int dstOffset, int count)
	{
	}

	public static int ByteLength(Array array)
	{
		throw null;
	}

	public static byte GetByte(Array array, int index)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static void MemoryCopy(void* source, void* destination, long destinationSizeInBytes, long sourceBytesToCopy)
	{
	}

	[CLSCompliant(false)]
	public unsafe static void MemoryCopy(void* source, void* destination, ulong destinationSizeInBytes, ulong sourceBytesToCopy)
	{
	}

	public static void SetByte(Array array, int index, byte value)
	{
	}
}
