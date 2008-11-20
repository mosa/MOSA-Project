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
    abstract class Elf32SymbolDefinitionSection : Elf32ProgbitsSection
    {
        /// <summary>
        /// 
        /// </summary>
        MemoryStream _stream;

        /// <summary>
        /// 
        /// </summary>
        BinaryWriter _writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Elf32SymbolDefinitionSection"/> class.
        /// </summary>
        /// <param name="file">File to write to</param>
        /// <param name="name">The section's name</param>
        /// <param name="type">Sectiontype</param>
        /// <param name="flags">Flags to use for this section</param>
        public Elf32SymbolDefinitionSection(Elf32File file, string name, Elf32SectionType type, Elf32SectionFlags flags)
            : base(file, name, type, flags)
        {
            _stream = new MemoryStream();
            _writer = new BinaryWriter(_stream);
        }

        /// <summary>
        /// Begins the symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns></returns>
        public Elf32Symbol BeginSymbol(Elf32Symbol symbol)
        {
            if (symbol.IsDefined)
                throw new NotSupportedException("Symbol re-defined");
            symbol.Begin(this, (int)_stream.Length);
            return symbol;
        }

        /// <summary>
        /// Ends the symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="buffer">The buffer.</param>
        public void EndSymbol(Elf32Symbol symbol, byte[] buffer)
        {
            EndSymbol(symbol, buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Ends the symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
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

        /// <summary>
        /// Applies the patch.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="value">The value.</param>
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
        /// <summary>
        /// Applies the patch.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="value">The value.</param>
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
        /// <summary>
        /// Applies the patch.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="value">The value.</param>
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
        /// <summary>
        /// Applies the patch.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="value">The value.</param>
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
