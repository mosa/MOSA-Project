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
        }
    }
}
