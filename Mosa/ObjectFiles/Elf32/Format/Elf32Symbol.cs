/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 */

using System;
using System.Collections.Generic;
using System.Text;
using Mosa.ObjectFiles.Elf32.Format.Sections;
using System.IO;

namespace Mosa.ObjectFiles.Elf32.Format
{
    class Elf32Symbol
    {
        public const int SYM_SIZE = 16;

        public Elf32Symbol(object tag)
        {
            this.Tag = tag;
            Offset = -1;
            Size = -1;
        }

        public object Tag { get; private set; }
        public string Name { get; set; }
        public int Offset { get; private set; }
        public int Size { get; private set; }
        public Elf32SymbolType Type { get; set; }
        public Elf32SymbolBinding Bind { get; set; }
        public Elf32SymbolDefinitionSection Section { get; private set; }

        public void Write(Elf32File file, Elf32StringTableSection symbolNames, BinaryWriter writer)
        {
            if (IsDefined)
            {
                writer.Write((int)symbolNames.GetStringIndex(Name)); // st_name
                writer.Write((int)Offset); // st_value
                writer.Write((int)Size); // st_size
                writer.Write((byte)(((byte)Bind << 4) | ((byte)Type & 0xf))); // st_info
                writer.Write((byte)0); // st_other
                writer.Write((short)file.Sections.IndexOf(Section)); // st_shndx
            }
            else
            {
                writer.Write((int)symbolNames.GetStringIndex(Name)); // st_name
                writer.Write((int)0); // st_value
                writer.Write((int)0); // st_size
                writer.Write((byte)(((byte)Bind << 4) | ((byte)Type & 0xf))); // st_info
                writer.Write((byte)0); // st_other
                writer.Write((short)0); // st_shndx
            }
        }

        public void Begin(Elf32SymbolDefinitionSection section, int offset)
        {
            Section = section;
            Offset = offset;
        }
        public void End(int size)
        {
            Size = size;
        }

        public bool IsDefined
        {
            get { return Section != null && Offset >= 0; }
        }

        public bool IsFinished
        {
            get { return Section != null && Offset >= 0 && Size >= 0; }
        }
    }
}
