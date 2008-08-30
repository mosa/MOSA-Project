using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.ObjectFiles.Elf32
{
    public enum Elf32MachineKind
    {
        ///<summary>No machine</summary>
        NONE = 0,
        ///<summary>AT&amp;T WE 32100</summary>
        M32 = 1,
        ///<summary>SPARC</summary>
        SPARC = 2,
        ///<summary>Intel Architecture</summary>
        I386 = 3,
        ///<summary>Motorola 68000</summary>
        M68K = 4,
        ///<summary>Motorola 88000</summary>
        M88K = 5,
        ///<summary>Intel 80860</summary>
        I860 = 7,
        ///<summary>MIPS RS3000 Big-Endian</summary>
        MIPS = 8,
        ///<summary>MIPS RS4000 Big-Endian</summary>
        MIPS_RS4_BE = 10,
    }
}
