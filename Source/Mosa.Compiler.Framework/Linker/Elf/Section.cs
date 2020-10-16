// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.IO;

namespace Mosa.Compiler.Framework.Linker.Elf
{
	/// <summary>
	/// ELF Section
	/// </summary>
	public class Section
	{
		public int Index { get; set; }

		public string Name { get; set; }

		public uint NameIndex { get; set; }

		public SectionType Type { get; set; }

		public SectionAttribute Flags { get; set; }

		public ulong Address { get; set; }

		public uint Offset { get; set; }

		public uint Size { get; set; }

		public Section Link { get; set; }

		public Section Info { get; set; }

		public ulong AddressAlignment { get; set; } = 0x1000;

		public uint EntrySize { get; set; }

		public bool IsEmitted { get; set; }

		public List<Section> Dependencies { get; }

		public SectionEmitter Emitter { get; set; }

		public Stream Stream { get; set; }

		public SectionKind SectionKind { get; set; }

		public Section()
		{
			IsEmitted = false;
			Dependencies = new List<Section>();
		}

		public void AddDependency(Section section)
		{
			Dependencies.Add(section);
		}

		public void WriteSectionHeader(LinkerFormatType elfType, BinaryWriter writer)
		{
			var header = new SectionHeaderEntry()
			{
				Name = NameIndex,
				Address = Address,
				Offset = Offset,
				Size = Size,
				EntrySize = EntrySize,
				AddressAlignment = AddressAlignment,
				Type = Type,
				Flags = Flags,
				Link = Link?.Index ?? 0,
				Info = Info?.Index ?? 0
			};
			header.Write(elfType, writer);
		}
	}
}
