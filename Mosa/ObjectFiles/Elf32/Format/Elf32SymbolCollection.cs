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
using System.Collections.ObjectModel;

namespace Mosa.ObjectFiles.Elf32.Format
{
    class Elf32SymbolCollection : KeyedCollection<object, Elf32Symbol>
    {
        protected override object GetKeyForItem(Elf32Symbol item)
        {
            if (item == null) return null;
            return item.Tag;
        }
    }
}
