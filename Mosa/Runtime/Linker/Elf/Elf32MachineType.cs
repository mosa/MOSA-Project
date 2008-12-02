using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Linker.Elf
{
    /// <summary>
    /// 
    /// </summary>
    public enum Elf32MachineType
    {
        /// <summary>
        /// 
        /// </summary>
        NoMachine       = 0x0,
        /// <summary>
        /// 
        /// </summary>
        M32             = 0x1,
        /// <summary>
        /// 
        /// </summary>
        Sparc           = 0x2,
        /// <summary>
        /// 
        /// </summary>
        Intel386        = 0x3,
        /// <summary>
        /// 
        /// </summary>
        Motorola68000   = 0x4,
        /// <summary>
        /// 
        /// </summary>
        Motorola88000   = 0x5,
        /// <summary>
        /// 
        /// </summary>
        Intel80860      = 0x7,
        /// <summary>
        /// 
        /// </summary>
        MipsRS3000      = 0x8,
        /// <summary>
        /// 
        /// </summary>
        MipsRS4000      = 0xA,
    }
}
