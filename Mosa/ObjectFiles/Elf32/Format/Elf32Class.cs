/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.ObjectFiles.Elf32.Format
{
    /// <summary>
    /// The file format is designed to be portable among machines of various sizes, without 
    /// imposing the sizes of the largest machine on the smallest.
    /// 
    /// Therefore, Elf32Class identifies the file's class and/or capacity
    /// </summary>
    enum Elf32Class
    {
        /// <summary>
        /// Invalid class
        /// </summary>
        ElfClassNone    = 0x0,
        /// <summary>
        /// ElfClass32 supports machines with files and virtual address 
        /// spaces up to 4 gigabytes; it uses the basic types defined above. 
        /// </summary>
        ElfClass32      = 0x1,
        /// <summary>
        /// ElfClass64 is incomplete and refers to the 64-bit architectures. Its 
        /// appearance here shows how the object file may change. Other classes will be defined 
        /// as necessary, with different basic types and sizes for object file data. 
        /// </summary>
        ElfClass64      = 0x2,
    }
}
