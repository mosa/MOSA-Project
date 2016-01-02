// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.IO;
using System.Text;
using Mosa.Compiler.Common;

namespace Mosa.Compiler.Linker.Elf
{
	public abstract class ElfLinker : BaseLinker
	{
		#region Data members

		protected ElfType elfType;
		protected ElfHeader elfheader = new ElfHeader();

		protected List<SectionHeader> sectionHeaders = new List<SectionHeader>();
		protected Dictionary<SectionHeader, ushort> sectionToIndex = new Dictionary<SectionHeader, ushort>();
		protected Dictionary<string, SectionHeader> sectionByName = new Dictionary<string, SectionHeader>();

		protected SectionHeader sectionHeaderStringSection = new SectionHeader();
		protected SectionHeader stringSection = new SectionHeader();
		protected SectionHeader symbolSection = new SectionHeader();

		protected List<byte> sectionHeaderStringTable = new List<byte>();
		protected List<byte> stringTable = new List<byte>();

		protected EndianAwareBinaryWriter writer;

		public static string[] SectionNames = { ".text", ".data", ".rodata", ".bss" };

		#endregion Data members

		public ElfLinker(ElfType elfType)
		{
			this.elfType = elfType;
			SectionAlignment = 0x1000;
			BaseFileOffset = 0x1000;

			sectionHeaderStringTable.Add((byte)'\0');
			stringTable.Add((byte)'\0');
		}

		#region Helpers

		private void AddSectionHeaders(SectionHeader section, string name)
		{
			sectionHeaders.Add(section);
			sectionToIndex.Add(section, (ushort)(sectionHeaders.Count - 1));
			sectionByName.Add(name, section);
		}

		private ushort GetSectionHeaderIndex(SectionHeader sectionHeader)
		{
			return sectionToIndex[sectionHeader];
		}

		private SectionHeader GetSectionHeader(string name)
		{
			return sectionByName[name];
		}

		private SectionHeader GetSectionHeader(SectionKind sectionKind)
		{
			return GetSectionHeader(SectionNames[(int)sectionKind]);
		}

		private void ResolveSectionOffset(SectionHeader sectionHeader)
		{
			int index = GetSectionHeaderIndex(sectionHeader);

			for (int i = index - 1; i >= 0; i--)
			{
				var section = sectionHeaders[i];

				if (section.Offset == 0)
					continue;

				sectionHeader.Offset = Alignment.AlignUp(section.Offset + (long)section.Size, SectionAlignment);

				return;
			}
		}

		#endregion Helpers

		/// <summary>
		/// Emits the implementation.
		/// </summary>
		/// <param name="stream">The stream.</param>
		protected override void EmitImplementation(Stream stream)
		{
			writer = new EndianAwareBinaryWriter(stream, Encoding.Unicode, Endianness);

			// Create the sections headers
			CreateSectionHeaders();

			// write sections
			WriteSections(stream);

			// Write program headers -- must be called before writing Elf header
			WriteProgramHeaders();

			// Write section headers
			WriteSectionHeaders();

			// Write ELF header
			WriteElfHeader();
		}

		private void WriteElfHeader()
		{
			writer.Position = 0;

			elfheader.Type = FileType.Executable;
			elfheader.Machine = (MachineType)MachineID;
			elfheader.EntryAddress = (uint)EntryPoint.VirtualAddress;
			elfheader.CreateIdent((elfType == ElfType.Elf32) ? IdentClass.Class32 : IdentClass.Class64, Endianness == Endianness.Little ? IdentData.Data2LSB : IdentData.Data2MSB, null);
			elfheader.SectionHeaderNumber = (ushort)sectionHeaders.Count;
			elfheader.SectionHeaderStringIndex = GetSectionHeaderIndex(sectionHeaderStringSection);

			elfheader.Write(elfType, writer);
		}

		private void WriteProgramHeaders()
		{
			elfheader.ProgramHeaderOffset = ElfHeader.GetEntrySize(elfType);

			writer.Position = elfheader.ProgramHeaderOffset;

			elfheader.ProgramHeaderNumber = 0;

			foreach (var section in Sections)
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

				programHeader.Write(elfType, writer);

				elfheader.ProgramHeaderNumber++;
			}
		}

		private void CreateSectionHeaders()
		{
			CreateNullHeaderSection();

			foreach (var section in Sections)
			{
				if (section.Size == 0 && section.SectionKind != SectionKind.BSS)
					continue;

				var header = new SectionHeader();

				switch (section.SectionKind)
				{
					case SectionKind.Text:
						header.Type = SectionType.ProgBits;
						header.Flags = SectionAttribute.AllocExecute;
						break;

					case SectionKind.Data:
						header.Type = SectionType.ProgBits;
						header.Flags = SectionAttribute.Alloc | SectionAttribute.Write;
						break;

					case SectionKind.ROData:
						header.Type = SectionType.ProgBits;
						header.Flags = SectionAttribute.Alloc;
						break;

					case SectionKind.BSS:
						header.Type = SectionType.NoBits;
						header.Flags = SectionAttribute.Alloc | SectionAttribute.Write;
						break;
				}

				string name = SectionNames[(int)section.SectionKind];

				header.Name = AddToSectionHeaderStringTable(name);
				header.Address = section.VirtualAddress;
				header.Offset = section.FileOffset;
				header.Size = section.AlignedSize;
				header.Link = 0;
				header.Info = 0;
				header.AddressAlignment = section.SectionAlignment;
				header.EntrySize = 0;

				AddSectionHeaders(header, name);
			}

			if (EmitSymbols)
			{
				CreateSymbolHeaderSection();
				CreateStringHeaderSection();
			}

			CreateSectionHeaderStringHeaderSection();
		}

