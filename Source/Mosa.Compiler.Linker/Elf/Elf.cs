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

		protected ElfType ElfType;
		protected Header elfheader = new Header();
		protected SectionHeader stringSection = new SectionHeader();
		protected List<byte> stringTable = new List<byte>();

		#endregion Data members

		public ElfLinker(ElfType elfType)
		{
			ElfType = elfType;
			SectionAlignment = 0x1000;
			BaseFileOffset = 0x1000;

			AddSection(new LinkerSection(SectionKind.Text, ".text", SectionAlignment));
			AddSection(new LinkerSection(SectionKind.Data, ".data", SectionAlignment));
			AddSection(new LinkerSection(SectionKind.ROData, ".rodata", SectionAlignment));
			AddSection(new LinkerSection(SectionKind.BSS, ".bss", SectionAlignment));

			stringTable.Add((byte)'\0');
		}

		/// <summary>
		/// Emits the implementation.
		/// </summary>
		/// <param name="stream">The stream.</param>
		protected override void EmitImplementation(Stream stream)
		{
			var writer = new EndianAwareBinaryWriter(stream, Encoding.Unicode, Endianness);

			// Write ELF header
			WriteElfHeader(writer);

			// Write program headers
			WriteProgramHeaders(writer);

			// Write section headers
			WriteSectionHeaders(writer);

			// Write sections
			foreach (var section in Sections)
			{
				stream.Position = section.FileOffset;
				section.WriteTo(stream);
			}

			WriteStringSection(writer);
		}

		private void WriteElfHeader(EndianAwareBinaryWriter writer)
		{
			ushort sectons = (ushort)(CountNonEmptySections() + 1);

			elfheader.Type = FileType.Executable;
			elfheader.Machine = (MachineType)MachineID;
			elfheader.EntryAddress = (uint)EntryPoint.VirtualAddress;
			elfheader.CreateIdent((ElfType == ElfType.Elf32) ? IdentClass.Class32 : IdentClass.Class64, Endianness == Endianness.Little ? IdentData.Data2LSB : IdentData.Data2MSB, null);
			elfheader.ProgramHeaderOffset = (ElfType == ElfType.Elf32) ? Header.ElfHeaderSize32 : Header.ElfHeaderSize64;
			elfheader.ProgramHeaderNumber = sectons;
			elfheader.SectionHeaderNumber = (ushort)(sectons + 2);
			elfheader.SectionHeaderOffset = (uint)(elfheader.ProgramHeaderOffset + (((ElfType == ElfType.Elf32) ? Header.ProgramHeaderEntrySize32 : Header.ProgramHeaderEntrySize64) * elfheader.ProgramHeaderNumber));
			elfheader.SectionHeaderStringIndex = (ushort)(sectons + 1);

			elfheader.Write(ElfType, writer);
		}

		private void WriteProgramHeaders(EndianAwareBinaryWriter writer)
		{
			writer.Position = elfheader.ProgramHeaderOffset;

			foreach (var section in Sections)
			{
				if (section.Size == 0 && section.SectionKind != SectionKind.BSS)
					continue;

				var pheader = new ProgramHeader
				{
					Alignment = section.SectionAlignment,
					FileSize = section.AlignedSize,
					MemorySize = section.AlignedSize,
					Offset = section.FileOffset,
					VirtualAddress = (uint)section.VirtualAddress,
					PhysicalAddress = (uint)section.VirtualAddress,
					Type = ProgramHeaderType.Load,
					Flags =
						(section.SectionKind == SectionKind.Text) ? ProgramHeaderFlags.Read | ProgramHeaderFlags.Execute :
						(section.SectionKind == SectionKind.ROData) ? ProgramHeaderFlags.Read : ProgramHeaderFlags.Read | ProgramHeaderFlags.Write
				};

				pheader.Write(ElfType, writer);
			}
		}

		private void WriteSectionHeaders(EndianAwareBinaryWriter writer)
		{
			writer.Position = elfheader.SectionHeaderOffset;

			WriteNullHeaderSection(writer);

			foreach (var section in Sections)
			{
				if (section.Size == 0 && section.SectionKind != SectionKind.BSS)
					continue;

				var sectionHeader = new SectionHeader();

				sectionHeader.Name = AddToStringTable(section.Name);

				switch (section.SectionKind)
				{
					case SectionKind.Text: sectionHeader.Type = SectionType.ProgBits; sectionHeader.Flags = SectionAttribute.AllocExecute; break;
					case SectionKind.Data: sectionHeader.Type = SectionType.ProgBits; sectionHeader.Flags = SectionAttribute.Alloc | SectionAttribute.Write; break;
					case SectionKind.ROData: sectionHeader.Type = SectionType.ProgBits; sectionHeader.Flags = SectionAttribute.Alloc; break;
					case SectionKind.BSS: sectionHeader.Type = SectionType.NoBits; sectionHeader.Flags = SectionAttribute.Alloc | SectionAttribute.Write; break;
				}

				sectionHeader.Address = (uint)section.VirtualAddress;
				sectionHeader.Offset = section.FileOffset;
				sectionHeader.Size = section.AlignedSize;
				sectionHeader.Link = 0;
				sectionHeader.Info = 0;
				sectionHeader.AddressAlignment = section.SectionAlignment;
				sectionHeader.EntrySize = 0;

				sectionHeader.Write(ElfType, writer);
			}

			WriteStringHeaderSection(writer);
		}

		private void WriteNullHeaderSection(EndianAwareBinaryWriter writer)
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

			nullSection.Write(ElfType, writer);
		}

		private void WriteStringHeaderSection(EndianAwareBinaryWriter writer)
		{
			stringSection.Name = AddToStringTable(".shstrtab");
			stringSection.Type = SectionType.StringTable;
			stringSection.Flags = (SectionAttribute)0;
			stringSection.Address = 0;
			stringSection.Offset = GetSection(SectionKind.BSS).FileOffset + GetSection(SectionKind.BSS).AlignedSize;
			stringSection.Size = (uint)stringTable.Count;
			stringSection.Link = 0;
			stringSection.Info = 0;
			stringSection.AddressAlignment = SectionAlignment;
			stringSection.EntrySize = 0;

			stringSection.Write(ElfType, writer);
		}

		protected void WriteStringSection(BinaryWriter writer)
		{
			writer.BaseStream.Position = stringSection.Offset;
			writer.Write(stringTable.ToArray());
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

			return index + 1;
		}
	}
}
