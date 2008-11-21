using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.ObjectFiles.Elf32.Sections
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
