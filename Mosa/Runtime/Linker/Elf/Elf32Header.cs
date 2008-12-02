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

namespace Mosa.ObjectFiles.Elf32
{
    /// <summary>
    /// 
    /// </summary>
    public class Elf32Header
    {
        /// <summary>
        /// 
        /// </summary>
        public byte[] Ident = new byte[16];
        /// <summary>
        /// 
        /// </summary>
        public Elf32FileType Type;
        /// <summary>
        /// 
        /// </summary>
        public Elf32MachineType Machine;
        /// <summary>
        /// 
        /// </summary>
        public Elf32Version Version;
        /// <summary>
        /// 
        /// </summary>
        public uint EntryAddress;
        /// <summary>
        /// 
        /// </summary>
        public uint ProgramHeaderOffset;
        /// <summary>
        /// 
        /// </summary>
        public uint SectionHeaderOffset;
        /// <summary>
        /// 
        /// </summary>
        public uint Flags;
        /// <summary>
        /// 
        /// </summary>
        public ushort ElfHeaderSize;
        /// <summary>
        /// 
        /// </summary>
        public ushort ProgramHeaderEntrySize;
        /// <summary>
        /// 
        /// </summary>
        public ushort ProgramHeaderNumber;
        /// <summary>
        /// 
        /// </summary>
        public ushort SectionHeaderEntrySize;
        /// <summary>
        /// 
        /// </summary>
        public ushort SectionHeaderNumber;
        /// <summary>
        /// 
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
