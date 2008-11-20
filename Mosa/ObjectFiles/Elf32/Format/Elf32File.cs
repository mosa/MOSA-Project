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
using System.IO;
using System.Text;
using Mosa.ObjectFiles.Elf32.Format.Sections;

namespace Mosa.ObjectFiles.Elf32.Format
{
    /// <summary>
    /// 
    /// </summary>
    class Elf32File
    {
        /// <summary>
        /// Elf32 Header size
        /// </summary>
        public const int EHDR_SIZE = 52;

        /// <summary>
        /// The "magic" signature containted in an ELF32 file
        /// </summary>
        public static readonly byte[] MagicBytes = new byte[] { 0x7f, (byte)'E', (byte)'L', (byte)'F' };

        #region Properties
        /// <summary>
        /// The machine's type
        /// </summary>
        public Elf32MachineKind MachineKind 
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// The nullsection, this is required!
        /// </summary>
        public Elf32NullSection NullSection 
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// The symbol table
        /// </summary>
        public Elf32SymbolTableSection SymbolTable 
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// The Code section
        /// </summary>
        public Elf32CodeSection Code 
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// The Data section
        /// </summary>
        public Elf32DataSection Data 
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// Read-Only Data section
        /// </summary>
        public Elf32DataSection ReadOnlyData 
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// Runtime Data section
        /// </summary>
        public Elf32RuntimeDataSection RuntimeData 
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// 
        /// </summary>
        public Elf32StringTableSection SectionNames 
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// Symbolnames 
        /// </summary>
        public Elf32StringTableSection SymbolNames 
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// Code relocationss
        /// </summary>
        public Elf32RelocationSection CodeRelocations 
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// List of all contained sections
        /// </summary>
        public List<Elf32Section> Sections 
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// Offset into the program header
        /// </summary>
        private long ProgramHeaderOffset 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Offset into the section header
        /// </summary>
        private long SectionHeaderOffset 
        { 
            get; 
            set;
        }
        #endregion

        #region Construction
        /// <summary>
        /// 
        /// </summary>
        /// <param name="machineKind">The machinetype we want to create an ELF32 binary for</param>
        public Elf32File(Elf32MachineKind machineKind)
        {
            // Save machinetype
            this.MachineKind = machineKind;

            // Create new list of sections
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

            this.CodeRelocations = new Elf32RelocationSection(this, ".rel.text") 
            { 
                SymbolTable = SymbolTable, 
                TargetSection = Code 
            };
        }
        #endregion

        #region Methods
        /// <summary>
        /// Write an ELF32 file to a binary file
        /// </summary>
        /// <param name="writer">A reference to a binary writer that has already a file open</param>
        public void Write(BinaryWriter writer)
        {
            int headerOffset = (int)writer.Seek(0, SeekOrigin.Current);

            // First we need to write the header
            WriteHeader(writer);

            // 
            foreach (Elf32Section section in Sections)
            {
                int index = SectionNames.GetStringIndex(section.Name);
            }

            // Write section's data
            foreach (Elf32Section section in Sections)
            {
                section.WriteData(writer);
            }

            this.SectionHeaderOffset = (int)writer.Seek(0, SeekOrigin.Current);

            // Now we need to write every single section's header
            foreach (Elf32Section section in Sections)
            {
                section.WriteHeader(writer);
            }

            writer.Seek(headerOffset, SeekOrigin.Begin);
            WriteHeader(writer);
        }

        /// <summary>
        /// Write an ELF32's main header
        /// </summary>
        /// <param name="writer"></param>
        private void WriteHeader(BinaryWriter writer)
        {
            // Write the magic signature to the binary to make it recognizable as an ELF32 file
            writer.Write(MagicBytes);

            switch (MachineKind)
            {
                case Elf32MachineKind.I386:
                    writer.Write((byte)Elf32Class.ElfClass32);
                    writer.Write((byte)Elf32DataFormat.ElfData2Lsb);
                    break;
                case Elf32MachineKind.Arm32Le: goto case Elf32MachineKind.I386;
                case Elf32MachineKind.Arm32Be: goto case Elf32MachineKind.I386;

                default:
                    throw new NotSupportedException("Target machine type not supported.");
            }

            // Write the ELF Header according to the specification in
            // the TIS (Tool Interface Standard) ELF (Executable and Linking Format)
            // Specification, 1-4, page 18, figure 1-3

            // Ident, first byte
            writer.Write((byte)Elf32Version.Current);
            // Ident, fill up 9 empty bytes
            writer.Write(new byte[16 - 7]);
            // Ident, remaining 6 bytes
            writer.Write((short)Elf32ObjectFileType.Relocatable);
            // Machinetype ID
            writer.Write((short)((int)MachineKind & 0xffff));
            // Version
            writer.Write((int)Elf32Version.Current);
            // Entry
            writer.Write((int)0);
            // Program header offset
            writer.Write((int)ProgramHeaderOffset);
            // Section header offset
            writer.Write((int)SectionHeaderOffset); 
            // Flags
            writer.Write((int)0); 
            // Elf header size
            writer.Write((short)EHDR_SIZE); 
            // Program header size
            writer.Write((short)0); 
            // Program header number
            writer.Write((short)0); 
            // Section header size
            writer.Write((short)Elf32Section.SHDR_SIZE); 
            // Section header number
            writer.Write((short)Sections.Count); 
            // Section header string index
            writer.Write((short)Sections.IndexOf(SectionNames));
        }
        #endregion
    }
}
