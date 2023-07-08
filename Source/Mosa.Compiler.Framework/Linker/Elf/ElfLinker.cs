// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Linker.Elf.Dwarf;

namespace Mosa.Compiler.Framework.Linker.Elf;

public delegate Stream SectionEmitter();

public sealed class ElfLinker
{
	#region Data Members

	private readonly MosaLinker Linker;

	private readonly LinkerFormatType LinkerFormatType;
	private readonly ElfHeader elfheader = new ElfHeader();

	private readonly List<Section> sections = new List<Section>();
	private readonly Dictionary<string, Section> sectionByName = new Dictionary<string, Section>();

	public Section nullSection = new Section();
	private readonly Section sectionHeaderStringSection = new Section();
	private readonly Section stringSection = new Section();
	private readonly Section symbolSection = new Section();

	private readonly List<byte> sectionHeaderStringTable = new List<byte>();

	private readonly List<byte> stringTable = new List<byte>(4096);

	private readonly Dictionary<LinkerSymbol, uint> symbolTableOffset = new Dictionary<LinkerSymbol, uint>();

	private static readonly string[] SectionNames = { ".text", ".data", ".rodata", ".bss" };

	private readonly MachineType MachineType;

	private readonly bool EmitShortSymbolName;

	#endregion Data Members

	#region Properties

	public uint BaseFileOffset { get; }

	public uint SectionAlignment { get; }

	#endregion Properties

	public ElfLinker(MosaLinker linker, LinkerFormatType linkerFormatType, MachineType machineType)
	{
		Linker = linker;
		LinkerFormatType = linkerFormatType;
		MachineType = machineType;

		sectionHeaderStringTable.Add((byte)'\0');
		stringTable.Add((byte)'\0');

		BaseFileOffset = 0x1000;   // required by ELF
		SectionAlignment = 0x1000; // default 1K

		// Cache for faster performance
		EmitShortSymbolName = linker.MosaSettings.EmitShortSymbolNames;
	}

	#region Helpers

	internal void RegisterSection(Section section)
	{
		Debug.Assert(section != null);

		var index = (ushort)sections.Count;
		section.Index = index;

		section.AddressAlignment = SectionAlignment;

		sections.Add(section);

		if (section.Name != null)
		{
			sectionByName.Add(section.Name, section);

			section.NameIndex = AddToSectionHeaderStringTable(section.Name);
		}
	}

	private Section GetSection(SectionKind sectionKind)
	{
		Debug.Assert(sectionKind != SectionKind.Unknown, "sectionKind != SectionKind.Unknown");
		return sectionByName[SectionNames[(int)sectionKind]];
	}

	#endregion Helpers

	public void Emit(Stream stream)
	{
		var writer = new BinaryWriter(stream, Encoding.Unicode);

		// Register the sections headers
		RegisterSections();

		// Write Sections
		WriteSections(writer);

		// Write program headers -- must be called before writing Elf header
		WriteProgramHeader(writer);

		// Write section headers
		WriteSectionHeader(writer);

		// Write ELF header
		WriteElfHeader(writer);
	}

	private void RegisterSections()
	{
		RegisterNullSection();

		RegisterStandardSections();

		RegisterSymbolSection();

		RegisterStringSection();

		RegisterRelocationSections();

		RegisterDwarfSections();

		RegisterSectionHeaderStringSection();
	}

	private void RegisterDwarfSections()
	{
		if (Linker.MosaSettings.EmitDwarf)
		{
			var dwarf = new DwarfSections(Linker.Compiler, this);
		}
	}

	private void RegisterStandardSections()
	{
		var previous = nullSection;

		foreach (var linkerSection in Linker.Sections)
		{
			if (linkerSection.Size == 0 && linkerSection.SectionKind != SectionKind.BSS)
				continue;

			var section = new Section
			{
				Name = SectionNames[(int)linkerSection.SectionKind],
				Address = linkerSection.VirtualAddress,
				Size = Alignment.AlignUp(linkerSection.Size, SectionAlignment),
				Emitter = () => { return WriteLinkerSection(linkerSection.SectionKind); },
				SectionKind = linkerSection.SectionKind
			};

			switch (linkerSection.SectionKind)
			{
				case SectionKind.Text:
					section.Type = SectionType.ProgBits;
					section.Flags = SectionAttribute.AllocExecute;
					break;

				case SectionKind.Data:
					section.Type = SectionType.ProgBits;
					section.Flags = SectionAttribute.Alloc | SectionAttribute.Write;
					break;

				case SectionKind.ROData:
					section.Type = SectionType.ProgBits;
					section.Flags = SectionAttribute.Alloc;
					break;

				case SectionKind.BSS:
					section.Type = SectionType.NoBits;
					section.Flags = SectionAttribute.Alloc | SectionAttribute.Write;
					break;
			}

			section.AddDependency(previous);

			RegisterSection(section);

			previous = section;
		}
	}

