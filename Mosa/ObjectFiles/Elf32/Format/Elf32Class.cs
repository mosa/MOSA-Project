/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the GNU GPL v3, with Classpath Linking Exception
 * Licensed under the terms of the New BSD License for exclusive use by the Ensemble OS Project
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.ObjectFiles.Elf32.Format
{
    enum Elf32Class
    {
        /// <summary>
        /// Invalid class
        /// </summary>
        ElfClassNone = 0,
        /// <summary>
        /// 32-bit objects
        /// </summary>
        ElfClass32 = 1,
        /// <summary>
        /// 64-bit objects
        /// </summary>
        ElfClass64 = 2,
    }
}
