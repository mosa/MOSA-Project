﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using System.IO;
using Mosa.Compiler.Common;
using Mosa.Compiler.LinkerFormat.Elf;

namespace Mosa.Compiler.LinkerFormat.Elf32
{
	/// <summary>
	///
	/// </summary>
	public class Header
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
		public uint EntryAddress;

		/// <summary>
		/// This member holds the program header table's file offset in bytes. If the file has no
		/// program header table, this member holds zero.
		/// </summary>
		public uint ProgramHeaderOffset;

		/// <summary>
		/// This member holds the section header table's file offset in bytes. If the file has no
		/// section header table, this member holds zero.
		/// </summary>
		public uint SectionHeaderOffset;

		/// <summary>
		/// This member holds processor-specific flags associated with the file. Flag names
		/// take the form EF_machine_flag.
		/// </summary>
		public uint Flags;

		/// <summary>
		/// This member holds the ELF header's size in bytes.
		/// </summary>
		public ushort ElfHeaderSize;

		/// <summary>
		/// This member holds the size in bytes of one entry in the file's program header table;
		/// all entries are the same size.
		/// </summary>
		public ushort ProgramHeaderEntrySize;

		/// <summary>
		/// This member holds the number of entries in the program header table. Thus the
		/// product of ProgramHeaderEntrySize and ProgramHeaderNumber gives the table's size in bytes. If a file
		/// has no program header table,  ProgramHeaderNumber holds the value zero.
		/// </summary>
		public ushort ProgramHeaderNumber;

		/// <summary>
		/// This member holds a section header's size in bytes. A section header is one entry
		/// in the section header table; all entries are the same size.
		/// </summary>
		public ushort SectionHeaderEntrySize;

		/// <summary>
		/// This member holds the number of entries in the section header table. Thus the
		/// product of SectionHeaderEntrySize and SectionHeaderNumber gives the section header table's size in
		/// bytes. If a file has no section header table,  SectionHeaderNumber holds the value zero.
		/// </summary>
		public ushort SectionHeaderNumber;

		/// <summary>
		/// This member holds the section header table index of the entry associated with the
		/// section name string table. If the file has no section name string table, this member
		/// holds the value  SHN_UNDEF. See "Sections" and "String Table" below for more
		/// information.
		/// </summary>
		public ushort SectionHeaderStringIndex;

		/// <summary>
		///
		/// </summary>
		public static readonly byte[] MagicNumber = new byte[] { 0x7F, (byte)'E', (byte)'L', (byte)'F' };

		/// <summary>
		/// Initializes a new instance of the <see cref="Header"/> class.
		/// </summary>
		public Header()
		{
			ElfHeaderSize = 0x34;
			ProgramHeaderEntrySize = 0x20;
			SectionHeaderEntrySize = 0x28;
		}

		/// <summary>
		/// Writes the specified fs.
		/// </summary>
		/// <param name="writer">The writer.</param>
		public void Write(EndianAwareBinaryWriter writer)
		{
			writer.Seek(0, SeekOrigin.Begin);
			writer.Write(Ident);
			writer.Write((ushort)Type);
			writer.Write((ushort)Machine);
			writer.Write((uint)Version);
			writer.Write(EntryAddress);
			writer.Write(ProgramHeaderOffset);
			writer.Write(SectionHeaderOffset);
			writer.Write(Flags);
			writer.Write(ElfHeaderSize);
			writer.Write(ProgramHeaderEntrySize);
			writer.Write(ProgramHeaderNumber);
			writer.Write(SectionHeaderEntrySize);
			writer.Write(SectionHeaderNumber);
			writer.Write(SectionHeaderStringIndex);
		}

		/// <summary>
		/// Reads the specified reader.
		/// </summary>
		/// <param name="reader">The reader.</param>
		public void Read(EndianAwareBinaryReader reader)
		{
			Ident = reader.ReadBytes(16);

			// Check for magic number
			if (Ident[0] != MagicNumber[0] || Ident[1] != MagicNumber[1] || Ident[2] != MagicNumber[2] || Ident[3] != MagicNumber[3])
			{
				// Magic number not present, so it seems to be an invalid ELF file
				throw new NotSupportedException("This is not a valid ELF file");
			}

			Type = (FileType)reader.ReadUInt16();
			Machine = (MachineType)reader.ReadUInt16();
			Version = (Version)reader.ReadUInt32();
			EntryAddress = reader.ReadUInt32();
			ProgramHeaderOffset = reader.ReadUInt32();
			SectionHeaderOffset = reader.ReadUInt32();
			Flags = reader.ReadUInt32();
			ElfHeaderSize = reader.ReadUInt16();
			ProgramHeaderEntrySize = reader.ReadUInt16();
			ProgramHeaderNumber = reader.ReadUInt16();
			SectionHeaderEntrySize = reader.ReadUInt16();
			SectionHeaderNumber = reader.ReadUInt16();
			SectionHeaderStringIndex = reader.ReadUInt16();
		}

		/// <summary>
		/// Creates the ident.
		/// </summary>
		/// <param name="identClass">The ident class.</param>
		/// <param name="data">The data.</param>
		/// <param name="padding">The padding.</param>
		public void CreateIdent(IdentClass identClass, IdentData data, byte[] padding)
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

			// Set padding byte to
			Ident[7] = 0x07;

			for (int i = 8; i < 16; ++i)
				Ident[i] = 0x00;
		}

		/// <summary>
		/// Prints the info.
		/// </summary>
		public void PrintInfo()
		{
			Console.WriteLine("--------------");
			Console.WriteLine("| Elf32 Info:");
			Console.WriteLine("--------------");
			Console.WriteLine();
			Console.WriteLine("Magic number equals 0x7F454C46: Yes");
			Console.WriteLine("Ident class:                    {0} ({1})", ((IdentClass)Ident[4]), ((IdentClass)Ident[4]).ToString("x"));
			Console.WriteLine("Ident data:                     {0} ({1})", ((IdentData)Ident[4]), ((IdentData)Ident[4]).ToString("x"));
			Console.WriteLine("FileType:                       {0}", Type);
			Console.WriteLine("Machine:                        {0}", Machine);
			Console.WriteLine("Version:                        {0}", Version);
			Console.WriteLine("Entry VirtualAddress:           0x{0}", EntryAddress.ToString("x"));
			Console.WriteLine("ProgramHeaderOffset:            0x{0}", ProgramHeaderOffset.ToString("x"));
			Console.WriteLine("SectionHeaderOffset:            0x{0}", SectionHeaderOffset.ToString("x"));
			Console.WriteLine("Flags:                          0x{0}", Flags.ToString("x"));
			Console.WriteLine("Header size:                    0x{0}", ElfHeaderSize.ToString("x"));
			Console.WriteLine("ProgramHeaderEntrySize:         0x{0}", ProgramHeaderEntrySize.ToString("x"));
			Console.WriteLine("ProgramHeaderNumber:            0x{0}", ProgramHeaderNumber.ToString("x"));
			Console.WriteLine("SectionHeaderEntrySize:         0x{0}", SectionHeaderEntrySize.ToString("x"));
			Console.WriteLine("SectionHeaderNumber:            0x{0}", SectionHeaderNumber.ToString("x"));
			Console.WriteLine("SectionHeaderStringIndex:       0x{0}", SectionHeaderStringIndex.ToString("x"));
		}
	}
}