using System;
using Mosa.Compiler.Common;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Mosa.Compiler.Framework.Linker.Elf;
using Mosa.Compiler.Framework.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;
using System.Linq;
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
