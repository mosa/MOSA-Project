using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Linker.Elf32
{
    /// <summary>
    /// 
    /// </summary>
    public enum Elf32ProgramHeaderType : uint
    {
        /// <summary>
        /// 
        /// </summary>
        Null            = 0x00,
        /// <summary>
        /// 
        /// </summary>
        Load            = 0x01,
        /// <summary>
        /// 
        /// </summary>
        Dynamic         = 0x02,
        /// <summary>
        /// 
        /// </summary>
        Interpreter     = 0x03,
        /// <summary>
        /// 
        /// </summary>
        Note            = 0x04,
        /// <summary>
        /// 
        /// </summary>
        Shlib           = 0x05,
        /// <summary>
        /// 
        /// </summary>
        ProgramHeader   = 0x06,
        /// <summary>
        /// 
        /// </summary>
        LoProc          = 0x70000000,
        /// <summary>
        /// 
        /// </summary>
        HiPrc           = 0x7FFFFFFF,
    }
}
