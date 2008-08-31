/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the GNU GPL v3, with Classpath Linking Exception
 * Licensed under the terms of the New BSD License for exclusive use by the Ensemble OS Project
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.ObjectFiles.Elf32
{
    /// <summary>
    /// This enum is defined by the ELF32 specification as a 16-bit enum,
    /// But the higher bits are used internally for differentiating between
    /// different architectures that use the same value here.
    /// </summary>
    public enum Elf32MachineKind : uint
    {
        ///<summary>No machine</summary>
        None = 0,
        ///<summary>AT&amp;T WE 32100</summary>
        M32 = 1,
        ///<summary>SPARC</summary>
        Sparc = 2,
        ///<summary>Intel Architecture</summary>
        I386 = 3,
        ///<summary>Motorola 68000</summary>
        M68k = 4,
        ///<summary>Motorola 88000</summary>
        M88k = 5,
        ///<summary>Intel 80860</summary>
        I860 = 7,
        ///<summary>MIPS RS3000 Big-Endian</summary>
        MipsRS3Be = 8,
        ///<summary>MIPS RS4000 Big-Endian</summary>
        MipsRS4Be = 10,
        /// <summary>
        /// ARM32 Little-Endian
        /// </summary>
        Arm32Le = 40,
        /// <summary>
        /// ARM32 Big-Endian
        /// </summary>
        Arm32Be = 0x80000000 | 40,
    }
}
