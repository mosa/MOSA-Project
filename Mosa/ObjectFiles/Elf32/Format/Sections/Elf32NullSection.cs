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
    class Elf32NullSection : Elf32ProgbitsSection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Elf32NullSection"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        public Elf32NullSection(Elf32File file)
            : base(file, null, Elf32SectionType.SHT_NULL, Elf32SectionFlags.SHF_NONE)
        {
        }

        /// <summary>
        /// Writes the section's header into the binary file
        /// </summary>
        /// <param name="writer">Reference to the binary writer</param>
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
