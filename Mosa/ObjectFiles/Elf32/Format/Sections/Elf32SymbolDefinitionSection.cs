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
using System.IO;

namespace Mosa.ObjectFiles.Elf32.Format.Sections
{
    abstract class Elf32SymbolDefinitionSection : Elf32ProgbitsSection
    {
        MemoryStream _stream;
        BinaryWriter _writer;

        public Elf32SymbolDefinitionSection(Elf32File file, string name, Elf32SectionType type, Elf32SectionFlags flags)
            : base(file, name, type, flags)
        {
            _stream = new MemoryStream();
            _writer = new BinaryWriter(_stream);
        }

        public Elf32Symbol BeginSymbol(Elf32Symbol symbol)
        {
            if (symbol.IsDefined)
                throw new NotSupportedException("Symbol re-defined");
            symbol.Begin(this, (int)_stream.Length);
            return symbol;
        }

        public void EndSymbol(Elf32Symbol symbol, byte[] buffer)
        {
            EndSymbol(symbol, buffer, 0, buffer.Length);
        }
        public void EndSymbol(Elf32Symbol symbol, byte[] buffer, int offset, int count)
        {
            if (symbol.IsFinished)
                throw new NotSupportedException("Symbol re-defined");
            symbol.End(count);
            _stream.Write(buffer, offset, count);
        }

        protected override void WriteDataImpl(BinaryWriter writer)
        {
            writer.Write(
                _stream.GetBuffer(),
                0,
                (int)_stream.Length
            );
        }

        public void ApplyPatch(int offset, sbyte value)
        {
            lock (this)
            {
                int old = (int)_writer.Seek(0, SeekOrigin.Current);
                _writer.Seek(offset, SeekOrigin.Begin);
                _writer.Write(value);
                _writer.Seek(old, SeekOrigin.Begin);
            }
        }
        public void ApplyPatch(int offset, short value)
        {
            lock (this)
            {
                int old = (int)_writer.Seek(0, SeekOrigin.Current);
                _writer.Seek(offset, SeekOrigin.Begin);
                _writer.Write(value);
                _writer.Seek(old, SeekOrigin.Begin);
            }
        }
        public void ApplyPatch(int offset, int value)
        {
            lock (this)
            {
                int old = (int)_writer.Seek(0, SeekOrigin.Current);
                _writer.Seek(offset, SeekOrigin.Begin);
                _writer.Write(value);
                _writer.Seek(old, SeekOrigin.Begin);
            }
        }
        public void ApplyPatch(int offset, long value)
        {
            lock (this)
            {
                int old = (int)_writer.Seek(0, SeekOrigin.Current);
                _writer.Seek(offset, SeekOrigin.Begin);
                _writer.Write(value);
                _writer.Seek(old, SeekOrigin.Begin);
            }
        }
    }
}
