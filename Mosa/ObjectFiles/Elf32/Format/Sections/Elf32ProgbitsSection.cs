using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Mosa.ObjectFiles.Elf32.Format.Sections
{
    abstract class Elf32ProgbitsSection : Elf32Section
    {
        int _offset, _size;

        public Elf32ProgbitsSection(Elf32File file, string name, Elf32SectionType type, Elf32SectionFlags flags)
            : base(file, name, type, flags)
        {
        }

        public sealed override int Offset { get { return _offset; } }
        public sealed override int Size { get { return _size; } }

        public sealed override void WriteData(System.IO.BinaryWriter writer)
        {
            _offset = (int)writer.Seek(0, SeekOrigin.Current);
            WriteDataImpl(writer);
            _size = (int)writer.Seek(0, SeekOrigin.Current) - _offset;
        }

        protected abstract void WriteDataImpl(BinaryWriter writer);
    }
}
