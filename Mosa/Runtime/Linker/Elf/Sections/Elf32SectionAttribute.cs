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
    /// Describes miscellaneous attributes for a section.
    /// </summary>
    public enum Elf32SectionAttribute : uint
    {
        /// <summary>
        /// The section contains data that should be writable during process execution.
        /// </summary>
        Write               = 0x00000001,
        /// <summary>
        /// The section occupies memory during process execution. Some control 
        /// sections do not reside in the memory image of an object file; this attribute 
        /// is off for those sections. 
        /// </summary>
        Alloc               = 0x00000002,
        /// <summary>
        /// The section contains executable machine instructions. 
        /// </summary>
        ExecuteInstructions = 0x00000004,
        /// <summary>
        /// 
        /// </summary>
        AllocExecute        = 0x00000006,
        /// <summary>
        /// All bits included in this mask are reserved for processor-specific semantics. 
        /// </summary>
        ProcessorMask       = 0xF0000000,
    }
}
