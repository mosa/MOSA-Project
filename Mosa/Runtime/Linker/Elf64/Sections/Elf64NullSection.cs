/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Linker.Elf64.Sections
{
    /// <summary>
    /// 
    /// </summary>
    public class Elf64NullSection : Elf64Section
    {
        /// <summary>
        /// Gets the length of the section in bytes.
        /// </summary>
        /// <value>The length of the section in bytes.</value>
        public override long Length
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Elf64CodeSection"/> class.
        /// </summary>
        public Elf64NullSection()
            : base(Mosa.Runtime.Linker.SectionKind.Text, @"", IntPtr.Zero)
        {
            header.Name = 0;
            header.Type = Elf64SectionType.Null;
            header.Flags = (Elf64SectionAttribute)0;
            header.Address = 0;
            header.Offset = 0;
            header.Size = 0;
            header.Link = 0;
            header.Info = 0;
            header.AddressAlignment = 0;
            header.EntrySize = 0;
        }

        /// <summary>
        /// Writes the specified fs.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public override void Write(System.IO.BinaryWriter writer)
        {
        }
    }
}
