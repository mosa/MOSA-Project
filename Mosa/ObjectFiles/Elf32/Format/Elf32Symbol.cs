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
using Mosa.ObjectFiles.Elf32.Format.Sections;
using System.IO;

namespace Mosa.ObjectFiles.Elf32.Format
{
    /// <summary>
    /// 
    /// </summary>
    class Elf32Symbol
    {
        /// <summary>
        /// 
        /// </summary>
        public const int SYM_SIZE = 16;

        /// <summary>
        /// Initializes a new instance of the <see cref="Elf32Symbol"/> class.
        /// </summary>
        /// <param name="tag">The tag.</param>
        public Elf32Symbol(object tag)
        {
            this.Tag = tag;
            Offset = -1;
            Size = -1;
        }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>The tag.</value>
        public object Tag { get; private set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        /// <value>The offset.</value>
        public int Offset { get; private set; }
        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public int Size { get; private set; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public Elf32SymbolType Type { get; set; }
        /// <summary>
        /// Gets or sets the bind.
        /// </summary>
        /// <value>The bind.</value>
        public Elf32SymbolBinding Bind { get; set; }
        /// <summary>
        /// Gets or sets the section.
        /// </summary>
        /// <value>The section.</value>
        public Elf32SymbolDefinitionSection Section { get; private set; }

        /// <summary>
        /// Writes the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="symbolNames">The symbol names.</param>
        /// <param name="writer">The writer.</param>
        public void Write(Elf32File file, Elf32StringTableSection symbolNames, BinaryWriter writer)
        {
            if (IsDefined)
            {
                writer.Write((int)symbolNames.GetStringIndex(Name)); // st_name
                writer.Write((int)Offset); // st_value
                writer.Write((int)Size); // st_size
                writer.Write((byte)(((byte)Bind << 4) | ((byte)Type & 0xf))); // st_info
                writer.Write((byte)0); // st_other
                writer.Write((short)file.Sections.IndexOf(Section)); // st_shndx
            }
            else
            {
                writer.Write((int)symbolNames.GetStringIndex(Name)); // st_name
                writer.Write((int)0); // st_value
                writer.Write((int)0); // st_size
                writer.Write((byte)(((byte)Bind << 4) | ((byte)Type & 0xf))); // st_info
                writer.Write((byte)0); // st_other
                writer.Write((short)0); // st_shndx
            }
        }

        /// <summary>
        /// Begins the specified section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="offset">The offset.</param>
        public void Begin(Elf32SymbolDefinitionSection section, int offset)
        {
            Section = section;
            Offset = offset;
        }
        /// <summary>
        /// Ends the specified size.
        /// </summary>
        /// <param name="size">The size.</param>
        public void End(int size)
        {
            Size = size;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is defined.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is defined; otherwise, <c>false</c>.
        /// </value>
        public bool IsDefined
        {
            get { return Section != null && Offset >= 0; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is finished.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is finished; otherwise, <c>false</c>.
        /// </value>
        public bool IsFinished
        {
            get { return Section != null && Offset >= 0 && Size >= 0; }
        }
    }
}
