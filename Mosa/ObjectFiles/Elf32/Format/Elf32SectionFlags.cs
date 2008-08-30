using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.ObjectFiles.Elf32.Format
{
    enum Elf32SectionFlags : uint
    {
        /// <summary></summary>
        SHF_NONE = 0x0,
        ///<summary></summary>
        SHF_WRITE = 0x1,
        ///<summary></summary>
        SHF_ALLOC = 0x2,
        ///<summary></summary>
        SHF_EXECINSTR = 0x4,
        ///<summary></summary>
        SHF_MASKPROC = 0xf0000000,
    }
}
