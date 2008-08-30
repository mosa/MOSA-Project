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
    enum Elf32DataFormat
    {
        ///<summary>Invalid data encoding</summary>
        ElfDataNone = 0,
        ///<summary>See below</summary>
        ElfData2Lsb = 1,
        ///<summary>See below</summary>
        ElfData2Msb = 2,
    }
}
