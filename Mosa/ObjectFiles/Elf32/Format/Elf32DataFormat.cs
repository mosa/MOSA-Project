using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.ObjectFiles.Elf32.Format
{
    enum Elf32DataFormat
    {
        ///<summary>Invalid data encoding</summary>
        ElfDataNone = 0,
        ///<summary>See below</summary>
        ElfData2Lsb = 1,
        ///<summary>See below</summary>
        ElfData2Msb = 2,
    }
}
