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

namespace Mosa.ObjectFiles.Elf32.Format.Sections
{
    abstract class Elf32NobitsSection : Elf32Section
    {
        int _offset;

        public Elf32NobitsSection(Elf32File file, string name, Elf32SectionType type, Elf32SectionFlags flags)
            : base(file, name, type, flags)
        {
        }

        public sealed override int Offset { get { return _offset; } }

        public override void WriteData(BinaryWriter writer)
        {
        }
    }
}
