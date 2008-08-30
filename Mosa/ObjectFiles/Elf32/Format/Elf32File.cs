using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mosa.ObjectFiles.Elf32.Format.Sections;

namespace Mosa.ObjectFiles.Elf32.Format
{
    class Elf32File
    {
        public const int EHDR_SIZE = 52;

        public static readonly byte[] MagicBytes = new byte[] { 0x7f, (byte)'E', (byte)'L', (byte)'F' };

        public Elf32File(Elf32MachineKind machineKind)
        {
            this.MachineKind = machineKind;
            this.Sections = new List<Elf32Section>();

            // create default sections
            this.NullSection = new Elf32NullSection(this); // this must be the first defined section
            this.SectionNames = new Elf32StringTableSection(this, ".shstrtab", Elf32SectionFlags.SHF_NONE);
            this.RuntimeData = new Elf32RuntimeDataSection(this);
            this.Code = new Elf32CodeSection(this, ".text", Elf32SectionType.SHT_PROGBITS, Elf32SectionFlags.SHF_ALLOC | Elf32SectionFlags.SHF_EXECINSTR);
            this.Data = new Elf32DataSection(this, ".data", Elf32SectionType.SHT_PROGBITS, Elf32SectionFlags.SHF_ALLOC | Elf32SectionFlags.SHF_WRITE);
            this.ReadOnlyData = new Elf32DataSection(this, ".rodata", Elf32SectionType.SHT_PROGBITS, Elf32SectionFlags.SHF_ALLOC);
            this.SymbolTable = new Elf32SymbolTableSection(this);
            this.SymbolNames = new Elf32StringTableSection(this, ".strtab", Elf32SectionFlags.SHF_NONE);
            SymbolTable.SymbolNames = SymbolNames;

            this.CodeRelocations = new Elf32RelocationSection(
                this,
                ".rel.text"
            ) { SymbolTable = SymbolTable, TargetSection = Code };
        }

        public Elf32MachineKind MachineKind { get; private set; }
        public Elf32NullSection NullSection { get; private set; }
        public Elf32SymbolTableSection SymbolTable { get; private set; }
        public Elf32CodeSection Code { get; private set; }
        public Elf32DataSection Data { get; private set; }
        public Elf32DataSection ReadOnlyData { get; private set; }
        public Elf32RuntimeDataSection RuntimeData { get; private set; }
        public Elf32StringTableSection SectionNames { get; private set; }
        public Elf32StringTableSection SymbolNames { get; private set; }
        public Elf32RelocationSection CodeRelocations { get; private set; }
        public List<Elf32Section> Sections { get; private set; }

        private long ProgramHeaderOffset { get; set; }
        private long SectionHeaderOffset { get; set; }

        public void Write(BinaryWriter writer)
        {
            int headerOffset = (int)writer.Seek(0, SeekOrigin.Current);
            WriteHeader(writer);

            foreach (var section in Sections)
            {
                int index = SectionNames.GetStringIndex(section.Name);
            }

            foreach (var section in Sections)
            {
                section.WriteData(writer);
            }

            this.SectionHeaderOffset = (int)writer.Seek(0, SeekOrigin.Current);

            foreach (var section in Sections)
            {
                section.WriteHeader(writer);
            }

            writer.Seek(headerOffset, SeekOrigin.Begin);
            WriteHeader(writer);
        }

        private void WriteHeader(BinaryWriter writer)
        {
            writer.Write(MagicBytes);
            switch (MachineKind)
            {
                case Elf32MachineKind.I386:
                    writer.Write((byte)Elf32Class.ElfClass32);
                    writer.Write((byte)Elf32DataFormat.ElfData2Lsb);
                    break;
                default:
                    throw new NotSupportedException();
            }
            writer.Write((byte)Elf32Version.Current);
            writer.Write(new byte[16 - 7]);
            writer.Write((short)Elf32ObjectFileType.Relocatable);
            writer.Write((short)MachineKind);
            writer.Write((int)Elf32Version.Current);
            writer.Write((int)0); // e_entry
            writer.Write((int)ProgramHeaderOffset); // e_phoff
            writer.Write((int)SectionHeaderOffset); // e_shoff
            writer.Write((int)0); // e_flags
            writer.Write((short)EHDR_SIZE); // e_ehsize
            writer.Write((short)0); // e_phentsize
            writer.Write((short)0); // e_phnum
            writer.Write((short)Elf32Section.SHDR_SIZE); // e_shentsize
            writer.Write((short)Sections.Count); // e_shnum
            writer.Write((short)Sections.IndexOf(SectionNames)); // e_shstrndx
        }
    }
}
