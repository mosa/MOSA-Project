/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.IO;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.Linker.PE
{
    /// <summary>
    /// A Linker, which creates portable executable files.
    /// </summary>
    public sealed class PortableExecutableLinker : AssemblyLinkerStageBase
    {
        #region Data members

        /// <summary>
        /// Holds the sections of the PE file.
        /// </summary>
        private List<LinkerSection> sections;

        /// <summary>
        /// Flag, if the symbols have been resolved.
        /// </summary>
        private bool symbolsResolved;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="PortableExecutableLinker"/> class.
        /// </summary>
        public PortableExecutableLinker()
        {
            this.sections = new List<LinkerSection>();

            // Create the default section set
            LinkerSection[] sections = new LinkerSection[(int)SectionKind.Max];
            sections[(int)SectionKind.Text] = new PortableExecutableLinkerSection(SectionKind.Text, @".text", IntPtr.Zero);
            sections[(int)SectionKind.Data] = new PortableExecutableLinkerSection(SectionKind.Data, @".data", IntPtr.Zero);
            sections[(int)SectionKind.ROData] = new PortableExecutableLinkerSection(SectionKind.ROData, @".rodata", IntPtr.Zero);
            sections[(int)SectionKind.BSS] = new PortableExecutableLinkerSection(SectionKind.BSS, @".bss", IntPtr.Zero);
            this.sections.AddRange(sections);
        }

        #endregion // Construction

        #region AssembyLinkerStageBase Overrides

        /// <summary>
        /// A request to patch already emitted code by storing the calculated address value.
        /// </summary>
        /// <param name="linkType">Type of the link.</param>
        /// <param name="method">The method whose code is being patched.</param>
        /// <param name="methodOffset">The value to store at the position in code.</param>
        /// <param name="methodRelativeBase">The method relative base.</param>
        /// <param name="targetAddress">The position in code, where it should be patched.</param>
        protected override void ApplyPatch(LinkType linkType, RuntimeMethod method, long methodOffset, long methodRelativeBase, long targetAddress)
        {
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
            return (this.symbolsResolved == true && base.IsResolved(symbol, out address) == true);
        }

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public override string Name
        {
            get { return @"Portable Executable File Linker"; }
        }

        /// <summary>
        /// Retrieves the collection of sections created during compilation.
        /// </summary>
        /// <value>The sections collection.</value>
        public override ICollection<LinkerSection> Sections
        {
            get { return this.sections; }
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
        protected override Stream Allocate(SectionKind section, int size, int alignment)
        {
            PortableExecutableLinkerSection linkerSection = (PortableExecutableLinkerSection)this.sections[(int)section];
            return linkerSection.Allocate(size, alignment);
        }

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
        public override void Run(AssemblyCompiler compiler)
        {
            if (String.IsNullOrEmpty(this.OutputFile) == true)
                throw new InvalidOperationException(@"Invalid output file specification.");

            // Layout the sections in memory
            LayoutSections();
            
            // Resolve all symbols first
            base.Run(compiler);

            // Persist the PE file now
            CreatePEFile();
        }

        #endregion // AssembyLinkerStageBase Overrides

        #region Internals

        /// <summary>
        /// Creates the PE file.
        /// </summary>
        private void CreatePEFile()
        {
            using (FileStream fs = new FileStream(this.OutputFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                
            }
        }

        /// <summary>
        /// Adjusts the section addresses and performs a proper layout.
        /// </summary>
        private void LayoutSections()
        {
            // Take the base address of the image
            long address = this.BaseAddress;

            // Add the PE header size to the address
            address += 0;

            // Move all sections to their right positions
            foreach (LinkerSection ls in this.sections)
            {
                // Set the section address
                ls.Address = new IntPtr(address);

                // Calculate the address of the next section
                address += ls.Length;
            }

            // We've resolved all symbols, allow IsResolved to succeed
            this.symbolsResolved = true;
        }

        #endregion // Internals
    }
}
