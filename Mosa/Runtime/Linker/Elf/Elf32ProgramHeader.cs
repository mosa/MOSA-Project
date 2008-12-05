using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Linker.Elf
{
    /// <summary>
    /// 
    /// </summary>
    public class Elf32ProgramHeader
    {
        /// <summary>
        /// 
        /// </summary>
        public uint Type;
        /// <summary>
        /// 
        /// </summary>
        public uint Offset;
        /// <summary>
        /// 
        /// </summary>
        public uint VirtualAddress;
        /// <summary>
        /// 
        /// </summary>
        public uint PhysicalAddress;
        /// <summary>
        /// 
        /// </summary>
        public uint FileSize;
        /// <summary>
        /// 
        /// </summary>
        public uint MemorySize;
        /// <summary>
        /// 
        /// </summary>
        public uint Flags;
        /// <summary>
        /// 
        /// </summary>
        public uint Alignment;
    }
}
