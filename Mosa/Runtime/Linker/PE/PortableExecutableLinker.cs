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
        /// Holds the name of the output file.
        /// </summary>
        private string outputFile;

        /// <summary>
        /// Holds the sections of the PE file.
        /// </summary>
        private List<LinkerSection> sections;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="PortableExecutableLinker"/> class.
        /// </summary>
        /// <param name="outputFile">The output file.</param>
        public PortableExecutableLinker(string outputFile)
        {
            if (String.IsNullOrEmpty(outputFile) == true)
                throw new ArgumentException(@"Invalid argument.", @"outputFile");

            this.outputFile = outputFile;
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
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public override string Name
        {
            get { return @"Portable Executable File Linker"; }
        }

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

        #endregion // AssembyLinkerStageBase Overrides
    }
}
