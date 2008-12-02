/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon "Kintaro" Wollwage (<mailto:kintaro@mosa-project.org>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Linker.Elf
{
    /// <summary>
    /// 
    /// </summary>
    public class Elf32Header
    {
        /// <summary>
        /// The initial bytes mark the file as an object file and provide machine-independent 
        /// data with which to decode and interpret the file's contents.  
        /// </summary>
        public byte[] Ident = new byte[16];
        /// <summary>
        /// This member identifies the object file type
        /// </summary>
        public Elf32FileType Type;
        /// <summary>
        /// This member's value specifies the required architecture for an individual file.
        /// </summary>
        public Elf32MachineType Machine;
        /// <summary>
        /// This member identifies the object file version. 
        /// </summary>
        public Elf32Version Version;
        /// <summary>
        /// This member gives the virtual address to which the system first transfers control, 
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
        public static readonly byte[] MagicNumber = new byte[4] { (byte)0x7F, (byte)'E', (byte)'L', (byte)'F' };

        /// <summary>
        /// Writes the specified fs.
        /// </summary>
        /// <param name="fs">The fs.</param>
        public void Write(System.IO.FileStream fs)
        {
            System.IO.BinaryWriter writer = new System.IO.BinaryWriter(fs);
            writer.Seek(0, System.IO.SeekOrigin.Begin);
            CreateIdent(Elf32IdentClass.Class32, Elf32IdentData.Data2LSB, null);
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
        /// Creates the ident.
        /// </summary>
        /// <param name="identClass">The ident class.</param>
        /// <param name="data">The data.</param>
        /// <param name="padding">The padding.</param>
        public void CreateIdent(Elf32IdentClass identClass, Elf32IdentData data, byte[] padding)
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
            Ident[6] = (byte)Elf32Version.Current;

            // Set padding byte to 
            Ident[7] = 0x07;

            for (int i = 8; i < 16; ++i)
                Ident[i] = 0x00;
        }
    }
}
