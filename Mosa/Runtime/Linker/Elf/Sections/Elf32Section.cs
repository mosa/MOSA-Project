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

namespace Mosa.Runtime.Linker.Elf.Sections
{
    /// <summary>
    /// 
    /// </summary>
    public class Elf32Section : Mosa.Runtime.Linker.LinkerSection
    {
        /// <summary>
        /// 
        /// </summary>
        protected Elf32SectionHeader header = new Elf32SectionHeader();
        /// <summary>
        /// 
        /// </summary>
        protected long length = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Elf32Section"/> class.
        /// </summary>
        /// <param name="kind">The kind of the section.</param>
        /// <param name="name">The name.</param>
        /// <param name="address">The address.</param>
        public Elf32Section(Mosa.Runtime.Linker.SectionKind kind, string name, IntPtr address) 
            : base(kind, name, address)
        {
            header = new Elf32SectionHeader();
            length = 50;
        }

        /// <summary>
        /// Gets the length of the section in bytes.
        /// </summary>
        /// <value>The length of the section in bytes.</value>
        public override long Length
        {
            get 
            {
                return length;
            }
        }

        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <value>The header.</value>
        public Elf32SectionHeader Header
        {
            get
            {
                return header;
            }
        }

        /// <summary>
        /// Writes the specified fs.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void Write(System.IO.BinaryWriter writer)
        {
            Header.Offset = (uint)writer.BaseStream.Position;
            Random rnd = new Random();
            byte[] data = new byte[length];
            rnd.NextBytes(data);
            writer.Write(data);
        }

        /// <summary>
        /// Writes the header.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void WriteHeader(System.IO.BinaryWriter writer)
        {
            Header.Size = (uint)Length;
            Header.Write(writer);
        }
    }
}