		private void WriteSectionHeaders()
		{
			elfheader.SectionHeaderOffset = elfheader.ProgramHeaderOffset + ProgramHeader.GetEntrySize(elfType) * elfheader.ProgramHeaderNumber;

			writer.Position = elfheader.SectionHeaderOffset;

			foreach (var section in sectionHeaders)
			{
				section.Write(elfType, writer);
			}
		}

		private void WriteSections(Stream stream)
		{
			// Write sections
			foreach (var section in Sections)
			{
				stream.Position = section.FileOffset;
				section.WriteTo(stream);
			}

			if (EmitSymbols)
			{
				WriteSymbolSection();
				WriteStringSection();
			}

			WriteSectionHeaderStringSection();
		}

		/// <summary>
		/// Counts the valid sections.
		/// </summary>
		/// <returns>Determines the number of sections.</returns>
		protected uint CountNonEmptySections()
		{
			uint sections = 0;

			foreach (var section in Sections)
			{
				if (section.Size > 0 && section.SectionKind != SectionKind.BSS)
				{
					sections++;
				}
			}

			return sections;
		}

		private void CreateNullHeaderSection()
		{
			var nullSection = new SectionHeader();

			nullSection.Name = 0;
			nullSection.Type = SectionType.Null;
			nullSection.Flags = 0;
			nullSection.Address = 0;
			nullSection.Offset = 0;
			nullSection.Size = 0;
			nullSection.Link = 0;
			nullSection.Info = 0;
			nullSection.AddressAlignment = 0;
			nullSection.EntrySize = 0;

			AddSectionHeaders(nullSection, string.Empty);
		}

		private void CreateSectionHeaderStringHeaderSection()
		{
			string name = ".shstrtab";
			sectionHeaderStringSection.Name = AddToSectionHeaderStringTable(name);
			sectionHeaderStringSection.Type = SectionType.StringTable;
			sectionHeaderStringSection.Flags = (SectionAttribute)0;
			sectionHeaderStringSection.Address = 0;
			sectionHeaderStringSection.Offset = 0;
			sectionHeaderStringSection.Size = 0;
			sectionHeaderStringSection.Link = 0;
			sectionHeaderStringSection.Info = 0;
			sectionHeaderStringSection.AddressAlignment = SectionAlignment;
			sectionHeaderStringSection.EntrySize = 0;

			AddSectionHeaders(sectionHeaderStringSection, name);
		}

		protected void WriteSectionHeaderStringSection()
		{
			ResolveSectionOffset(sectionHeaderStringSection);

			sectionHeaderStringSection.Size = (ulong)sectionHeaderStringTable.Count;
			writer.BaseStream.Position = sectionHeaderStringSection.Offset;
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

		private void CreateStringHeaderSection()
		{
			string name = ".strtab";
			stringSection.Name = AddToSectionHeaderStringTable(name);
			stringSection.Type = SectionType.StringTable;
			stringSection.Flags = (SectionAttribute)0;
			stringSection.Address = 0;
			stringSection.Offset = 0;
			stringSection.Size = 0;
			stringSection.Link = 0;
			stringSection.Info = 0;
			stringSection.AddressAlignment = SectionAlignment;
			stringSection.EntrySize = 0;

			AddSectionHeaders(stringSection, name);
		}

		protected void WriteStringSection()
		{
			ResolveSectionOffset(stringSection);

			stringSection.Size = (ulong)stringTable.Count;
			writer.BaseStream.Position = stringSection.Offset;
			writer.Write(stringTable.ToArray());
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

		private void CreateSymbolHeaderSection()
		{
			string name = ".symtab";

			symbolSection.Name = AddToSectionHeaderStringTable(name);
			symbolSection.Type = SectionType.SymbolTable;
			symbolSection.Flags = (SectionAttribute)0;
			symbolSection.Address = 0;
			symbolSection.Offset = 0;
			symbolSection.Size = 0;
			symbolSection.Link = 0;
			symbolSection.Info = 0;
			symbolSection.AddressAlignment = SectionAlignment;
			symbolSection.EntrySize = (ulong)SymbolEntry.GetEntrySize(elfType);

			AddSectionHeaders(symbolSection, name);
		}

		protected void WriteSymbolSection()
		{
			ResolveSectionOffset(symbolSection);

			writer.Position = symbolSection.Offset;

			// first entry is completely filled with zeros
			writer.WriteZeroBytes(SymbolEntry.GetEntrySize(elfType));

			int count = 1;

			foreach (var symbol in Symbols)
			{
				var symbolEntry = new SymbolEntry()
				{
					Name = AddToStringTable(symbol.Name),
					Value = symbol.VirtualAddress,
					Size = symbol.Size,
					SymbolBinding = SymbolBinding.Global,
					SymbolVisibility = SymbolVisibility.Default,
					SymbolType = symbol.SectionKind == SectionKind.Text ? SymbolType.Function : SymbolType.Object,
					SectionHeaderTableIndex = sectionToIndex[GetSectionHeader(symbol.SectionKind)],
				};

				symbolEntry.Write(elfType, writer);
				count++;

				break; // FIXME: temp
			}

			symbolSection.Size = (ulong)(count * SymbolEntry.GetEntrySize(elfType));
		}
	}
}
