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
