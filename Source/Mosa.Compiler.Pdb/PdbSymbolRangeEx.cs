// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;

namespace Mosa.Compiler.Pdb
{
	internal class PdbSymbolRangeEx
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
			segment = reader.ReadInt16();
			pad1 = reader.ReadInt16();
			offset = reader.ReadInt32();
			size = reader.ReadInt32();
			characteristics = reader.ReadInt32();
			index = reader.ReadInt16();
			pad2 = reader.ReadInt16();
			timestamp = reader.ReadInt32();
			unknown = reader.ReadInt32();
		}
	}
}
