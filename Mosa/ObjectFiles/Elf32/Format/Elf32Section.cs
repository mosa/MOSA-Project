/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the GNU GPL v3, with Classpath Linking Exception
 * Licensed under the terms of the New BSD License for exclusive use by the Ensemble OS Project
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Mosa.ObjectFiles.Elf32.Format
{
    abstract class Elf32Section
    {
        string _name;
        Elf32SectionType _type;
        Elf32SectionFlags _flags;
        Elf32File _file;

        public const int SHDR_SIZE = 40;

        public Elf32Section(Elf32File file, string name, Elf32SectionType type, Elf32SectionFlags flags)
        {
            _name = name;
            _type = type;
            _flags = flags;
            _file = file;
            file.Sections.Add(this);
        }

        protected Elf32File File { get { return _file; } }
        public string Name { get { return _name; } }
        public Elf32SectionType Type { get { return _type; } }
        public Elf32SectionFlags Flags { get { return _flags; } }
        public virtual IntPtr RuntimeImageAddress { get { return IntPtr.Zero; } }
        public virtual int EntitySize { get { return 0; } }
        public abstract int Offset { get; }
        public abstract int Size { get; }
        protected virtual int Link { get { return 0; } }
        protected virtual int Info { get { return 0; } }
        protected virtual int AddrAlign { get { return 0; } }

        //Int32 sh_link, sh_info, sh_addralign, sh_entsize;

        public virtual void WriteHeader(BinaryWriter writer)
        {
            writer.Write((Int32)File.SectionNames.GetStringIndex(Name));
            writer.Write((Int32)Type);
            writer.Write((Int32)Flags);
            writer.Write(RuntimeImageAddress.ToInt32());
            writer.Write((Int32)Offset);
            writer.Write((Int32)Size);
            writer.Write((Int32)Link); // sh_link
            writer.Write((Int32)Info); // sh_info
            writer.Write((Int32)AddrAlign); // sh_addralign
            writer.Write((Int32)EntitySize); // sh_entsize
        }

        public abstract void WriteData(BinaryWriter writer);
    }
}
