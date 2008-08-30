using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.ObjectFiles.Elf32.Format
{
    enum Elf32Class
    {
        /// <summary>
        /// Invalid class
        /// </summary>
        ElfClassNone = 0,
        /// <summary>
        /// 32-bit objects
        /// </summary>
        ElfClass32 = 1,
        /// <summary>
        /// 64-bit objects
        /// </summary>
        ElfClass64 = 2,
    }
}
