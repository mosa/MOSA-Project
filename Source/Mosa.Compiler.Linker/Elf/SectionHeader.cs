// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;

namespace Mosa.Compiler.Linker.Elf
{
	/// <summary>
	///
	/// </summary>
	public class SectionHeader
	{
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
		public long Offset;

		/// <summary>
		///
		/// </summary>
		public ulong Size;

		/// <summary>
		/// This member holds a section header table index link, whose interpretation
		/// depends on the section type.
		/// </summary>
		public uint Link;

		/// <summary>
		/// This member holds extra information, whose interpretation depends on the
		/// section type.
		/// </summary>
		public uint Info;

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

		/// <summary>
		/// Writes the section header
		/// </summary>
		/// <param name="writer">The writer.</param>
		public void Write32(BinaryWriter writer)
		{
			writer.Write((uint)Name);
			writer.Write((uint)Type);
			writer.Write((uint)Flags);
			writer.Write((uint)Address);
			writer.Write((int)Offset);
			writer.Write((uint)Size);
			writer.Write((uint)Link);
			writer.Write((uint)Info);
			writer.Write((uint)AddressAlignment);
			writer.Write((uint)EntrySize);
		}

		/// <summary>
		/// Writes the section header
		/// </summary>
		/// <param name="writer">The writer.</param>
		public void Write64(BinaryWriter writer)
		{
			writer.Write((uint)Name);
			writer.Write((uint)Type);
			writer.Write((ulong)Flags);
			writer.Write((ulong)Address);
			writer.Write((long)Offset);
			writer.Write((ulong)Size);
			writer.Write((uint)Link);
			writer.Write((uint)Info);
			writer.Write((ulong)AddressAlignment);
			writer.Write((ulong)EntrySize);
		}

		/// <summary>
		/// Reads the section header
		/// </summary>
		/// <param name="reader">The reader.</param>
		public void Read32(BinaryReader reader)
		{
			Address = reader.ReadUInt16();
			Name = reader.ReadUInt32();
			Type = (SectionType)reader.ReadUInt16();
			Flags = (SectionAttribute)reader.ReadUInt16();
			Address = reader.ReadUInt32();
			Offset = reader.ReadUInt32();
			Size = reader.ReadUInt32();
			Link = reader.ReadUInt32();
			Info = reader.ReadUInt32();
			AddressAlignment = reader.ReadUInt32();
			EntrySize = reader.ReadUInt32();
		}
	}
}
