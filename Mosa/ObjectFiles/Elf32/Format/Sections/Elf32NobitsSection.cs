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
using System.IO;

namespace Mosa.ObjectFiles.Elf32.Format.Sections
{
    /// <summary>
    /// 
    /// </summary>
    abstract class Elf32NobitsSection : Elf32Section
    {
        /// <summary>
        /// 
        /// </summary>
        int _offset;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="flags"></param>
        public Elf32NobitsSection(Elf32File file, string name, Elf32SectionType type, Elf32SectionFlags flags)
            : base(file, name, type, flags)
        {
            _offset = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed override int Offset 
        { 
            get { return _offset; } 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteData(BinaryWriter writer)
        {
        }
    }
}
