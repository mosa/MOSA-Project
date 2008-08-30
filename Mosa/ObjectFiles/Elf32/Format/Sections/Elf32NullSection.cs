using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Mosa.ObjectFiles.Elf32.Format.Sections
{
    class Elf32NullSection : Elf32ProgbitsSection
    {
        public Elf32NullSection(Elf32File file)
            : base(file, null, Elf32SectionType.SHT_NULL, Elf32SectionFlags.SHF_NONE)
        {
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            // This writes SHDR_SIZE 0 bytes, as per spec.
            writer.Write(new byte[SHDR_SIZE]);
        }

        protected override void WriteDataImpl(BinaryWriter writer)
        {
        }
    }
}
