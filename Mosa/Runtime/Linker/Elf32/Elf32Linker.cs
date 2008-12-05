/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon "Kintaro" Wollwage (<mailto:kintaro@mosa-project.org>)
 */

using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.Linker;

namespace Mosa.Runtime.Linker.Elf32
{
    /// <summary>
    /// 
    /// </summary>
    public class Elf32Linker : Mosa.Runtime.Linker.AssemblyLinkerStageBase
    {
        /// <summary>
        /// 
        /// </summary>
        private List<Mosa.Runtime.Linker.LinkerSection> sections;
        /// <summary>
        /// 
        /// </summary>
        private Elf32.Sections.Elf32NullSection nullSection;
        /// <summary>
        /// 
        /// </summary>
        private Elf32.Sections.Elf32StringTableSection stringTableSection;

        /// <summary>
        /// Retrieves the collection of sections created during compilation.
        /// </summary>
        /// <value>The sections collection.</value>
        public override ICollection<Mosa.Runtime.Linker.LinkerSection> Sections
        {
            get
            {
                return this.sections;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Elf32Linker"/> class.
        /// </summary>
        public Elf32Linker()
        {
            this.sections = new List<LinkerSection>();

            // Create the default section set
            Elf32.Sections.Elf32Section[] sections = new Sections.Elf32Section[(int)SectionKind.Max];
            sections[(int)SectionKind.Text]   = new Sections.Elf32CodeSection();
            sections[(int)SectionKind.Data]   = new Sections.Elf32DataSection();
            sections[(int)SectionKind.ROData] = new Sections.Elf32RoDataSection();
            sections[(int)SectionKind.BSS]    = new Sections.Elf32BssSection();
            this.sections.AddRange(sections);

            nullSection = new Mosa.Runtime.Linker.Elf32.Sections.Elf32NullSection();
            stringTableSection = new Mosa.Runtime.Linker.Elf32.Sections.Elf32StringTableSection();
        }

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
        public override void Run(Mosa.Runtime.CompilerFramework.AssemblyCompiler compiler)
        {
            // Resolve all symbols first
            base.Run(compiler);
          
            // Persist the Elf32 file now
            CreateElf32File(compiler);
        }

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public override string Name
        {
            get
            {
                return @"Executable and Linking Format (ELF) Linker";
            }
        }

        /// <summary>
        /// Special resolution for internal calls.
        /// </summary>
        /// <param name="method">The internal call method to resolve.</param>
        /// <returns>The address</returns>
        protected override long ResolveInternalCall(Mosa.Runtime.Vm.RuntimeMethod method)
        {
            return base.ResolveInternalCall(method);
        }

        /// <summary>
        /// Allocates a symbol of the given name in the specified section.
        /// </summary>
        /// <param name="section">The executable section to allocate from.</param>
        /// <param name="size">The number of bytes to allocate. If zero, indicates an unknown amount of memory is required.</param>
        /// <param name="alignment">The alignment. A value of zero indicates the use of a default alignment for the section.</param>
        /// <returns>
        /// A stream, which can be used to populate the section.
        /// </returns>
        protected override System.IO.Stream Allocate(SectionKind section, int size, int alignment)
        {
            Elf32.Sections.Elf32Section linkerSection = (Elf32.Sections.Elf32Section)GetSection(section);
            return linkerSection.Allocate(size, alignment);
        }

        /// <summary>
        /// A request to patch already emitted code by storing the calculated address value.
        /// </summary>
        /// <param name="linkType">Type of the link.</param>
        /// <param name="methodAddress">The virtual address of the method whose code is being patched.</param>
        /// <param name="methodOffset">The value to store at the position in code.</param>
        /// <param name="methodRelativeBase">The method relative base.</param>
        /// <param name="targetAddress">The position in code, where it should be patched.</param>
        protected override void ApplyPatch(LinkType linkType, long methodAddress, long methodOffset, long methodRelativeBase, long targetAddress)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves a linker section by its type.
        /// </summary>
        /// <param name="sectionKind">The type of the section to retrieve.</param>
        /// <returns>The retrieved linker section.</returns>
        protected override LinkerSection GetSection(SectionKind sectionKind)
        {
            return this.sections[(int)sectionKind];
        }

        /// <summary>
        /// Determines whether the specified symbol is resolved.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="address">The address.</param>
        /// <returns>
        /// 	<c>true</c> if the specified symbol is resolved; otherwise, <c>false</c>.
        /// </returns>
        protected override bool IsResolved(string symbol, out long address)
        {
            address = 0;
            return true;
            //return base.IsResolved(symbol, out address);*/
        }

        /// <summary>
        /// Creates the elf32 file.
        /// </summary>
        /// <param name="compiler">The compiler.</param>
        private void CreateElf32File(Mosa.Runtime.CompilerFramework.AssemblyCompiler compiler)
        {
            using (System.IO.FileStream fs = new System.IO.FileStream(this.OutputFile, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
            {
                Elf32Header header = new Elf32Header();
                header.Type = Elf32FileType.Executable;
                header.Machine = Elf32MachineType.Intel386;
                header.SectionHeaderNumber = (ushort)(Sections.Count + 2);
                header.SectionHeaderOffset = header.ElfHeaderSize;

                header.CreateIdent(Elf32IdentClass.Class32, Elf32IdentData.Data2LSB, null);

                // Calculate the concatenated size of all section's data
                uint offset = 0;
                foreach (Mosa.Runtime.Linker.Elf32.Sections.Elf32Section section in Sections)
                {
                    offset += (uint)section.Length;
                }
                offset += (uint)nullSection.Length;
                offset += (uint)stringTableSection.Length;

                // Calculate offsets
                header.ProgramHeaderOffset = (uint)header.ElfHeaderSize + (uint)header.SectionHeaderEntrySize * (uint)header.SectionHeaderNumber + offset;
                header.ProgramHeaderNumber = 1;
                header.SectionHeaderStringIndex = 1;

                System.IO.BinaryWriter writer = new System.IO.BinaryWriter(fs);

                // Write the ELF Header
                header.Write(writer);

                // Overjump the Section Header Table and write the section's data first
                long tmp = fs.Position;
                writer.Seek((int)(tmp + header.SectionHeaderNumber * header.SectionHeaderEntrySize), System.IO.SeekOrigin.Begin);

                nullSection.Write(writer);
                stringTableSection.Write(writer);

                // Write the sections
                foreach (Mosa.Runtime.Linker.Elf32.Sections.Elf32Section section in Sections)
                    section.Write(writer);

                // Jump back to the Section Header Table
                writer.Seek((int)tmp, System.IO.SeekOrigin.Begin);

                nullSection.WriteHeader(writer);
                stringTableSection.WriteHeader(writer);

                // Write the section headers
                foreach (Mosa.Runtime.Linker.Elf32.Sections.Elf32Section section in Sections)
                    section.WriteHeader(writer);

                Elf32ProgramHeader pheader = new Elf32ProgramHeader();
                pheader.Alignment = 0;
                pheader.FileSize = (uint)(GetSection(SectionKind.Text) as Elf32.Sections.Elf32Section).Length;
                pheader.MemorySize = pheader.FileSize;
                pheader.VirtualAddress = 0xFF0000;
                pheader.Flags = Elf32ProgramHeaderFlags.Execute | Elf32ProgramHeaderFlags.Read | Elf32ProgramHeaderFlags.Write;
                pheader.Offset = (GetSection(SectionKind.Text) as Elf32.Sections.Elf32Section).Header.Offset;
                pheader.Type = Elf32ProgramHeaderType.Load;

                writer.Seek((int)header.ProgramHeaderOffset, System.IO.SeekOrigin.Begin);
                pheader.Write(writer);
            }
        }
    }
}
