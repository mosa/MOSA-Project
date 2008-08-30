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
    enum Elf32SectionType
    {
        ///<summary></summary>
        SHT_NULL = 0,
        ///<summary></summary>
        SHT_PROGBITS = 1,
        ///<summary></summary>
        SHT_SYMTAB = 2,
        ///<summary></summary>
        SHT_STRTAB = 3,
        ///<summary></summary>
        SHT_RELA = 4,
        ///<summary></summary>
        SHT_HASH = 5,
        ///<summary></summary>
        SHT_DYNAMIC = 6,
        ///<summary></summary>
        SHT_NOTE = 7,
        ///<summary></summary>
        SHT_NOBITS = 8,
        ///<summary></summary>
        SHT_REL = 9,
        ///<summary></summary>
        SHT_SHLIB = 10,
        ///<summary></summary>
        SHT_DYNSYM = 11,
        ///<summary>x70000000</summary>
        SHT_LOPROC = 0,
        ///<summary>x7fffffff</summary>
        SHT_HIPROC = 0,
        ///<summary>x80000000</summary>
        SHT_LOUSER = 0,
        ///<summary>xffffffff</summary>
        SHT_HIUSER = 0,
    }
}
