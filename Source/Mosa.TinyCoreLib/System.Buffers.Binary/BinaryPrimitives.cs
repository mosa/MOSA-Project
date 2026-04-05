using System.Runtime.InteropServices;

namespace System.Buffers.Binary;

public static class BinaryPrimitives
{
	public static double ReadDoubleBigEndian(ReadOnlySpan<byte> source) => throw new NotImplementedException();

	public static double ReadDoubleLittleEndian(ReadOnlySpan<byte> source) => throw new NotImplementedException();

	public static Half ReadHalfBigEndian(ReadOnlySpan<byte> source) => throw new NotImplementedException();

	public static Half ReadHalfLittleEndian(ReadOnlySpan<byte> source) => throw new NotImplementedException();

	public static short ReadInt16BigEndian(ReadOnlySpan<byte> source)
	{
		var result = MemoryMarshal.Read<short>(source);

		if (BitConverter.IsLittleEndian)
			result = ReverseEndianness(result);

		return result;
	}

	public static short ReadInt16LittleEndian(ReadOnlySpan<byte> source)
	{
		var result = MemoryMarshal.Read<short>(source);

		if (!BitConverter.IsLittleEndian)
			result = ReverseEndianness(result);

		return result;
	}

	public static int ReadInt32BigEndian(ReadOnlySpan<byte> source)
	{
		var result = MemoryMarshal.Read<int>(source);

		if (BitConverter.IsLittleEndian)
			result = ReverseEndianness(result);

		return result;
	}

	public static int ReadInt32LittleEndian(ReadOnlySpan<byte> source)
	{
		var result = MemoryMarshal.Read<int>(source);

		if (!BitConverter.IsLittleEndian)
			result = ReverseEndianness(result);

		return result;
	}

	public static long ReadInt64BigEndian(ReadOnlySpan<byte> source)
	{
		var result = MemoryMarshal.Read<long>(source);

		if (BitConverter.IsLittleEndian)
			result = ReverseEndianness(result);

		return result;
	}

	public static long ReadInt64LittleEndian(ReadOnlySpan<byte> source)
	{
		var result = MemoryMarshal.Read<long>(source);

		if (!BitConverter.IsLittleEndian)
			result = ReverseEndianness(result);

		return result;
	}

	public static Int128 ReadInt128BigEndian(ReadOnlySpan<byte> source)
	{
		var result = MemoryMarshal.Read<Int128>(source);

		if (BitConverter.IsLittleEndian)
			result = ReverseEndianness(result);

		return result;
	}

	public static Int128 ReadInt128LittleEndian(ReadOnlySpan<byte> source)
	{
		var result = MemoryMarshal.Read<Int128>(source);

		if (!BitConverter.IsLittleEndian)
			result = ReverseEndianness(result);

		return result;
	}

	public static IntPtr ReadIntPtrBigEndian(ReadOnlySpan<byte> source)
	{
		var result = MemoryMarshal.Read<IntPtr>(source);

		if (BitConverter.IsLittleEndian)
			result = ReverseEndianness(result);

		return result;
	}

	public static IntPtr ReadIntPtrLittleEndian(ReadOnlySpan<byte> source)
	{
		var result = MemoryMarshal.Read<IntPtr>(source);

		if (!BitConverter.IsLittleEndian)
			result = ReverseEndianness(result);

		return result;
	}

	public static float ReadSingleBigEndian(ReadOnlySpan<byte> source) => throw new NotImplementedException();

	public static float ReadSingleLittleEndian(ReadOnlySpan<byte> source) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static ushort ReadUInt16BigEndian(ReadOnlySpan<byte> source)
	{
		var result = MemoryMarshal.Read<ushort>(source);

		if (BitConverter.IsLittleEndian)
			result = ReverseEndianness(result);

		return result;
	}

	[CLSCompliant(false)]
	public static ushort ReadUInt16LittleEndian(ReadOnlySpan<byte> source)
	{
		var result = MemoryMarshal.Read<ushort>(source);

		if (!BitConverter.IsLittleEndian)
			result = ReverseEndianness(result);

		return result;
	}

	[CLSCompliant(false)]
	public static uint ReadUInt32BigEndian(ReadOnlySpan<byte> source)
	{
		var result = MemoryMarshal.Read<uint>(source);

		if (BitConverter.IsLittleEndian)
			result = ReverseEndianness(result);

		return result;
	}

