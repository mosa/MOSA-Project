/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon "Kintaro" Wollwage (<mailto:kintaro@mosa-project.org>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.ObjectFiles.Elf32.Sections
{
    /// <summary>
    /// 
    /// </summary>
    public class Elf32SectionHeader
    {
        /// <summary>
        /// 
        /// </summary>
        public uint Name;
        /// <summary>
        /// 
        /// </summary>
        public uint Type;
        /// <summary>
        /// 
        /// </summary>
        public uint Flags;
        /// <summary>
        /// 
        /// </summary>
        public uint Address;
        /// <summary>
        /// 
        /// </summary>
        public uint Offset;
        /// <summary>
        /// 
        /// </summary>
        public uint Size;
        /// <summary>
        /// 
        /// </summary>
        public uint Link;
        /// <summary>
        /// 
        /// </summary>
        public uint Info;
        /// <summary>
        /// 
        /// </summary>
        public uint AddressAlignment;
        /// <summary>
        /// 
        /// </summary>
        public uint EntrySize;

        /// <summary>
        /// Writes the specified fs.
        /// </summary>
        /// <param name="fs">The fs.</param>
        public void Write(System.IO.FileStream fs)
        {
            System.IO.BinaryWriter writer = new System.IO.BinaryWriter(fs);
            writer.Write(Name);
            writer.Write(Type);
            writer.Write(Flags);
            writer.Write(Address);
            writer.Write(Offset);
            writer.Write(Size);
            writer.Write(Link);
            writer.Write(Info);
            writer.Write(AddressAlignment);
            writer.Write(EntrySize);
        }
    }
}