	private void RegisterNullSection()
	{
		nullSection = new Section
		{
			Name = null,
			Type = SectionType.Null
		};

		RegisterSection(nullSection);
	}

	private void RegisterSectionHeaderStringSection()
	{
		sectionHeaderStringSection.Name = ".shstrtab";
		sectionHeaderStringSection.Type = SectionType.StringTable;
		sectionHeaderStringSection.Emitter = EmitSectionHeaderStringSection;

		RegisterSection(sectionHeaderStringSection);
	}

	private void RegisterStringSection()
	{
		stringSection.Name = ".strtab";
		stringSection.Type = SectionType.StringTable;
		stringSection.Emitter = EmitStringSection;

		RegisterSection(stringSection);

		sectionHeaderStringSection.AddDependency(stringSection);
	}

	private void RegisterSymbolSection()
	{
		//if (!Linker.LinkerSettings.Symbols)
		//	return;

		symbolSection.Name = ".symtab";
		symbolSection.Type = SectionType.SymbolTable;
		symbolSection.EntrySize = SymbolEntry.GetEntrySize(LinkerFormatType);
		symbolSection.Link = stringSection;
		symbolSection.Emitter = () => { return EmitSymbolSection(); };

		RegisterSection(symbolSection);

		stringSection.AddDependency(symbolSection);
		sectionHeaderStringSection.AddDependency(symbolSection);
	}

	private void RegisterRelocationSections()
	{
		if (!Linker.MosaSettings.EmitStaticRelocations)
			return;

		foreach (var kind in MosaLinker.SectionKinds)
		{
			var reloc = false;
			var relocAddend = false;

			foreach (var symbol in Linker.Symbols)
			{
				if (symbol.SectionKind != kind)
					continue;

				if (symbol.IsExternalSymbol)
					continue;

				foreach (var patch in symbol.LinkRequests)
				{
					if (patch.LinkType == LinkType.Size)
						continue;

					if (!patch.ReferenceSymbol.IsExternalSymbol)
						continue;

					if (patch.ReferenceOffset == 0)
						reloc = true;
					else
						relocAddend = true;

					if (reloc && relocAddend)
						break;
				}

				if (reloc && relocAddend)
					break;
			}

			if (reloc)
			{
				RegisterRelocationSection(kind, false);
			}

			if (relocAddend)
			{
				RegisterRelocationSection(kind, true);
			}
		}
	}

	private void RegisterRelocationSection(SectionKind kind, bool addend)
	{
		var relocationSection = new Section
		{
			Name = (addend ? ".rela" : ".rel") + SectionNames[(int)kind],
			Type = addend ? SectionType.RelocationA : SectionType.Relocation,
			Link = symbolSection,
			Info = GetSection(kind),
			EntrySize = addend ? RelocationAddendEntry.GetEntrySize(LinkerFormatType) : RelocationEntry.GetEntrySize(LinkerFormatType),
			Emitter = () => { return addend ? EmitRelocationAddendSection(kind) : EmitRelocationSection(kind); }
		};

		RegisterSection(relocationSection);

		relocationSection.AddDependency(symbolSection);
		relocationSection.AddDependency(GetSection(kind));
	}

	private void WriteSections(BinaryWriter writer)
	{
		var completed = new HashSet<Section>();

		for (var i = 0; i < sections.Count;)
		{
			var section = sections[i];

			if (completed.Contains(section))
			{
				i++;
				continue;
			}

			var dependency = false;

			foreach (var dep in section.Dependencies)
			{
				if (!completed.Contains(dep))
				{
					dependency = true;
					break;
				}
			}

			if (!dependency)
			{
				WriteSection(writer, section);
				completed.Add(section);
				i = 0;
				continue;
			}

			i++;
		}
	}

	private void WriteSection(BinaryWriter writer, Section section)
	{
		if (section.Type == SectionType.Null)
			return;

		section.Offset = Math.Max(BaseFileOffset, Alignment.AlignUp((uint)writer.BaseStream.Length, SectionAlignment));

		if (section.Type == SectionType.NoBits)
			return;

		writer.SetPosition(section.Offset);

		if (section.Emitter != null)
		{
			section.Stream = section.Emitter();
		}

		if (section.Stream != null)
		{
			section.Stream.Position = 0;
			section.Stream.WriteTo(writer.BaseStream);
		}

		section.Size = (uint)section.Stream.Length;
	}

