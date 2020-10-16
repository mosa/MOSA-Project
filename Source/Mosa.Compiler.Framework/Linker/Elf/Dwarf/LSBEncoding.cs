// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;

namespace Mosa.Compiler.Framework.Linker.Elf.Dwarf
{
	// Source: https://github.com/aumcode/nfx/blob/9fca1a4d1672cd724025f5ea242811d53197fad4/Source/NFX/IO/LEB128.cs
	public static class LSBEncoding
	{
		public static void WriteULEB128(byte[] buf, ulong value, out int count, int idxStart = 0, uint padding = 0)
		{
			var origIdx = idxStart;
			do
			{
				byte bt = (byte)((byte)value & 0x7f);
				value >>= 7;
				if (value != 0 || padding != 0)
					bt |= 0x80; // Mark this byte to show that more bytes will follow.

				buf[idxStart] = bt;
				idxStart++;
			}
			while (value != 0);

			count = idxStart - origIdx;
		}

		public static void WriteULEB128(this BinaryWriter writer, ulong value)
		{
			var bytes = new byte[16];
			int count;
			WriteULEB128(bytes, value, out count);
			writer.Write(bytes, 0, count);
		}

		public static void WriteSLEB128(this byte[] buf, long value, out int count, int idxStart = 0, int padding = 0)
		{
			var origIdx = idxStart;
			bool more;
			do
			{
				byte bt;
				unchecked
				{
					bt = (byte)((byte)value & 0x7f);
				}
				value >>= 7;
				more = !((((value == 0) && ((bt & 0x40) == 0)) ||
						  ((value == -1) && ((bt & 0x40) != 0))));
				if (more)
					bt |= 0x80; // Mark this byte to show that more bytes will follow.

				buf[idxStart] = bt;
				idxStart++;
			}
			while (more);

			// Pad with 0x80 and emit a null byte at the end.
			if (padding != 0)
			{
				for (; padding != 1; --padding)
				{
					buf[idxStart] = 0x80;
					idxStart++;
				}
				buf[idxStart] = 0x00;
				idxStart++;
			}

			count = idxStart - origIdx;
		}

		public static void WriteSLEB128(this BinaryWriter writer, long value)
		{
			var bytes = new byte[16];
			int count;
			WriteSLEB128(bytes, value, out count);
			writer.Write(bytes, 0, count);
		}
	}
}
