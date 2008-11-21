using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.ObjectFiles.Elf32.Sections
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
        }
    }
}
