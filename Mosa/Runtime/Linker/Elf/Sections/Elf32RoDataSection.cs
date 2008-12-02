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
    public class Elf32RoDataSection : Elf32Section
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Elf32RoDataSection"/> class.
        /// </summary>
        public Elf32RoDataSection()
            : base(Mosa.Runtime.Linker.SectionKind.ROData, @".rodata", IntPtr.Zero)
        {
        }
    }
}
