/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon "Kintaro" Wollwage (<mailto:kintaro@think-in-co.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Linker.Elf64
{
    /// <summary>
    /// 
    /// </summary>
    public enum Elf64Version : uint
    {
        /// <summary>
        /// Invalid version
        /// </summary>
        None    = 0x00,
        /// <summary>
        /// Currrent version
        /// </summary>
        Current = 0x01,
    }
}
