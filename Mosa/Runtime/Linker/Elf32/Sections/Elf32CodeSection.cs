/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon "Kintaro" Wollwage (<mailto:kintaro@think-in-co.de>)
 */

using System;

namespace Mosa.Runtime.Linker.Elf32.Sections
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
            : base(SectionKind.Text, @".text", IntPtr.Zero)
        {
            _header.Type = Elf32SectionType.ProgBits;
            _header.Flags = Elf32SectionAttribute.AllocExecute;
        }
    }
}