	[CLSCompliant(false)]
	public static uint ReadUInt32LittleEndian(ReadOnlySpan<byte> source)
	{
		var result = MemoryMarshal.Read<uint>(source);

		if (!BitConverter.IsLittleEndian)
			result = ReverseEndianness(result);

		return result;
	}

	[CLSCompliant(false)]
	public static ulong ReadUInt64BigEndian(ReadOnlySpan<byte> source)
	{
		var result = MemoryMarshal.Read<ulong>(source);

		if (BitConverter.IsLittleEndian)
			result = ReverseEndianness(result);

		return result;
	}

	[CLSCompliant(false)]
	public static ulong ReadUInt64LittleEndian(ReadOnlySpan<byte> source)
	{
		var result = MemoryMarshal.Read<ulong>(source);

		if (!BitConverter.IsLittleEndian)
			result = ReverseEndianness(result);

		return result;
	}

	[CLSCompliant(false)]
	public static UInt128 ReadUInt128BigEndian(ReadOnlySpan<byte> source)
	{
		var result = MemoryMarshal.Read<UInt128>(source);

		if (BitConverter.IsLittleEndian)
			result = ReverseEndianness(result);

		return result;
	}

	[CLSCompliant(false)]
	public static UInt128 ReadUInt128LittleEndian(ReadOnlySpan<byte> source)
	{
		var result = MemoryMarshal.Read<UInt128>(source);

		if (!BitConverter.IsLittleEndian)
			result = ReverseEndianness(result);

		return result;
	}

	[CLSCompliant(false)]
	public static UIntPtr ReadUIntPtrBigEndian(ReadOnlySpan<byte> source)
	{
		var result = MemoryMarshal.Read<UIntPtr>(source);

		if (BitConverter.IsLittleEndian)
			result = ReverseEndianness(result);

		return result;
	}

	[CLSCompliant(false)]
	public static UIntPtr ReadUIntPtrLittleEndian(ReadOnlySpan<byte> source)
	{
		var result = MemoryMarshal.Read<UIntPtr>(source);

		if (!BitConverter.IsLittleEndian)
			result = ReverseEndianness(result);

		return result;
	}

	public static byte ReverseEndianness(byte value) => throw new NotImplementedException();

	public static short ReverseEndianness(short value) => throw new NotImplementedException();

	public static int ReverseEndianness(int value) => throw new NotImplementedException();

