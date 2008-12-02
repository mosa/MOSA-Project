using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Linker.Elf.Sections
{
    /// <summary>
    /// 
    /// </summary>
    public class Elf32DataSection : Elf32Section
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Elf32DataSection"/> class.
        /// </summary>
        public Elf32DataSection()
            : base(Mosa.Runtime.Linker.SectionKind.Data, @".data", IntPtr.Zero)
        {
        }
    }
}
