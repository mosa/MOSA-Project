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
    /// 
    /// </summary>
    enum Elf32SymbolBinding
    {
        /// <summary>
        /// 
        /// </summary>
        STB_LOCAL = 0,
        /// <summary>
        /// 
        /// </summary>
        STB_GLOBAL = 1,
        /// <summary>
        /// 
        /// </summary>
        STB_WEAK = 2,
        /// <summary>
        /// 
        /// </summary>
        STB_LOPROC = 13,
        /// <summary>
        /// 
        /// </summary>
        STB_HIPROC = 15,
    }
}