	public static long ReverseEndianness(long value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static sbyte ReverseEndianness(sbyte value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static ushort ReverseEndianness(ushort value)
	{
		// Don't need to AND with 0xFF00 or 0x00FF since the final
		// cast back to ushort will clear out all bits above [ 15 .. 00 ].
		// This is normally implemented via "movzx eax, ax" on the return.
		// Alternatively, the compiler could elide the movzx instruction
		// entirely if it knows the caller is only going to access "ax"
		// instead of "eax" / "rax" when the function returns.

		return (ushort)((value >> 8) + (value << 8));
	}

	[CLSCompliant(false)]
	public static uint ReverseEndianness(uint value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static ulong ReverseEndianness(ulong value) => throw new NotImplementedException();

	public static IntPtr ReverseEndianness(IntPtr value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static UIntPtr ReverseEndianness(UIntPtr value) => throw new NotImplementedException();

	public static Int128 ReverseEndianness(Int128 value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static UInt128 ReverseEndianness(UInt128 value) => throw new NotImplementedException();

	public static void ReverseEndianness(ReadOnlySpan<int> source, Span<int> destination) => throw new NotImplementedException();

	public static void ReverseEndianness(ReadOnlySpan<Int128> source, Span<Int128> destination) => throw new NotImplementedException();

	public static void ReverseEndianness(ReadOnlySpan<long> source, Span<long> destination) => throw new NotImplementedException();

	public static void ReverseEndianness(ReadOnlySpan<IntPtr> source, Span<IntPtr> destination) => throw new NotImplementedException();

	public static void ReverseEndianness(ReadOnlySpan<short> source, Span<short> destination) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static void ReverseEndianness(ReadOnlySpan<UIntPtr> source, Span<UIntPtr> destination) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static void ReverseEndianness(ReadOnlySpan<uint> source, Span<uint> destination) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static void ReverseEndianness(ReadOnlySpan<UInt128> source, Span<UInt128> destination) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static void ReverseEndianness(ReadOnlySpan<ulong> source, Span<ulong> destination) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static void ReverseEndianness(ReadOnlySpan<ushort> source, Span<ushort> destination) => throw new NotImplementedException();

	public static bool TryReadDoubleBigEndian(ReadOnlySpan<byte> source, out double value) => throw new NotImplementedException();

	public static bool TryReadDoubleLittleEndian(ReadOnlySpan<byte> source, out double value) => throw new NotImplementedException();

	public static bool TryReadHalfBigEndian(ReadOnlySpan<byte> source, out Half value) => throw new NotImplementedException();

	public static bool TryReadHalfLittleEndian(ReadOnlySpan<byte> source, out Half value) => throw new NotImplementedException();

	public static bool TryReadInt16BigEndian(ReadOnlySpan<byte> source, out short value) => throw new NotImplementedException();

	public static bool TryReadInt16LittleEndian(ReadOnlySpan<byte> source, out short value) => throw new NotImplementedException();

	public static bool TryReadInt32BigEndian(ReadOnlySpan<byte> source, out int value) => throw new NotImplementedException();

	public static bool TryReadInt32LittleEndian(ReadOnlySpan<byte> source, out int value) => throw new NotImplementedException();

	public static bool TryReadInt64BigEndian(ReadOnlySpan<byte> source, out long value) => throw new NotImplementedException();

	public static bool TryReadInt64LittleEndian(ReadOnlySpan<byte> source, out long value) => throw new NotImplementedException();

	public static bool TryReadInt128BigEndian(ReadOnlySpan<byte> source, out Int128 value) => throw new NotImplementedException();

	public static bool TryReadInt128LittleEndian(ReadOnlySpan<byte> source, out Int128 value) => throw new NotImplementedException();

	public static bool TryReadIntPtrBigEndian(ReadOnlySpan<byte> source, out IntPtr value) => throw new NotImplementedException();

	public static bool TryReadIntPtrLittleEndian(ReadOnlySpan<byte> source, out IntPtr value) => throw new NotImplementedException();

	public static bool TryReadSingleBigEndian(ReadOnlySpan<byte> source, out float value) => throw new NotImplementedException();

	public static bool TryReadSingleLittleEndian(ReadOnlySpan<byte> source, out float value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static bool TryReadUInt16BigEndian(ReadOnlySpan<byte> source, out ushort value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static bool TryReadUInt16LittleEndian(ReadOnlySpan<byte> source, out ushort value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static bool TryReadUInt32BigEndian(ReadOnlySpan<byte> source, out uint value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static bool TryReadUInt32LittleEndian(ReadOnlySpan<byte> source, out uint value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static bool TryReadUInt64BigEndian(ReadOnlySpan<byte> source, out ulong value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static bool TryReadUInt64LittleEndian(ReadOnlySpan<byte> source, out ulong value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static bool TryReadUInt128BigEndian(ReadOnlySpan<byte> source, out UInt128 value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static bool TryReadUInt128LittleEndian(ReadOnlySpan<byte> source, out UInt128 value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static bool TryReadUIntPtrBigEndian(ReadOnlySpan<byte> source, out UIntPtr value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static bool TryReadUIntPtrLittleEndian(ReadOnlySpan<byte> source, out UIntPtr value) => throw new NotImplementedException();

	public static bool TryWriteDoubleBigEndian(Span<byte> destination, double value) => throw new NotImplementedException();

	public static bool TryWriteDoubleLittleEndian(Span<byte> destination, double value) => throw new NotImplementedException();

	public static bool TryWriteHalfBigEndian(Span<byte> destination, Half value) => throw new NotImplementedException();

	public static bool TryWriteHalfLittleEndian(Span<byte> destination, Half value) => throw new NotImplementedException();

	public static bool TryWriteInt16BigEndian(Span<byte> destination, short value) => throw new NotImplementedException();

	public static bool TryWriteInt16LittleEndian(Span<byte> destination, short value) => throw new NotImplementedException();

	public static bool TryWriteInt32BigEndian(Span<byte> destination, int value) => throw new NotImplementedException();

	public static bool TryWriteInt32LittleEndian(Span<byte> destination, int value) => throw new NotImplementedException();

	public static bool TryWriteInt64BigEndian(Span<byte> destination, long value) => throw new NotImplementedException();

	public static bool TryWriteInt64LittleEndian(Span<byte> destination, long value) => throw new NotImplementedException();

	public static bool TryWriteInt128BigEndian(Span<byte> destination, Int128 value) => throw new NotImplementedException();

	public static bool TryWriteInt128LittleEndian(Span<byte> destination, Int128 value) => throw new NotImplementedException();

	public static bool TryWriteIntPtrBigEndian(Span<byte> destination, IntPtr value) => throw new NotImplementedException();

	public static bool TryWriteIntPtrLittleEndian(Span<byte> destination, IntPtr value) => throw new NotImplementedException();

	public static bool TryWriteSingleBigEndian(Span<byte> destination, float value) => throw new NotImplementedException();

	public static bool TryWriteSingleLittleEndian(Span<byte> destination, float value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static bool TryWriteUInt16BigEndian(Span<byte> destination, ushort value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static bool TryWriteUInt16LittleEndian(Span<byte> destination, ushort value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static bool TryWriteUInt32BigEndian(Span<byte> destination, uint value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static bool TryWriteUInt32LittleEndian(Span<byte> destination, uint value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static bool TryWriteUInt64BigEndian(Span<byte> destination, ulong value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static bool TryWriteUInt64LittleEndian(Span<byte> destination, ulong value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static bool TryWriteUInt128BigEndian(Span<byte> destination, UInt128 value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static bool TryWriteUInt128LittleEndian(Span<byte> destination, UInt128 value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static bool TryWriteUIntPtrBigEndian(Span<byte> destination, UIntPtr value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static bool TryWriteUIntPtrLittleEndian(Span<byte> destination, UIntPtr value) => throw new NotImplementedException();

	public static void WriteDoubleBigEndian(Span<byte> destination, double value) => throw new NotImplementedException();

	public static void WriteDoubleLittleEndian(Span<byte> destination, double value) => throw new NotImplementedException();

	public static void WriteHalfBigEndian(Span<byte> destination, Half value) => throw new NotImplementedException();

	public static void WriteHalfLittleEndian(Span<byte> destination, Half value) => throw new NotImplementedException();

	public static void WriteInt16BigEndian(Span<byte> destination, short value) => throw new NotImplementedException();

	public static void WriteInt16LittleEndian(Span<byte> destination, short value) => throw new NotImplementedException();

	public static void WriteInt32BigEndian(Span<byte> destination, int value) => throw new NotImplementedException();

	public static void WriteInt32LittleEndian(Span<byte> destination, int value) => throw new NotImplementedException();

	public static void WriteInt64BigEndian(Span<byte> destination, long value) => throw new NotImplementedException();

	public static void WriteInt64LittleEndian(Span<byte> destination, long value) => throw new NotImplementedException();

	public static void WriteInt128BigEndian(Span<byte> destination, Int128 value) => throw new NotImplementedException();

	public static void WriteInt128LittleEndian(Span<byte> destination, Int128 value) => throw new NotImplementedException();

	public static void WriteIntPtrBigEndian(Span<byte> destination, IntPtr value) => throw new NotImplementedException();

	public static void WriteIntPtrLittleEndian(Span<byte> destination, IntPtr value) => throw new NotImplementedException();

	public static void WriteSingleBigEndian(Span<byte> destination, float value) => throw new NotImplementedException();

	public static void WriteSingleLittleEndian(Span<byte> destination, float value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static void WriteUInt16BigEndian(Span<byte> destination, ushort value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static void WriteUInt16LittleEndian(Span<byte> destination, ushort value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static void WriteUInt32BigEndian(Span<byte> destination, uint value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static void WriteUInt32LittleEndian(Span<byte> destination, uint value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static void WriteUInt64BigEndian(Span<byte> destination, ulong value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static void WriteUInt64LittleEndian(Span<byte> destination, ulong value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static void WriteUInt128BigEndian(Span<byte> destination, UInt128 value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static void WriteUInt128LittleEndian(Span<byte> destination, UInt128 value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static void WriteUIntPtrBigEndian(Span<byte> destination, UIntPtr value) => throw new NotImplementedException();

	[CLSCompliant(false)]
	public static void WriteUIntPtrLittleEndian(Span<byte> destination, UIntPtr value) => throw new NotImplementedException();
}
