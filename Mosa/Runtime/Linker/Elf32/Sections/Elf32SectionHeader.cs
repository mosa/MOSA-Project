/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon "Kintaro" Wollwage (<mailto:kintaro@think-in-co.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Linker.Elf32.Sections
{
    /// <summary>
    /// 
    /// </summary>
    public class Elf32SectionHeader
    {
        /// <summary>
        /// This member specifies the name of the section. Its value is an index into 
        /// the section _header string table section , giving 
        /// the location of a null-terminated string. 
        /// </summary>
        public uint Name;
        /// <summary>
        /// This member categorizes the section's contents and semantics. Section 
        /// types and their descriptions appear below.
        /// </summary>
        public Elf32SectionType Type;
        /// <summary>
        /// Sections support 1-bit flags that describe miscellaneous attributes. 
        /// </summary>
        public Elf32SectionAttribute Flags;
        /// <summary>
        /// If the section will appear in the memory image of a process, this member 
        /// gives the virtualAddress at which the section's first byte should reside. Otherwise, 
        /// the member contains 0. 
        /// </summary>
        public uint Address;
        /// <summary>
        /// This member's value gives the byte offset from the beginning of the file to 
        /// the first byte in the section. One section type, NoBits,occupies no 
        /// space in the file, and its Offset member locates 
        /// the conceptual placement in the file. 
        /// </summary>
        public uint Offset;
        /// <summary>
        /// 
        /// </summary>
        public uint Size;
        /// <summary>
        /// This member holds a section _header table index link, whose interpretation 
        /// depends on the section type. 
        /// </summary>
        public uint Link;
        /// <summary>
        /// This member holds extra information, whose interpretation depends on the 
        /// section type.
        /// </summary>
        public uint Info;
        /// <summary>
        /// Some sections have virtualAddress alignment constraints. For example, if a section 
        /// holds a doubleword, the system must ensure doubleword alignment for the 
        /// entire section.  That is, the value of sh_addr must be congruent to 0, 
        /// modulo the value of sh_addralign. Currently, only 0 and positive 
        /// integral powers of two are allowed. Values 0 and 1 mean the section has no 
        /// alignment constraints. 
        /// </summary>
        public uint AddressAlignment;
        /// <summary>
        /// Some sections hold a table of fixed-size entries, such as a symbol table. For 
        /// such a section, this member gives the size in bytes of each entry. The 
        /// member contains 0 if the section does not hold a table of fixed-size entries. 
        /// </summary>
        public uint EntrySize;

        /// <summary>
        /// Writes the specified fs.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void Write(System.IO.BinaryWriter writer)
        {
            Address = (uint)writer.BaseStream.Position;
            writer.Write(Name);
            writer.Write((uint)Type);
            writer.Write((uint)Flags);
            writer.Write(Address);
            writer.Write(Offset);
            writer.Write(Size);
            writer.Write(Link);
            writer.Write(Info);
            writer.Write(AddressAlignment);
            writer.Write(EntrySize);
        }

        /// <summary>
        /// Reads the specified writer.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public void Read(System.IO.BinaryReader reader)
        {
            Address = reader.ReadUInt16();
            Name = reader.ReadUInt32();
            Type = (Elf32SectionType)reader.ReadUInt16();
            Flags = (Elf32SectionAttribute)reader.ReadUInt16();
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
