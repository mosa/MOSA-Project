/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon "Kintaro" Wollwage (<mailto:kintaro@think-in-co.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Linker.Elf32.Sections
{
    /// <summary>
    /// 
    /// </summary>
    public class Elf32BssSection : Elf32Section
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Elf32BssSection"/> class.
        /// </summary>
        public Elf32BssSection()
            : base(Mosa.Runtime.Linker.SectionKind.BSS, @".bss", IntPtr.Zero)
        {
            header.Type = Elf32SectionType.NoBits;
            header.Flags = Elf32SectionAttribute.Alloc | Elf32SectionAttribute.Write;
        }
    }
}
