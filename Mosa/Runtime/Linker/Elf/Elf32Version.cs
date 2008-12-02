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
    /// 
    /// </summary>
    public enum Elf32Version
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
