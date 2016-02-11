// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.IO;

namespace Mosa.Compiler.Linker.Elf
{
	/// <summary>
	///
	/// </summary>
	public class Section
	{
		public delegate void EmitSectionMethod(Section section);

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

		public ulong AddressAlignment { get; set; }

		public uint EntrySize { get; set; }

		public bool IsEmitted { get; set; }

		public List<Section> Dependencies { get; private set; }

		public EmitSectionMethod EmitMethod { get; set; }

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
			var header = new SectionHeaderEntry();

			header.Name = NameIndex;
			header.Address = Address;
			header.Offset = Offset;
			header.Size = Size;
			header.EntrySize = EntrySize;
			header.AddressAlignment = AddressAlignment;
			header.Type = Type;
			header.Flags = Flags;
			header.Link = Link == null ? 0 : Link.Index;
			header.Info = Info == null ? 0 : Info.Index;

			header.Write(elfType, writer);
		}
	}
}
