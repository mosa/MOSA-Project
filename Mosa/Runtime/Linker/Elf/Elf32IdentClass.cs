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

namespace Mosa.Runtime.Linker.Elf
{
    /// <summary>
    /// Identifies the file's class, or capacity.
    /// </summary>
    public enum Elf32IdentClass
    {
        /// <summary>
        /// Invalid class
        /// </summary>
        ClassNone   = 0x00,
        /// <summary>
        /// 32-bit objects
        /// </summary>
        Class32     = 0x01,
        /// <summary>
        /// 64-bit objects
        /// </summary>
        Class64     = 0x02,
    }
}
