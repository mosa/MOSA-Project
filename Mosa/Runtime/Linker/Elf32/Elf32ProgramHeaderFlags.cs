using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Linker.Elf32
{
    /// <summary>
    /// 
    /// </summary>
    public enum Elf32ProgramHeaderFlags : uint
    {
        /// <summary>
        /// 
        /// </summary>
        Execute     = 0x1,
        /// <summary>
        /// 
        /// </summary>
        Write       = 0x2,
        /// <summary>
        /// 
        /// </summary>
        Read        = 0x4,
        /// <summary>
        /// 
        /// </summary>
        MaskProc    = 0xF0000000,
    }
}
