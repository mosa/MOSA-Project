using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Linker.Elf
{
    /// <summary>
    /// 
    /// </summary>
    public enum Elf32FileType
    {
        /// <summary>
        /// 
        /// </summary>
        None        = 0x0000,
        /// <summary>
        /// 
        /// </summary>
        Relocatable = 0x0001,
        /// <summary>
        /// 
        /// </summary>
        Executable  = 0x0002,
        /// <summary>
        /// 
        /// </summary>
        Dynamic     = 0x0003,
        /// <summary>
        /// 
        /// </summary>
        Core        = 0x0004,
        /// <summary>
        /// 
        /// </summary>
        LoProc      = 0xFF00,
        /// <summary>
        /// 
        /// </summary>
        HiProc      = 0xFFFF,
    }
}
