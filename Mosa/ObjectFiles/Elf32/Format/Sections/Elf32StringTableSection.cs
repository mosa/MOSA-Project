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
using System.IO;

namespace Mosa.ObjectFiles.Elf32.Format.Sections
{
    /// <summary>
    /// This section holds strings, most commonly the strings that represent the names 
    /// associated with symbol table entries. If a file has a loadable segment that 
    /// includes the symbol string table, the section's attributes will include the 
    /// SHF_ALLOC bit; otherwise, that bit will be off. 
    /// 
    /// Everything according to the specification in
    /// the TIS (Tool Interface Standard) ELF (Executable and Linking Format)
    /// Specification, 1-4, page 30.
    /// 
    /// Section's ELF Name: ".strtab"
    /// </summary>
    class Elf32StringTableSection : Elf32ProgbitsSection
    {
        /// <summary>
        /// 
        /// </summary>
        MemoryStream _stream;

        /// <summary>
        /// 
        /// </summary>
        StreamWriter _writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Elf32StringTableSection"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="name">The name.</param>
        /// <param name="flags">The flags.</param>
        public Elf32StringTableSection(Elf32File file, string name, Elf32SectionFlags flags)
            : base(file, name, Elf32SectionType.SHT_STRTAB, flags)
        {
            _stream = new MemoryStream();
            _writer = new StreamWriter(_stream);
            _stream.WriteByte(0);
            mapping.Add("", 0);
        }

        /// <summary>
        /// 
        /// </summary>
        Dictionary<string, long> mapping = new Dictionary<string, long>();

        /// <summary>
        /// Gets the index of the string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public int GetStringIndex(string str)
        {
            if (str == null) str = "";
            long index;
            if (!mapping.TryGetValue(str, out index))
            {
                mapping.Add(str, index = _stream.Length);
                _writer.Write(str);
                _writer.Flush();
                _stream.WriteByte(0);
            }
            return (int)index;
        }

        /// <summary>
        /// Overloaded method to write the section's data accoring to the
        /// section's specification.
        /// Every section inherting Elf32ProgbitsSection has to implement
        /// this method.
        /// </summary>
        /// <param name="writer">Reference to the binary writer</param>
        protected override void WriteDataImpl(BinaryWriter writer)
        {
            writer.Write(
                _stream.GetBuffer(),
                0,
                (int)_stream.Length
            );
        }
    }
}
