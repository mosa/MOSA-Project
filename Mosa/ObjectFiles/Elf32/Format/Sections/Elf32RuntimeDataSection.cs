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
    class Elf32RuntimeDataSection : Elf32NobitsSection
    {
        /// <summary>
        /// This member gives the section's size in bytes.  Unless the section type is
        /// SHT_NOBITS, the section occupies sh_size bytes in the file. A section
        /// of type SHT_NOBITS may have a non-zero size, but it occupies no space
        /// in the file.
        /// </summary>
        int _size;

        /// <summary>
        /// Initializes a new instance of the <see cref="Elf32RuntimeDataSection"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        public Elf32RuntimeDataSection(Elf32File file)
            : base(file, ".bss", Elf32SectionType.SHT_NOBITS, Elf32SectionFlags.SHF_ALLOC | Elf32SectionFlags.SHF_WRITE)
        {
            _size = 0;
        }

        /// <summary>
        /// This member gives the section's size in bytes.  Unless the section type is
        /// SHT_NOBITS, the section occupies sh_size bytes in the file. A section
        /// of type SHT_NOBITS may have a non-zero size, but it occupies no space
        /// in the file.
        /// </summary>
        /// <value></value>
        public override int Size
        {
            get { return _size; }
        }
    }
}
