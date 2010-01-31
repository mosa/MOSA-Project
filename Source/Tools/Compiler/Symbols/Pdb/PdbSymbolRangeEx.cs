using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Mosa.Tools.Compiler.Symbols.Pdb
{
    class PdbSymbolRangeEx
    {
        private short segment;
        private short pad1;
        private int offset;
        private int size;
        private int characteristics;
        private short index;
        private short pad2;
        private int timestamp;
        private int unknown;

        public PdbSymbolRangeEx(BinaryReader reader)
        {
            this.segment = reader.ReadInt16();
            this.pad1 = reader.ReadInt16();
            this.offset = reader.ReadInt32();
            this.size = reader.ReadInt32();
            this.characteristics = reader.ReadInt32();
            this.index = reader.ReadInt16();
            this.pad2 = reader.ReadInt16();
            this.timestamp = reader.ReadInt32();
            this.unknown = reader.ReadInt32();
        }
    }
}
