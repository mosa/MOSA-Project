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

namespace Mosa.Runtime.Linker.Elf.Sections
{
    /// <summary>
    /// 
    /// </summary>
    public enum Elf32SectionAttribute : uint
    {
        /// <summary>
        /// 
        /// </summary>
        Write               = 0x00000001,
        /// <summary>
        /// 
        /// </summary>
        Alloc               = 0x00000002,
        /// <summary>
        /// 
        /// </summary>
        ExecuteInstructions = 0x00000004,
        /// <summary>
        /// 
        /// </summary>
        ProcessorMaks       = 0xF0000000,
    }
}
