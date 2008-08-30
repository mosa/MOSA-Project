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
