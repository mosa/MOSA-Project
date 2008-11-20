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
    /// A symbol's type provides a general classification for the associated entity. 
    /// </summary>
    enum Elf32SymbolType
    {
        /// <summary>
        /// The symbol's type is not specified. 
        /// </summary>
        STT_NOTYPE = 0,

        /// <summary>
        /// The symbol is associated with a data object, such as a variable, an array, 
        /// and so on.
        /// </summary>
        STT_OBJECT = 1,

        /// <summary>
        /// The symbol is associated with a function or other executable code.
        /// </summary>
        STT_FUNC = 2,

        /// <summary>
        /// The symbol is associated with a section. Symbol table entries of this type 
        /// exist primarily for relocation and normally have STB_LOCAL binding.
        /// </summary>
        STT_SECTION = 3,

        /// <summary>
        /// A file symbol has STB_LOCAL binding, its section index is SHN_ABS, and 
        /// it precedes the other STB_LOCAL symbols for the file, if it is present.
        /// </summary>
        STT_FILE = 4,

        /// <summary>
        /// Values in this inclusive range are reserved for processor-specific semantics.
        /// 
        /// If a symbol's value refers to a specific location within a section, its section 
        /// index member, st_shndx, holds an index into the section header table. 
        /// As the section moves during relocation, the symbol's value changes as well, 
        /// and references to the symbol continue to "point'' to the same location in the 
        /// program. Some special section index values give other semantics.
        /// </summary>
        STT_LOPROC = 13,

        /// <summary>
        /// Values in this inclusive range are reserved for processor-specific semantics.
        /// 
        /// If a symbol's value refers to a specific location within a section, its section 
        /// index member, st_shndx, holds an index into the section header table. 
        /// As the section moves during relocation, the symbol's value changes as well, 
        /// and references to the symbol continue to "point'' to the same location in the 
        /// program. Some special section index values give other semantics.
        /// </summary>
        STT_HIPROC = 15,
    }
}
