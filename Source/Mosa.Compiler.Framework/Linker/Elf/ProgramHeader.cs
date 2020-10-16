// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;

namespace Mosa.Compiler.Framework.Linker.Elf
{
	/// <summary>
	/// Program Header
	/// </summary>
	public class ProgramHeader
	{
		/// <summary>
		/// This member holds the size in bytes of one entry in the file's program header table;
		/// all entries are the same size.
		/// </summary>
		public const ushort EntrySize32 = 0x20;

		internal const ushort EntrySize64 = 0x38;

		/// <summary>
		/// This member tells what kind of segment this array element describes or how to
		/// interpret the array element's information.
		/// </summary>
		public ProgramHeaderType Type;

		/// <summary>
		/// This member gives the offset from the beginning of the file at which the first byte
		/// of the segment resides.
		/// </summary>
		public ulong Offset;

		/// <summary>
		/// This member gives the virtual virtualAddress at which the first byte of the segment resides
		/// in memory.
		/// </summary>
		public ulong VirtualAddress;

		/// <summary>
		/// On systems for which physical addressing is relevant, this member is reserved for
		/// the segment's physical virtualAddress.
		/// </summary>
		public ulong PhysicalAddress;

		/// <summary>
		/// This member gives the number of bytes in the file image of the segment; it may be
		/// zero.
		/// </summary>
		public ulong FileSize;

		/// <summary>
		/// This member gives the number of bytes in the memory image of the segment; it
		/// may be zero.
		/// </summary>
		public ulong MemorySize;

		/// <summary>
		/// This member gives flags relevant to the segment.
		/// </summary>
		public ProgramHeaderFlags Flags;

		/// <summary>
		/// The alignment
		/// </summary>
		public ulong Alignment;

		public static int GetEntrySize(LinkerFormatType elfType)
		{
			if (elfType == LinkerFormatType.Elf32)
				return EntrySize32;
			else // if (elfType == ElfType.Elf64)
				return EntrySize64;
		}

		/// <summary>
		/// Writes the program header
		/// </summary>
		/// <param name="elfType">Type of the elf.</param>
		/// <param name="writer">The writer.</param>
		public void Write(LinkerFormatType elfType, BinaryWriter writer)
		{
			if (elfType == LinkerFormatType.Elf32)
				Write32(writer);
			else if (elfType == LinkerFormatType.Elf64)
				Write64(writer);
		}

		/// <summary>
		/// Writes the program header
		/// </summary>
		/// <param name="writer">The writer.</param>
		protected void Write32(BinaryWriter writer)
		{
			writer.Write((uint)Type);
			writer.Write((uint)Offset);
			writer.Write((uint)VirtualAddress);
			writer.Write((uint)PhysicalAddress);
			writer.Write((uint)FileSize);
			writer.Write((uint)MemorySize);
			writer.Write((uint)Flags);
			writer.Write((uint)Alignment);
		}

		/// <summary>
		/// Writes the program header
		/// </summary>
		/// <param name="writer">The writer.</param>
		protected void Write64(BinaryWriter writer)
		{
			writer.Write((uint)Type);
			writer.Write((uint)Flags);
			writer.Write(Offset);
			writer.Write(VirtualAddress);
			writer.Write(PhysicalAddress);
			writer.Write(FileSize);
			writer.Write(MemorySize);
			writer.Write(Alignment);
		}
	}
}
