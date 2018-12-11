// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;

namespace Mosa.Compiler.Framework.Linker.Elf
{
	/// <summary>
	/// Section Header Entry
	/// </summary>
	public class SectionHeaderEntry
	{
		/// <summary>
		/// This member holds a section header's size in bytes. A section header is one entry
		/// in the section header table; all entries are the same size.
		/// </summary>
		public const ushort EntrySize32 = 0x28;

		public const ushort EntrySize64 = 0x40;

		/// <summary>
		/// This member specifies the name of the section. Its value is an index into
		/// the section header string table section , giving
		/// the location of a null-terminated string.
		/// </summary>
		public uint Name;

		/// <summary>
		/// This member categorizes the section's contents and semantics. Section
		/// types and their descriptions appear below.
		/// </summary>
		public SectionType Type;

		/// <summary>
		/// Sections support 1-bit flags that describe miscellaneous attributes.
		/// </summary>
		public SectionAttribute Flags;

		/// <summary>
		/// If the section will appear in the memory image of a process, this member
		/// gives the virtualAddress at which the section's first byte should reside. Otherwise,
		/// the member contains 0.
		/// </summary>
		public ulong Address;

		/// <summary>
		/// This member's value gives the byte offset from the beginning of the file to
		/// the first byte in the section. One section type, NoBits,occupies no
		/// space in the file, and its Offset member locates
		/// the conceptual placement in the file.
		/// </summary>
		public ulong Offset;

		/// <summary>
		/// Size
		/// </summary>
		public ulong Size;

		/// <summary>
		/// This member holds a section header table index link, whose interpretation
		/// depends on the section type.
		/// </summary>
		public int Link;

		/// <summary>
		/// This member holds extra information, whose interpretation depends on the
		/// section type.
		/// </summary>
		public int Info;

		/// <summary>
		/// Some sections have alignment constraints. For example, if a section
		/// holds a doubleword, the system must ensure doubleword alignment for the
		/// entire section.  That is, the value of sh_addr must be congruent to 0,
		/// modulo the value of sh_addralign. Currently, only 0 and positive
		/// integral powers of two are allowed. Values 0 and 1 mean the section has no
		/// alignment constraints.
		/// </summary>
		public ulong AddressAlignment;

		/// <summary>
		/// Some sections hold a table of fixed-size entries, such as a symbol table. For
		/// such a section, this member gives the size in bytes of each entry. The
		/// member contains 0 if the section does not hold a table of fixed-size entries.
		/// </summary>
		public ulong EntrySize;

		public static uint GetEntrySize(LinkerFormatType elfType)
		{
			if (elfType == LinkerFormatType.Elf32)
				return EntrySize32;
			else // if (elfType == ElfType.Elf64)
				return EntrySize64;
		}

		/// <summary>
		/// Writes the section header
		/// </summary>
		/// <param name="elfType">Type of the elf.</param>
		/// <param name="writer">The writer.</param>
		public void Write(LinkerFormatType elfType, BinaryWriter writer)
		{
			if (elfType == LinkerFormatType.Elf32)
				Write32(writer);
			else // if (elfType == ElfType.Elf64)
				Write64(writer);
		}

		/// <summary>
		/// Writes the section header
		/// </summary>
		/// <param name="writer">The writer.</param>
		protected void Write32(BinaryWriter writer)
		{
			writer.Write(Name);
			writer.Write((uint)Type);
			writer.Write((uint)Flags);
			writer.Write((uint)Address);
			writer.Write((int)Offset);
			writer.Write((uint)Size);
			writer.Write(Link);
			writer.Write(Info);
			writer.Write((uint)AddressAlignment);
			writer.Write((uint)EntrySize);
		}

		/// <summary>
		/// Writes the section header
		/// </summary>
		/// <param name="writer">The writer.</param>
		protected void Write64(BinaryWriter writer)
		{
			writer.Write(Name);
			writer.Write((uint)Type);
			writer.Write((ulong)Flags);
			writer.Write(Address);
			writer.Write(Offset);
			writer.Write(Size);
			writer.Write(Link);
			writer.Write(Info);
			writer.Write(AddressAlignment);
			writer.Write(EntrySize);
		}
	}
}
