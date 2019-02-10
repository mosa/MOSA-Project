// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System.Collections.Generic;

namespace Mosa.Compiler.Extensions.Dwarf
{
	public class DwarfWriteContext
	{
		public EndianAwareBinaryWriter Writer { get; set; }

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
