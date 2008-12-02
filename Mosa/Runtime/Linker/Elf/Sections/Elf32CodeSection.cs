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

namespace Mosa.Runtime.Linker.Elf.Sections
{
    /// <summary>
    /// 
    /// </summary>
    public class Elf32CodeSection : Elf32Section
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Elf32CodeSection"/> class.
        /// </summary>
        public Elf32CodeSection()
            : base(Mosa.Runtime.Linker.SectionKind.Text, @".text", IntPtr.Zero)
        {
            header.Type = Elf32SectionType.ProgBits;
            header.Flags = Elf32SectionAttribute.Alloc | Elf32SectionAttribute.ExecuteInstructions;
        }
    }
}
