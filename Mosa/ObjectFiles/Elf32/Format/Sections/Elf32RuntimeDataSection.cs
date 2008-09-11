/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the GNU GPL v3, with Classpath Linking Exception
 * Licensed under the terms of the New BSD License for exclusive use by the Ensemble OS Project
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
        /// 
        /// </summary>
        int _size;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        public Elf32RuntimeDataSection(Elf32File file)
            : base(file, ".bss", Elf32SectionType.SHT_NOBITS, Elf32SectionFlags.SHF_ALLOC | Elf32SectionFlags.SHF_WRITE)
        {
            _size = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public override int Size
        {
            get { return _size; }
        }
    }
}
