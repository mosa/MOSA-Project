using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Linker.Elf32
{
    /// <summary>
    /// 
    /// </summary>
    public class Elf32ProgramHeader
    {
        /// <summary>
        /// This member tells what kind of segment this array element describes or how to 
        /// interpret the array element's information.
        /// </summary>
        public Elf32ProgramHeaderType Type;
        /// <summary>
        /// This member gives the offset from the beginning of the file at which the first byte 
        /// of the segment resides.
        /// </summary>
        public uint Offset;
        /// <summary>
        /// This member gives the virtual address at which the first byte of the segment resides 
        /// in memory. 
        /// </summary>
        public uint VirtualAddress;
        /// <summary>
        /// On systems for which physical addressing is relevant, this member is reserved for 
        /// the segment's physical address. 
        /// </summary>
        public uint PhysicalAddress;
        /// <summary>
        /// This member gives the number of bytes in the file image of the segment; it may be 
        /// zero.
        /// </summary>
        public uint FileSize;
        /// <summary>
        /// This member gives the number of bytes in the memory image of the segment; it 
        /// may be zero.
        /// </summary>
        public uint MemorySize;
        /// <summary>
        /// This member gives flags relevant to the segment.
        /// </summary>
        public Elf32ProgramHeaderFlags Flags;
        /// <summary>
        /// 
        /// </summary>
        public uint Alignment;
    }
}
