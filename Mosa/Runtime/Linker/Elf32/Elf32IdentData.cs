/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon "Kintaro" Wollwage (<mailto:kintaro@mosa-project.org>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Linker.Elf32
{
    /// <summary>
    /// Specifies the data encoding of the 
    /// processor-specific data in the object file. 
    /// </summary>
    public enum Elf32IdentData : byte
    {
        /// <summary>
        /// Invalid data encoding
        /// </summary>
        DataNone    = 0x00,
        /// <summary>
        /// Encoding Data2LSB specifies 2's complement values, with the least significant byte 
        /// occupying the lowest address. 
        /// </summary>
        Data2LSB    = 0x01,
        /// <summary>
        /// Encoding Data2MSB specifies 2's complement values, with the most significant byte 
        /// occupying the lowest address. 
        /// </summary>
        Data2MSB    = 0x02,
    }
}
