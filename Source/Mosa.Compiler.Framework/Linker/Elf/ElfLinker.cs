// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Mosa.Compiler.Framework.Linker.Elf
{
	public sealed class ElfLinker
	{
		#region Data Members

		private readonly MosaLinker linker;

		private readonly LinkerFormatType linkerFormatType;
		private readonly ElfHeader elfheader = new ElfHeader();

		private readonly List<Section> sections = new List<Section>();
		private readonly Dictionary<string, Section> sectionByName = new Dictionary<string, Section>();
		private readonly Dictionary<Section, ushort> sectionToIndex = new Dictionary<Section, ushort>();

		public Section nullSection = new Section();
		private readonly Section sectionHeaderStringSection = new Section();
		private readonly Section stringSection = new Section();
		private readonly Section symbolSection = new Section();

		private readonly List<byte> sectionHeaderStringTable = new List<byte>();

		private readonly List<byte> stringTable = new List<byte>(4096);

		private readonly Dictionary<LinkerSymbol, uint> symbolTableOffset = new Dictionary<LinkerSymbol, uint>();

		private EndianAwareBinaryWriter writer;

		private static readonly string[] LinkerSectionNames = { ".text", ".data", ".rodata", ".bss" };

		public uint BaseFileOffset { get; }

		public uint SectionAlignment { get; }

		#endregion Data Members

		public ElfLinker(MosaLinker linker, LinkerFormatType linkerFormatType)
		{
			this.linker = linker;
			this.linkerFormatType = linkerFormatType;

			sectionHeaderStringTable.Add((byte)'\0');
			stringTable.Add((byte)'\0');

			BaseFileOffset = 0x1000;   // required by ELF
			SectionAlignment = 0x1000; // default 1K
		}

		#region Helpers

		private void AddSection(Section section)
		{
			Debug.Assert(section != null);

			var index = (ushort)sections.Count;
			section.Index = index;

			sections.Add(section);
			sectionToIndex.Add(section, index);

			if (section.Name != null)
			{
				sectionByName.Add(section.Name, section);

				section.NameIndex = AddToSectionHeaderStringTable(section.Name);
			}
		}

		private Section GetSection(string name)
		{
			return sectionByName[name];
		}

		private Section GetSection(SectionKind sectionKind)
		{
			Debug.Assert(sectionKind != SectionKind.Unknown, "sectionKind != SectionKind.Unknown");
			return GetSection(LinkerSectionNames[(int)sectionKind]);
		}

		private void ResolveSectionOffset(Section section)
		{
			if (section.Type == SectionType.NoBits || section.Type == SectionType.Null)
				return;

			if (section.Offset != 0)
				return;

			uint max = 0;

			foreach (var sec in sections)
			{
				max = Math.Max(max, sec.Offset + sec.Size);
			}

			section.Offset = Alignment.AlignUp(max, linker.SectionAlignment);
		}

		#endregion Helpers

		public void Emit(Stream stream)
		{
			writer = new EndianAwareBinaryWriter(stream, Encoding.Unicode, linker.Endianness);

			// Create the sections headers
			CreateSections();

			// Write Sections
			WriteSections();

			// Write program headers -- must be called before writing Elf header
			WriteProgramHeaders();

			// Write section headers
			WriteSectionHeaders();

			// Write ELF header
			WriteElfHeader();
		}

		private void WriteSections()
		{
			var completed = new HashSet<Section>();

			for (int i = 0; i < sections.Count;)
			{
				var section = sections[i];

				if (completed.Contains(section))
				{
					i++;
					continue;
				}

				bool dependency = false;

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
					WriteSection(section);
					completed.Add(section);
					i = 0;
					continue;
				}

				i++;
			}
		}

		private void WriteSection(Section section)
		{
			if (section.Type == SectionType.NoBits || section.Type == SectionType.Null)
				return;

			if (section.EmitMethod == null)
				return;

			ResolveSectionOffset(section);
			writer.Position = section.Offset;

			section.EmitMethod?.Invoke(section, writer);
		}

		private void WriteElfHeader()
		{
			writer.Position = 0;

			elfheader.Type = FileType.Executable;
			elfheader.Machine = linker.MachineType;
			elfheader.EntryAddress = (uint)linker.EntryPoint.VirtualAddress;
			elfheader.CreateIdent((linkerFormatType == LinkerFormatType.Elf32) ? IdentClass.Class32 : IdentClass.Class64, linker.Endianness == Endianness.Little ? IdentData.Data2LSB : IdentData.Data2MSB);
			elfheader.SectionHeaderNumber = (ushort)sections.Count;
			elfheader.SectionHeaderStringIndex = sectionHeaderStringSection.Index;

			elfheader.Write(linkerFormatType, writer);
		}

		private void WriteProgramHeaders()
		{
			elfheader.ProgramHeaderOffset = ElfHeader.GetEntrySize(linkerFormatType);

			writer.Position = elfheader.ProgramHeaderOffset;

			elfheader.ProgramHeaderNumber = 0;

			foreach (var linkerSection in linker.Sections)
			{
				if (linkerSection.Size == 0 && linkerSection.SectionKind != SectionKind.BSS)
					continue;

				var programHeader = new ProgramHeader
				{
					Alignment = linkerSection.SectionAlignment,
					FileSize = linkerSection.AlignedSize,
					MemorySize = linkerSection.AlignedSize,
					Offset = linkerSection.FileOffset,
					VirtualAddress = linkerSection.VirtualAddress,
					PhysicalAddress = linkerSection.VirtualAddress,
					Type = ProgramHeaderType.Load,
					Flags =
						(linkerSection.SectionKind == SectionKind.Text) ? ProgramHeaderFlags.Read | ProgramHeaderFlags.Execute :
						(linkerSection.SectionKind == SectionKind.ROData) ? ProgramHeaderFlags.Read : ProgramHeaderFlags.Read | ProgramHeaderFlags.Write
				};

				programHeader.Write(linkerFormatType, writer);

				elfheader.ProgramHeaderNumber++;
			}

			if (linker.CreateExtraProgramHeaders != null)
			{
				foreach (var programHeader in linker.CreateExtraProgramHeaders())
				{
					if (programHeader.FileSize == 0)
						continue;

					programHeader.Write(linkerFormatType, writer);

					elfheader.ProgramHeaderNumber++;
				}
			}
		}

		private void CreateSections()
		{
			CreateNullSection();

			var previous = nullSection;

			foreach (var linkerSection in linker.Sections)
			{
				if (linkerSection.Size == 0 && linkerSection.SectionKind != SectionKind.BSS)
					continue;

				var section = new Section()
				{
					Name = LinkerSectionNames[(int)linkerSection.SectionKind],
					Address = linkerSection.VirtualAddress,
					Offset = linkerSection.FileOffset,
					Size = linkerSection.AlignedSize,
					AddressAlignment = linkerSection.SectionAlignment,
					EmitMethod = WriteLinkerSection,
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

				AddSection(section);

				previous = section;
			}

			CreateSymbolSection();

			CreateStringSection();

			CreateRelocationSections();

			CreatePluginSecttions();

			if (linker.CreateExtraSections != null)
				CreateExtraSections();

			CreateSectionHeaderStringSection();
		}

		private void CreatePluginSecttions()
		{
		}

		private void CreateExtraSections()
		{
			foreach (var section in linker.CreateExtraSections())
			{
				AddSection(section);
			}
		}

		private void WriteSectionHeaders()
		{
			elfheader.SectionHeaderOffset = elfheader.ProgramHeaderOffset + (ProgramHeader.GetEntrySize(linkerFormatType) * elfheader.ProgramHeaderNumber);

			writer.Position = elfheader.SectionHeaderOffset;

			foreach (var section in sections)
			{
				section.WriteSectionHeader(linkerFormatType, writer);
			}
		}

		private void WriteLinkerSection(Section section, EndianAwareBinaryWriter writer)
		{
			var linkerSection = linker.Sections[(int)section.SectionKind];

			writer.Position = section.Offset;
			linker.WriteTo(writer.BaseStream, linkerSection);
		}

		private void CreateNullSection()
		{
			nullSection = new Section()
			{
				Name = null,
				Type = SectionType.Null
			};
			AddSection(nullSection);
		}

		private void CreateSectionHeaderStringSection()
		{
			sectionHeaderStringSection.Name = ".shstrtab";
			sectionHeaderStringSection.Type = SectionType.StringTable;
			sectionHeaderStringSection.AddressAlignment = linker.SectionAlignment;
			sectionHeaderStringSection.EmitMethod = WriteSectionHeaderStringSection;

			AddSection(sectionHeaderStringSection);
		}

		private void WriteSectionHeaderStringSection(Section section, EndianAwareBinaryWriter writer)
		{
			Debug.Assert(section == sectionHeaderStringSection);

			section.Size = (uint)sectionHeaderStringTable.Count;
			writer.Write(sectionHeaderStringTable.ToArray());
		}

		private uint AddToSectionHeaderStringTable(string text)
		{
			if (text.Length == 0)
				return 0;

			uint index = (uint)sectionHeaderStringTable.Count;

			foreach (char c in text)
			{
				sectionHeaderStringTable.Add((byte)c);
			}

			sectionHeaderStringTable.Add((byte)'\0');

			return index;
		}

		private void CreateStringSection()
		{
			stringSection.Name = ".strtab";
			stringSection.Type = SectionType.StringTable;
			stringSection.AddressAlignment = linker.SectionAlignment;
			stringSection.EmitMethod = WriteStringSection;

			AddSection(stringSection);

			sectionHeaderStringSection.AddDependency(stringSection);
		}

		private void WriteStringSection(Section section, EndianAwareBinaryWriter writer)
		{
			Debug.Assert(section == stringSection);

			writer.Write(stringTable.ToArray());

			section.Size = (uint)stringTable.Count;
		}

		private uint AddToStringTable(string text)
		{
			if (text.Length == 0)
				return 0;

			uint index = (uint)stringTable.Count;

			foreach (char c in text)
			{
				stringTable.Add((byte)c);
			}

			stringTable.Add((byte)'\0');

			return index;
		}

		private void CreateSymbolSection()
		{
			symbolSection.Name = ".symtab";
			symbolSection.Type = SectionType.SymbolTable;
			symbolSection.AddressAlignment = linker.SectionAlignment;
			symbolSection.EntrySize = SymbolEntry.GetEntrySize(linkerFormatType);
			symbolSection.Link = stringSection;
			symbolSection.EmitMethod = WriteSymbolSection;

			AddSection(symbolSection);

			stringSection.AddDependency(symbolSection);
			sectionHeaderStringSection.AddDependency(symbolSection);
		}

		private void WriteSymbolSection(Section section, EndianAwareBinaryWriter writer)
		{
			Debug.Assert(section == symbolSection);

			// first entry is completely filled with zeros
			writer.WriteZeroBytes(SymbolEntry.GetEntrySize(linkerFormatType));

			uint count = 1;

			foreach (var symbol in linker.Symbols)
			{
				if (symbol.SectionKind == SectionKind.Unknown && symbol.LinkRequests.Count == 0)
					continue;

				Debug.Assert(symbol.SectionKind != SectionKind.Unknown, "symbol.SectionKind != SectionKind.Unknown");

				if (!(symbol.IsExternalSymbol || linker.EmitAllSymbols))
					continue;

				var symbolEntry = new SymbolEntry()
				{
					Name = AddToStringTable(symbol.ExternalSymbolName ?? symbol.Name),
					Value = symbol.VirtualAddress,
					Size = symbol.Size,
					SymbolBinding = SymbolBinding.Global,
					SymbolVisibility = SymbolVisibility.Default,
					SymbolType = symbol.SectionKind == SectionKind.Text ? SymbolType.Function : SymbolType.Object,
					SectionHeaderTableIndex = GetSection(symbol.SectionKind).Index
				};

				symbolEntry.Write(linkerFormatType, writer);
				symbolTableOffset.Add(symbol, count);

				count++;
			}

			section.Size = count * SymbolEntry.GetEntrySize(linkerFormatType);
		}

		private void CreateRelocationSections()
		{
			foreach (var kind in MosaLinker.SectionKinds)
			{
				bool reloc = false;
				bool relocAddend = false;

				foreach (var symbol in linker.Symbols)
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
					CreateRelocationSection(kind, false);
				}

				if (relocAddend)
				{
					CreateRelocationSection(kind, true);
				}
			}
		}

		private void CreateRelocationSection(SectionKind kind, bool addend)
		{
			var relocationSection = new Section()
			{
				Name = (addend ? ".rela" : ".rel") + LinkerSectionNames[(int)kind],
				Type = addend ? SectionType.RelocationA : SectionType.Relocation,
				Link = symbolSection,
				Info = GetSection(kind),
				AddressAlignment = linker.SectionAlignment,
				EntrySize = addend ? RelocationAddendEntry.GetEntrySize(linkerFormatType) : RelocationEntry.GetEntrySize(linkerFormatType),
				EmitMethod = WriteRelocationSection
			};

			AddSection(relocationSection);

			relocationSection.AddDependency(symbolSection);
			relocationSection.AddDependency(GetSection(kind));
		}

		private bool ContainsKind(SectionKind kind)
		{
			foreach (var symbol in linker.Symbols)
			{
				if (symbol.SectionKind == kind)
					return true;
			}

			return false;
		}

		private void WriteRelocationSection(Section section, EndianAwareBinaryWriter writer)
		{
			if (section.SectionKind == SectionKind.BSS)
				return;

			if (!ContainsKind(section.SectionKind))
				return;

			if (section.Type == SectionType.Relocation)
			{
				EmitRelocation(section, writer);
			}
			else if (section.Type == SectionType.RelocationA)
			{
				EmitRelocationAddend(section, writer);
			}
		}

		private void EmitRelocation(Section section, EndianAwareBinaryWriter writer)
		{
			int count = 0;

			foreach (var symbol in linker.Symbols)
			{
				if (symbol.IsExternalSymbol)
					continue;

				foreach (var patch in symbol.LinkRequests)
				{
					if (patch.ReferenceOffset != 0)
						continue;

					if (patch.ReferenceSymbol.SectionKind != section.SectionKind)
						continue;

					if (patch.LinkType == LinkType.Size)
						continue;

					if (!patch.ReferenceSymbol.IsExternalSymbol) // FUTURE: include relocations for static symbols, if option selected
						continue;

					var relocationEntry = new RelocationEntry()
					{
						RelocationType = ConvertType(patch.LinkType, linker.MachineType),
						Symbol = symbolTableOffset[patch.ReferenceSymbol],
						Offset = (ulong)(symbol.SectionOffset + patch.PatchOffset),
					};

					relocationEntry.Write(linkerFormatType, writer);
					count++;
				}

				section.Size = (uint)(count * RelocationEntry.GetEntrySize(linkerFormatType));
			}
		}

		private void EmitRelocationAddend(Section section, EndianAwareBinaryWriter writer)
		{
			int count = 0;

			foreach (var symbol in linker.Symbols)
			{
				//if (symbol.SectionKind != section.SectionKind)
				//	continue;

				if (symbol.IsExternalSymbol)
					continue;

				foreach (var patch in symbol.LinkRequests)
				{
					if (patch.ReferenceOffset == 0)
						continue;

					if (patch.ReferenceSymbol.SectionKind != section.SectionKind)
						continue;

					if (patch.LinkType == LinkType.Size)
						continue;

					if (!patch.ReferenceSymbol.IsExternalSymbol) // FUTURE: include relocations for static symbols, if option selected
						continue;

					var relocationAddendEntry = new RelocationAddendEntry()
					{
						RelocationType = ConvertType(patch.LinkType, linker.MachineType),
						Symbol = symbolTableOffset[patch.ReferenceSymbol],
						Offset = (ulong)(symbol.SectionOffset + patch.PatchOffset),
						Addend = (ulong)patch.ReferenceOffset,
					};

					relocationAddendEntry.Write(linkerFormatType, writer);

					count++;
				}
			}

			section.Size = (uint)(count * RelocationAddendEntry.GetEntrySize(linkerFormatType));
		}

		private static RelocationType ConvertType(LinkType linkType, MachineType machineType)
		{
			if (machineType == MachineType.Intel386)
			{
				if (linkType == LinkType.AbsoluteAddress)
					return RelocationType.R_386_32;
				else if (linkType == LinkType.RelativeOffset)
					return RelocationType.R_386_PC32;
			}

			return RelocationType.R_386_NONE;
		}
	}
}
