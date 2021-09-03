// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;

namespace System.Buffers.Binary
{
	public class BinaryPrimitives
	{
		public static ushort ReadUInt16BigEndian(Span<byte> source)
		{
			ushort result = MemoryMarshal.Read<ushort>(source);

			if (BitConverter.IsLittleEndian)
				result = ReverseEndianness(result);

			return result;
		}

		public static ushort ReadUInt16BigEndian(ReadOnlySpan<byte> source)
		{
			ushort result = MemoryMarshal.Read<ushort>(source);

			if (BitConverter.IsLittleEndian)
				result = ReverseEndianness(result);

			return result;
		}

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
	}
}
