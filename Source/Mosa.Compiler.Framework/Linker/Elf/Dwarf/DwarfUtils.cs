// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.IO;

namespace Mosa.Compiler.Framework.Linker.Elf.Dwarf
{
	public class DwarfWriteContext
	{
		public BinaryWriter Writer { get; set; }

		public List<DwarfAbbrev> AbbrevList;

		public DwarfAbbrev CreateAbbrev(DwarfTag tag, ICollection<DwarfAbbrevAttribute> attributes, ICollection<DwarfAbbrev> children = null)
		{
			var abbr = new DwarfAbbrev
			{
				Number = GetNewTagNumber(),
				Tag = tag,
				Attributes = attributes,
				Children = children
			};
			AbbrevList.Add(abbr);
			return abbr;
		}

		private uint LastTagNumber = 0;

		public uint GetNewTagNumber()
		{
			return ++LastTagNumber;
		}
	}
}
