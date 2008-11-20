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
using System.Collections.ObjectModel;

namespace Mosa.ObjectFiles.Elf32.Format
{
    /// <summary>
    /// 
    /// </summary>
    class Elf32SymbolCollection : KeyedCollection<object, Elf32Symbol>
    {
        /// <summary>
        /// When implemented in a derived class, extracts the key from the specified element.
        /// </summary>
        /// <param name="item">The element from which to extract the key.</param>
        /// <returns>The key for the specified element.</returns>
        protected override object GetKeyForItem(Elf32Symbol item)
        {
            if (item == null) return null;
            return item.Tag;
        }
    }
}
