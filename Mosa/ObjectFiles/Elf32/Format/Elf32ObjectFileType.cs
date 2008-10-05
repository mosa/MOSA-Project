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
    enum Elf32ObjectFileType
    {
        ///<summary>No file type</summary>
        None = 0,
        ///<summary>Relocatable file</summary>
        Relocatable = 1,
        ///<summary>Executable file</summary>
        Executable = 2,
        ///<summary>Shared object file</summary>
        Dynamic = 3,
        ///<summary>Core file</summary>
        Core = 4,
        ///<summary>Processor-specific</summary>
        ET_LOPROC = 0xff00,
        ///<summary>Processor-specific</summary>
        ET_HIPROC = 0xffff,
    }
}
