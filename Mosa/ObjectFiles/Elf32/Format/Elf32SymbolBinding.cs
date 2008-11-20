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
    /// A symbol's binding determines the linkage visibility and behavior.
    /// In each symbol table, all symbols with STB_LOCAL binding precede the weak and global 
    /// symbols. A symbol's type provides a general classification for the associated entity. 
    /// </summary>
    enum Elf32SymbolBinding
    {
        /// <summary>
        /// Local symbols are not visible outside the object file containing their 
        /// definition. Local symbols of the same name may exist in multiple files 
        /// without interfering with each other.
        /// </summary>
        STB_LOCAL = 0,

        /// <summary>
        /// Global symbols are visible to all object files being combined. One file's 
        /// definition of a global symbol will satisfy another file's undefined reference 
        /// to the same global symbol. 
        /// </summary>
        STB_GLOBAL = 1,

        /// <summary>
        /// Weak symbols resemble global symbols, but their definitions have lower 
        /// precedence. 
        /// </summary>
        STB_WEAK = 2,

        /// <summary>
        /// Values in this inclusive range are reserved for processor-specific semantics.
        /// </summary>
        STB_LOPROC = 13,

        /// <summary>
        /// Values in this inclusive range are reserved for processor-specific semantics.
        /// </summary>
        STB_HIPROC = 15,
    }
}
