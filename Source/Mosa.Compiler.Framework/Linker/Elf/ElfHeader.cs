// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.IO;

namespace Mosa.Compiler.Framework.Linker.Elf
{
	/// <summary>
	/// Elf Header
	/// </summary>
	public class ElfHeader
	{
		/// <summary>
		/// The initial bytes mark the file as an object file and provide machine-independent
		/// data with which to decode and interpret the file's contents.
		/// </summary>
		public byte[] Ident = new byte[16];

		/// <summary>
		/// This member identifies the object file type
		/// </summary>
		public FileType Type;

		/// <summary>
		/// This member's value specifies the required architecture for an individual file.
		/// </summary>
		public MachineType Machine;

		/// <summary>
		/// This member identifies the object file version.
		/// </summary>
		public Version Version;

		/// <summary>
		/// This member gives the virtual virtualAddress to which the system first transfers control,
		/// thus starting the process. If the file has no associated entry point, this member holds
		/// zero.
		/// </summary>
		public ulong EntryAddress;

		/// <summary>
		/// This member holds the program header table's file offset in bytes. If the file has no
		/// program header table, this member holds zero.
		/// </summary>
		public long ProgramHeaderOffset;

		/// <summary>
		/// This member holds the section header table's file offset in bytes. If the file has no
		/// section header table, this member holds zero.
		/// </summary>
		public long SectionHeaderOffset;

		/// <summary>
		/// This member holds processor-specific flags associated with the file. Flag names
		/// take the form EF_machine_flag.
		/// </summary>
		public uint Flags;

		/// <summary>
		/// This member holds the ELF header's size in bytes.
		/// </summary>
		internal const ushort ElfHeaderSize32 = 0x34;

		internal const ushort ElfHeaderSize64 = 0x40;

		/// <summary>
		/// This member holds the number of entries in the program header table. Thus the
		/// product of ProgramHeaderEntrySize and ProgramHeaderNumber gives the table's size in bytes.
		/// </summary>
		public ushort ProgramHeaderNumber;

		/// <summary>
		/// This member holds the number of entries in the section header table. Thus the
		/// product of SectionHeaderEntrySize and SectionHeaderNumber gives the section header table's size in
		/// bytes.
		/// </summary>
		public ushort SectionHeaderNumber;

		/// <summary>
		/// This member holds the section header table index of the entry associated with the
		/// section name string table. If the file has no section name string table, this member
		/// holds the value  SHN_UNDEF. See "Sections" and "String Table" below for more
		/// information.
		/// </summary>
		public int SectionHeaderStringIndex;

		/// <summary>
		/// The magic number
		/// </summary>
		public static readonly byte[] MagicNumber = new byte[] { 0x7F, (byte)'E', (byte)'L', (byte)'F' };

		public static int GetEntrySize(LinkerFormatType elfType)
		{
			if (elfType == LinkerFormatType.Elf32)
				return ElfHeaderSize32;
			else // if (elfType == ElfType.Elf64)
				return ElfHeaderSize64;
		}

		/// <summary>
		/// Writes the elf header
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
		/// Writes the elf header
		/// </summary>
		/// <param name="writer">The writer.</param>
		protected void Write32(BinaryWriter writer)
		{
			writer.Seek(0, SeekOrigin.Begin);
			writer.Write(Ident);                    // ident
			writer.Write((ushort)Type);             // type
			writer.Write((ushort)Machine);          // machine
			writer.Write((uint)Version);            // version
			writer.Write((uint)EntryAddress);             // entry
			writer.Write((uint)ProgramHeaderOffset);      // phoff
			writer.Write((uint)SectionHeaderOffset);      // shoff
			writer.Write(Flags);                    // flags
			writer.Write(ElfHeaderSize32);            // ehsize
			writer.Write(ProgramHeader.EntrySize32);   // phentsize
			writer.Write(ProgramHeaderNumber);      // phnum
			writer.Write(SectionHeaderEntry.EntrySize32);   // shentsize
			writer.Write((ushort)SectionHeaderNumber);      // shnum
			writer.Write((ushort)SectionHeaderStringIndex); // shstrndx
		}

		/// <summary>
		/// Writes the elf header
		/// </summary>
		/// <param name="writer">The writer.</param>
		protected void Write64(BinaryWriter writer)
		{
			writer.Seek(0, SeekOrigin.Begin);
			writer.Write(Ident);                    // ident
			writer.Write((ushort)Type);             // type
			writer.Write((ushort)Machine);          // machine
			writer.Write((uint)Version);            // version
			writer.Write(EntryAddress);             // entry
			writer.Write((ulong)ProgramHeaderOffset);      // phoff
			writer.Write((ulong)SectionHeaderOffset);      // shoff
			writer.Write(Flags);                    // flags
			writer.Write(ElfHeaderSize64);            // ehsize
			writer.Write(ProgramHeader.EntrySize64);   // phentsize
			writer.Write(ProgramHeaderNumber);      // phnum
			writer.Write(SectionHeaderEntry.EntrySize64);   // shentsize
			writer.Write((ushort)SectionHeaderNumber);      // shnum
			writer.Write((ushort)SectionHeaderStringIndex); // shstrndx
		}

		/// <summary>
		/// Creates the ident.
		/// </summary>
		/// <param name="identClass">The ident class.</param>
		/// <param name="data">The data.</param>
		///
		public void CreateIdent(IdentClass identClass, IdentData data)
		{
			// Store magic number
			Ident[0] = MagicNumber[0];
			Ident[1] = MagicNumber[1];
			Ident[2] = MagicNumber[2];
			Ident[3] = MagicNumber[3];

			// Store class
			Ident[4] = (byte)identClass;

			// Store data flags
			Ident[5] = (byte)data;

			// Version has to be current, otherwise the file won't load
			Ident[6] = (byte)Version.Current;
			Version = Version.Current;

			Ident[7] = 0x00;

			for (int i = 8; i < 16; ++i)
				Ident[i] = 0x00;
		}

		/// <summary>
		/// Prints the info.
		/// </summary>
		public void PrintInfo()
		{
			Console.WriteLine("--------------");
			Console.WriteLine("| Elf Info:");
			Console.WriteLine("--------------");
			Console.WriteLine();
			Console.WriteLine("Magic number equals 0x7F454C46: Yes");
			Console.WriteLine("Ident class:                    {0} ({1})", (IdentClass)Ident[4], ((IdentClass)Ident[4]).ToString("x"));
			Console.WriteLine("Ident data:                     {0} ({1})", (IdentData)Ident[4], ((IdentData)Ident[4]).ToString("x"));
			Console.WriteLine("FileType:                       {0}", Type);
			Console.WriteLine("Machine:                        {0}", Machine);
			Console.WriteLine("Version:                        {0}", Version);
			Console.WriteLine("Entry VirtualAddress:           0x{0}", EntryAddress.ToString("x"));
			Console.WriteLine("ProgramHeaderOffset:            0x{0}", ProgramHeaderOffset.ToString("x"));
			Console.WriteLine("SectionHeaderOffset:            0x{0}", SectionHeaderOffset.ToString("x"));
			Console.WriteLine("Flags:                          0x{0}", Flags.ToString("x"));
			Console.WriteLine("ProgramHeaderNumber:            0x{0}", ProgramHeaderNumber.ToString("x"));
			Console.WriteLine("SectionHeaderNumber:            0x{0}", SectionHeaderNumber.ToString("x"));
			Console.WriteLine("SectionHeaderStringIndex:       0x{0}", SectionHeaderStringIndex.ToString("x"));
		}
	}
}
