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

namespace Mosa.Runtime.Linker.Elf
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
            Sections.Elf32Section[] sections  = new Sections.Elf32Section[(int)SectionKind.Max];
            sections[(int)SectionKind.Text]   = new Sections.Elf32CodeSection();
            sections[(int)SectionKind.Data]   = new Sections.Elf32DataSection();
            sections[(int)SectionKind.ROData] = new Sections.Elf32RoDataSection();
            sections[(int)SectionKind.BSS]    = new Sections.Elf32BssSection();
            this.sections.AddRange(sections);
        }

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
        public override void Run(Mosa.Runtime.CompilerFramework.AssemblyCompiler compiler)
        {
            // Resolve all symbols first
            //base.Run(compiler);
          
            // Persist the Elf32 file now
            CreateElf32File();
        }

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public override string Name
        {
            get
            {
                throw new NotImplementedException();
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
            Sections.Elf32Section linkerSection = (Sections.Elf32Section)this.sections[(int)section];
            //return linkerSection.Allocate(size, alignment);
            return System.IO.Stream.Null;
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
            return base.IsResolved(symbol, out address);
        }

        /// <summary>
        /// Creates the elf32 file.
        /// </summary>
        private void CreateElf32File()
        {
            using (System.IO.FileStream fs = new System.IO.FileStream(this.OutputFile, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
            {
                Elf32Header header = new Elf32Header();
                header.Write(fs);

                foreach (Mosa.Runtime.Linker.Elf.Sections.Elf32Section section in Sections)
                    section.Write(fs);
            }
        }
    }
}