	private void WriteElfHeader(BinaryWriter writer)
	{
		writer.SetPosition(0);

		elfheader.Type = FileType.Executable;
		elfheader.Machine = MachineType;
		elfheader.EntryAddress = (uint)Linker.EntryPoint.VirtualAddress;
		elfheader.CreateIdent(LinkerFormatType == LinkerFormatType.Elf32 ? IdentClass.Class32 : IdentClass.Class64, IdentData.Data2LSB);
		elfheader.SectionHeaderNumber = (ushort)sections.Count;
		elfheader.SectionHeaderStringIndex = sectionHeaderStringSection.Index;

		elfheader.Write(LinkerFormatType, writer);
	}

	private void WriteProgramHeader(BinaryWriter writer)
	{
		elfheader.ProgramHeaderOffset = ElfHeader.GetEntrySize(LinkerFormatType);

		writer.SetPosition(elfheader.ProgramHeaderOffset);

		elfheader.ProgramHeaderNumber = 0;

		foreach (var section in sections)
		{
			if (section.SectionKind == SectionKind.Unknown)
				continue;

			if (section.Size == 0 && section.SectionKind != SectionKind.BSS)
				continue;

			if (section.Address == 0)
				continue;

			var programHeader = new ProgramHeader
			{
				Alignment = SectionAlignment,
				FileSize = Alignment.AlignUp(section.Size, SectionAlignment),
				MemorySize = Alignment.AlignUp(section.Size, SectionAlignment),
				Offset = section.Offset,
				VirtualAddress = section.Address,
				PhysicalAddress = section.Address,
				Type = ProgramHeaderType.Load,
				Flags =
					section.SectionKind == SectionKind.Text ? ProgramHeaderFlags.Read | ProgramHeaderFlags.Execute :
					section.SectionKind == SectionKind.ROData ? ProgramHeaderFlags.Read : ProgramHeaderFlags.Read | ProgramHeaderFlags.Write
			};

			programHeader.Write(LinkerFormatType, writer);

			elfheader.ProgramHeaderNumber++;
		}
	}

	private void WriteSectionHeader(BinaryWriter writer)
	{
		elfheader.SectionHeaderOffset = elfheader.ProgramHeaderOffset + ProgramHeader.GetEntrySize(LinkerFormatType) * elfheader.ProgramHeaderNumber;

		writer.SetPosition(elfheader.SectionHeaderOffset);

		foreach (var section in sections)
		{
			section.WriteSectionHeader(LinkerFormatType, writer);
		}
	}

	private Stream EmitSectionHeaderStringSection()
	{
		return new MemoryStream(sectionHeaderStringTable.ToArray());
	}

	private Stream EmitStringSection()
	{
		return new MemoryStream(stringTable.ToArray());
	}

	private Stream EmitSymbolSection()
	{
		var stream = new MemoryStream();
		var writer = new BinaryWriter(stream);

		var emitSymbols = Linker.MosaSettings.EmitSumbols;

		// first entry is completely filled with zeros
		writer.WriteZeroBytes(SymbolEntry.GetEntrySize(LinkerFormatType));

		uint count = 1;

		foreach (var symbol in Linker.Symbols)
		{
			if (symbol.SectionKind == SectionKind.Unknown && symbol.LinkRequests.Count == 0)
				continue;

			Debug.Assert(symbol.SectionKind != SectionKind.Unknown, "symbol.SectionKind != SectionKind.Unknown");

			if (!(symbol.IsExternalSymbol || emitSymbols))
				continue;

			if (symbol.VirtualAddress == 0)
				continue;

			if (symbol.Size == 0)
				continue;

			var name = GetFinalSymboName(symbol);

			var symbolEntry = new SymbolEntry
			{
				Name = AddToStringTable(name),
				Value = symbol.VirtualAddress,
				Size = symbol.Size,
				SymbolBinding = SymbolBinding.Global,
				SymbolVisibility = SymbolVisibility.Default,
				SymbolType = symbol.SectionKind == SectionKind.Text ? SymbolType.Function : SymbolType.Object,
				SectionHeaderTableIndex = GetSection(symbol.SectionKind).Index
			};

			symbolEntry.Write(LinkerFormatType, writer);
			symbolTableOffset.Add(symbol, count);

			count++;
		}

		return stream;
	}

