// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Mosa.Compiler.Linker.Elf
{
	public class ElfLinker
	{
		#region Data members

		protected BaseLinker linker;

		protected LinkerFormatType linkerFormatType;
		protected ElfHeader elfheader = new ElfHeader();

		protected List<Section> sections = new List<Section>();
		protected Dictionary<string, Section> sectionByName = new Dictionary<string, Section>();
		protected Dictionary<Section, ushort> sectionToIndex = new Dictionary<Section, ushort>();

		protected Section nullSection = new Section();
		protected Section sectionHeaderStringSection = new Section();
		protected Section stringSection = new Section();
		protected Section symbolSection = new Section();

		protected List<byte> sectionHeaderStringTable = new List<byte>();
		protected List<byte> stringTable = new List<byte>();

		private Dictionary<LinkerSymbol, uint> symbolTableOffset = new Dictionary<LinkerSymbol, uint>();

		protected EndianAwareBinaryWriter writer;

		protected static string[] LinkerSectionNames = { ".text", ".data", ".rodata", ".bss" };

		public uint BaseFileOffset { get; private set; }

		public uint SectionAlignment { get; private set; }

		#endregion Data members

		public ElfLinker(BaseLinker linker, LinkerFormatType linkerFormatType)
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
			ushort index = (ushort)sections.Count;
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

		/// <summary>
		/// Emits the implementation.
		/// </summary>
		/// <param name="stream">The stream.</param>
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
					if (completed.Contains(dep))
					{
						continue;
					}
					else
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

			if (section.EmitMethod != null)
			{
				section.EmitMethod(section);
			}
		}

		private void WriteElfHeader()
		{
			writer.Position = 0;

			elfheader.Type = FileType.Executable;
			elfheader.Machine = linker.MachineType;
			elfheader.EntryAddress = (uint)linker.EntryPoint.VirtualAddress;
			elfheader.CreateIdent((linkerFormatType == LinkerFormatType.Elf32) ? IdentClass.Class32 : IdentClass.Class64, linker.Endianness == Endianness.Little ? IdentData.Data2LSB : IdentData.Data2MSB, null);
			elfheader.SectionHeaderNumber = (ushort)sections.Count;
			elfheader.SectionHeaderStringIndex = sectionHeaderStringSection.Index;

			elfheader.Write(linkerFormatType, writer);
		}

		private void WriteProgramHeaders()
		{
			elfheader.ProgramHeaderOffset = ElfHeader.GetEntrySize(linkerFormatType);

			writer.Position = elfheader.ProgramHeaderOffset;

			elfheader.ProgramHeaderNumber = 0;

			foreach (var section in linker.LinkerSections)
			{
				if (section.Size == 0 && section.SectionKind != SectionKind.BSS)
					continue;

				var programHeader = new ProgramHeader
				{
					Alignment = section.SectionAlignment,
					FileSize = section.AlignedSize,
					MemorySize = section.AlignedSize,
					Offset = section.FileOffset,
					VirtualAddress = section.VirtualAddress,
					PhysicalAddress = section.VirtualAddress,
					Type = ProgramHeaderType.Load,
					Flags =
						(section.SectionKind == SectionKind.Text) ? ProgramHeaderFlags.Read | ProgramHeaderFlags.Execute :
						(section.SectionKind == SectionKind.ROData) ? ProgramHeaderFlags.Read : ProgramHeaderFlags.Read | ProgramHeaderFlags.Write
				};

				programHeader.Write(linkerFormatType, writer);

				elfheader.ProgramHeaderNumber++;
			}
		}

		private void CreateSections()
		{
			CreateNullSection();

			Section previous = nullSection;

			foreach (var linkerSection in linker.LinkerSections)
			{
				if (linkerSection.Size == 0 && linkerSection.SectionKind != SectionKind.BSS)
					continue;

				var section = new Section();

				section.Name = LinkerSectionNames[(int)linkerSection.SectionKind];
				section.Address = linkerSection.VirtualAddress;
				section.Offset = linkerSection.FileOffset;
				section.Size = linkerSection.AlignedSize;
				section.AddressAlignment = linkerSection.SectionAlignment;
				section.EmitMethod = WriteLinkerSection;
				section.SectionKind = linkerSection.SectionKind;

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

			if (linker.EmitSymbols)
			{
				CreateSymbolSection();
				CreateStringSection();

				CreateRelocationSections();
			}

			CreateSectionHeaderStringSection();
		}

		private void WriteSectionHeaders()
		{
			elfheader.SectionHeaderOffset = elfheader.ProgramHeaderOffset + ProgramHeader.GetEntrySize(linkerFormatType) * elfheader.ProgramHeaderNumber;

			writer.Position = elfheader.SectionHeaderOffset;

			foreach (var section in sections)
			{
				section.WriteSectionHeader(linkerFormatType, writer);
			}
		}

		private void WriteLinkerSection(Section section)
		{
			writer.Position = (long)section.Offset;

			var linkerSection = linker.LinkerSections[(int)section.SectionKind];

			linkerSection.WriteTo(writer.BaseStream);
		}

		private void CreateNullSection()
		{
			nullSection = new Section();
			nullSection.Name = null;
			nullSection.Type = SectionType.Null;

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

		protected void WriteSectionHeaderStringSection(Section section)
		{
			Debug.Assert(section == sectionHeaderStringSection);

			section.Size = (uint)sectionHeaderStringTable.Count;
			writer.Write(sectionHeaderStringTable.ToArray());
		}

		protected uint AddToSectionHeaderStringTable(string text)
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

		protected void WriteStringSection(Section section)
		{
			Debug.Assert(section == stringSection);

			writer.Write(stringTable.ToArray());

			section.Size = (uint)stringTable.Count;
		}

		protected uint AddToStringTable(string text)
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

			//symbolSection.Info = sectionHeaderStringSection;
			symbolSection.EmitMethod = WriteSymbolSection;

			AddSection(symbolSection);

			stringSection.AddDependency(symbolSection);
			sectionHeaderStringSection.AddDependency(symbolSection);
		}

		protected void WriteSymbolSection(Section section)
		{
			Debug.Assert(section == symbolSection);

			// first entry is completely filled with zeros
			writer.WriteZeroBytes(SymbolEntry.GetEntrySize(linkerFormatType));

			uint count = 1;

			foreach (var symbol in linker.Symbols)
			{
				var symbolEntry = new SymbolEntry()
				{
					Name = AddToStringTable(symbol.Name),
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

			section.Size = (uint)(count * SymbolEntry.GetEntrySize(linkerFormatType));
		}

		protected void CreateRelocationSections()
		{
			foreach (var linkerSection in linker.LinkerSections)
			{
				bool reloc = false;
				bool relocAddend = false;

				foreach (var symbol in linkerSection.Symbols)
				{
					foreach (var patch in symbol.LinkRequests)
					{
						if (patch.LinkType == LinkType.Size)
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
					CreateRelocationSection(linkerSection.SectionKind, false);

				if (relocAddend)
					CreateRelocationSection(linkerSection.SectionKind, true);
			}
		}

		protected void CreateRelocationSection(SectionKind kind, bool addend)
		{
			var relocationSection = new Section();

			relocationSection.Name = (addend ? ".rela" : ".rel") + LinkerSectionNames[(int)kind];
			relocationSection.Type = addend ? SectionType.RelocationA : SectionType.Relocation;
			relocationSection.Link = symbolSection;
			relocationSection.Info = GetSection(kind);
			relocationSection.AddressAlignment = linker.SectionAlignment;
			relocationSection.EntrySize = (uint)(addend ? RelocationAddendEntry.GetEntrySize(linkerFormatType) : RelocationEntry.GetEntrySize(linkerFormatType));
			relocationSection.EmitMethod = WriteRelocationSection;

			AddSection(relocationSection);

			relocationSection.AddDependency(symbolSection);
			relocationSection.AddDependency(GetSection(kind));
		}

		protected void WriteRelocationSection(Section section)
		{
			var linkerSection = linker.LinkerSections[(int)section.Info.SectionKind];

			if (linkerSection == null || linkerSection.Symbols.Count == 0 || linkerSection.SectionKind == SectionKind.BSS)
				return;

			if (section.Type == SectionType.Relocation)
			{
				EmitRelocation(linkerSection, section);
			}
			else if (section.Type == SectionType.RelocationA)
			{
				EmitRelocationAddend(linkerSection, section);
			}
		}

		protected void EmitRelocation(LinkerSection linkerSection, Section section)
		{
			int count = 0;
			foreach (var symbol in linkerSection.Symbols)
			{
				foreach (var patch in symbol.LinkRequests)
				{
					if (patch.ReferenceOffset != 0)
						continue;

					if (patch.LinkType == LinkType.Size)
						continue;

					var relocationEntry = new RelocationEntry()
					{
						RelocationType = ConvertType(patch.PatchType, patch.LinkType, linker.MachineType),
						Symbol = symbolTableOffset[symbol],
						Offset = (ulong)patch.PatchOffset,
					};

					relocationEntry.Write(linkerFormatType, writer);
					count++;
				}

				section.Size = (uint)(count * RelocationEntry.GetEntrySize(linkerFormatType));
			}
		}

		protected void EmitRelocationAddend(LinkerSection linkerSection, Section section)
		{
			int count = 0;

			foreach (var symbol in linkerSection.Symbols)
			{
				foreach (var patch in symbol.LinkRequests)
				{
					if (patch.ReferenceOffset == 0)
						continue;

					if (patch.LinkType == LinkType.Size)
						continue;

					var relocationAddendEntry = new RelocationAddendEntry()
					{
						RelocationType = ConvertType(patch.PatchType, patch.LinkType, linker.MachineType),
						Symbol = symbolTableOffset[symbol],
						Offset = (ulong)patch.PatchOffset,
						Addend = (ulong)patch.ReferenceOffset,
					};

					relocationAddendEntry.Write(linkerFormatType, writer);

					count++;
				}
			}

			section.Size = (uint)(count * RelocationAddendEntry.GetEntrySize(linkerFormatType));
		}

		private static RelocationType ConvertType(PatchType patchType, LinkType linkType, MachineType machineType)
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
