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
using System.Text;
using System.IO;

namespace Mosa.ObjectFiles.Elf32.Format.Sections
{
    /// <summary>
    /// 
    /// </summary>
    abstract class Elf32NobitsSection : Elf32Section
    {
        /// <summary>
        /// This member's value gives the byte offset from the beginning of the file to
        /// the first byte in the section. One section type, SHT_NOBITS described
        /// below, occupies no space in the file, and its sh_offset member locates
        /// the conceptual placement in the file.
        /// </summary>
        int _offset;

        /// <summary>
        /// Initializes a new instance of the <see cref="Elf32NobitsSection"/> class.
        /// </summary>
        /// <param name="file">File to write to</param>
        /// <param name="name">The section's name</param>
        /// <param name="type">Sectiontype</param>
        /// <param name="flags">Flags to use for this section</param>
        public Elf32NobitsSection(Elf32File file, string name, Elf32SectionType type, Elf32SectionFlags flags)
            : base(file, name, type, flags)
        {
            _offset = 0;
        }

        /// <summary>
        /// This member's value gives the byte offset from the beginning of the file to
        /// the first byte in the section. One section type, SHT_NOBITS described
        /// below, occupies no space in the file, and its sh_offset member locates
        /// the conceptual placement in the file.
        /// </summary>
        /// <value></value>
        public sealed override int Offset 
        { 
            get { return _offset; } 
        }

        /// <summary>
        /// Writes the section's data into the binary file
        /// </summary>
        /// <param name="writer">Reference to the binary writer</param>
        public override void WriteData(BinaryWriter writer)
        {
        }
    }
}