	private Stream EmitRelocationSection(SectionKind kind)
	{
		var stream = new MemoryStream();
		var writer = new BinaryWriter(stream);

		foreach (var symbol in Linker.Symbols)
		{
			if (symbol.IsExternalSymbol)
				continue;

			foreach (var patch in symbol.LinkRequests)
			{
				if (patch.ReferenceOffset != 0)
					continue;

				if (patch.ReferenceSymbol.SectionKind != kind)
					continue;

				if (patch.LinkType == LinkType.Size)
					continue;

				if (!patch.ReferenceSymbol.IsExternalSymbol) // FUTURE: include relocations for static symbols, if option selected
					continue;

				var relocationEntry = new RelocationEntry
				{
					RelocationType = ConvertType(MachineType, patch.LinkType, patch.PatchType),
					Symbol = symbolTableOffset[patch.ReferenceSymbol],
					Offset = (ulong)(symbol.SectionOffset + patch.PatchOffset),
				};

				relocationEntry.Write(LinkerFormatType, writer);
			}
		}

		return stream;
	}

	private Stream EmitRelocationAddendSection(SectionKind kind)
	{
		var stream = new MemoryStream();
		var writer = new BinaryWriter(stream);

		foreach (var symbol in Linker.Symbols)
		{
			if (symbol.IsExternalSymbol)
				continue;

			foreach (var patch in symbol.LinkRequests)
			{
				if (patch.ReferenceOffset == 0)
					continue;

				if (patch.ReferenceSymbol.SectionKind != kind)
					continue;

				if (patch.LinkType == LinkType.Size)
					continue;

				if (!patch.ReferenceSymbol.IsExternalSymbol) // FUTURE: include relocations for static symbols, if option selected
					continue;

				var relocationAddendEntry = new RelocationAddendEntry
				{
					RelocationType = ConvertType(MachineType, patch.LinkType, patch.PatchType),
					Symbol = symbolTableOffset[patch.ReferenceSymbol],
					Offset = (ulong)(symbol.SectionOffset + patch.PatchOffset),
					Addend = (ulong)patch.ReferenceOffset,
				};

				relocationAddendEntry.Write(LinkerFormatType, writer);
			}
		}

		return stream;
	}

	private Stream WriteLinkerSection(SectionKind kind)
	{
		var stream = new MemoryStream();

		foreach (var symbol in Linker.Symbols)
		{
			if (symbol.IsReplaced)
				continue;

			if (symbol.SectionKind != kind)
				continue;

			if (!symbol.IsResolved)
				continue;

			if (symbol.Size == 0)
				continue;

			stream.Seek(symbol.SectionOffset, SeekOrigin.Begin);

			if (symbol.IsDataAvailable)
			{
				symbol.Stream.Position = 0;
				symbol.Stream.WriteTo(stream);
			}
		}

		return stream;
	}

	private uint AddToStringTable(string text)
	{
		if (text.Length == 0)
			return 0;

		var index = (uint)stringTable.Count;

		foreach (var c in text)
		{
			stringTable.Add((byte)c);
		}

		stringTable.Add((byte)'\0');

		return index;
	}

	private uint AddToSectionHeaderStringTable(string text)
	{
		if (text.Length == 0)
			return 0;

		var index = (uint)sectionHeaderStringTable.Count;

		foreach (var c in text)
		{
			sectionHeaderStringTable.Add((byte)c);
		}

		sectionHeaderStringTable.Add((byte)'\0');

		return index;
	}

	private string GetFinalSymboName(LinkerSymbol symbol)
	{
		if (symbol.ExternalSymbolName != null)
			return symbol.ExternalSymbolName;

		if (symbol.SectionKind != SectionKind.Text)
			return symbol.Name;

		if (!EmitShortSymbolName)
			return symbol.Name;

		var pos = symbol.Name.LastIndexOf(") ");

		if (pos < 0)
			return symbol.Name;

		var shortname = symbol.Name.Substring(0, pos + 1);

		return shortname;
	}

	private static RelocationType ConvertType(MachineType machineType, LinkType linkType, PatchType patchType)
	{
		if (machineType == MachineType.Intel386)
		{
			if (linkType == LinkType.AbsoluteAddress)
				return RelocationType.R_386_32;
			else if (linkType == LinkType.RelativeOffset)
				return RelocationType.R_386_PC32;
			else if (linkType == LinkType.Size)
				return RelocationType.R_386_COPY;
		}
		else if (machineType == MachineType.ARM)
		{
			if (linkType == LinkType.AbsoluteAddress)
				return RelocationType.R_ARM_ABS16;
			else if (linkType == LinkType.RelativeOffset)
				return RelocationType.R_ARM_PC24;
			else if (linkType == LinkType.Size)
				return RelocationType.R_ARM_COPY;
		}

		return RelocationType.R_386_NONE;
	}
}
