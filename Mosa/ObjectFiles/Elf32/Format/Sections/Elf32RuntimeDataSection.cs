using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Mosa.ObjectFiles.Elf32.Format.Sections
{
    class Elf32RuntimeDataSection : Elf32NobitsSection
    {
        public Elf32RuntimeDataSection(Elf32File file)
            : base(file, ".bss", Elf32SectionType.SHT_NOBITS, Elf32SectionFlags.SHF_ALLOC | Elf32SectionFlags.SHF_WRITE)
        {
        }

        int _size;

        public override int Size
        {
            get { return _size; }
        }
    }
}
