using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.Vm;

namespace Mosa.ObjectFiles.Elf32.Format.Sections
{
    class Elf32CodeSection : Elf32SymbolDefinitionSection
    {
        public Elf32CodeSection(Elf32File file, string name, Elf32SectionType type, Elf32SectionFlags flags)
            : base(file, name, type, flags)
        {
        }
    }
}
