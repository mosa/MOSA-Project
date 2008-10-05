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
    class Elf32StringTableSection : Elf32ProgbitsSection
    {
        MemoryStream _stream;
        StreamWriter _writer;

        public Elf32StringTableSection(Elf32File file, string name, Elf32SectionFlags flags)
            : base(file, name, Elf32SectionType.SHT_STRTAB, flags)
        {
            _stream = new MemoryStream();
            _writer = new StreamWriter(_stream);
            _stream.WriteByte(0);
            mapping.Add("", 0);
        }

        Dictionary<string, long> mapping = new Dictionary<string, long>();

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
