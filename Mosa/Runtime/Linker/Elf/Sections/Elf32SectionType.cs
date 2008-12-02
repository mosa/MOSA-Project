using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Linker.Elf.Sections
{
    /// <summary>
    /// 
    /// </summary>
    public enum Elf32SectionType : uint
    {
        /// <summary>
        /// 
        /// </summary>
        Null                = 0x00,
        /// <summary>
        /// 
        /// </summary>
        ProgBits            = 0x01,
        /// <summary>
        /// 
        /// </summary>
        SymbolTable         = 0x02,
        /// <summary>
        /// 
        /// </summary>
        StringTable         = 0x03,
        /// <summary>
        /// 
        /// </summary>
        RelocationA         = 0x04,
        /// <summary>
        /// 
        /// </summary>
        HashTable           = 0x05,
        /// <summary>
        /// 
        /// </summary>
        Dynamic             = 0x06,
        /// <summary>
        /// 
        /// </summary>
        Note                = 0x07,
        /// <summary>
        /// 
        /// </summary>
        NoBits              = 0x08,
        /// <summary>
        /// 
        /// </summary>
        Relocation          = 0x09,
        /// <summary>
        /// 
        /// </summary>
        SectionLib          = 0x0A,
        /// <summary>
        /// 
        /// </summary>
        DynamicSymbolTable  = 0x0B,
        /// <summary>
        /// 
        /// </summary>
        LoProc              = 0x70000000,
        /// <summary>
        /// 
        /// </summary>
        HiProc              = 0x7FFFFFFF,
        /// <summary>
        /// 
        /// </summary>
        LoUser              = 0x80000000,
        /// <summary>
        /// 
        /// </summary>
        HiUser              = 0xFFFFFFFF,
    }
}
