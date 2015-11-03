// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.LinkerFormat.Elf;
using Mosa.Compiler.LinkerFormat.Elf32;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mosa.Compiler.Linker.Elf32
{
	public class Elf32 : BaseLinker
	{
		#region Data members

		protected Header elfheader = new Header();
		protected List<byte> stringTable = new List<byte>();
		protected SectionHeader stringSection = new SectionHeader();

		#endregion Data members

		public Elf32()
		{
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
			elfheader.CreateIdent(IdentClass.Class32, Endianness == Endianness.Little ? IdentData.Data2LSB : IdentData.Data2MSB, null);
			elfheader.ProgramHeaderOffset = Header.ElfHeaderSize;
			elfheader.ProgramHeaderNumber = sectons;
			elfheader.SectionHeaderNumber = (ushort)(sectons + 2);
			elfheader.SectionHeaderOffset = (uint)(elfheader.ProgramHeaderOffset + (Header.ProgramHeaderEntrySize * elfheader.ProgramHeaderNumber));
			elfheader.SectionHeaderStringIndex = (ushort)(sectons + 1);
			elfheader.Write(writer);
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

				pheader.Write(writer);
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

				var sheader = new SectionHeader();

				sheader.Name = AddToStringTable(section.Name);

				switch (section.SectionKind)
				{
					case SectionKind.Text: sheader.Type = SectionType.ProgBits; sheader.Flags = SectionAttribute.AllocExecute; break;
					case SectionKind.Data: sheader.Type = SectionType.ProgBits; sheader.Flags = SectionAttribute.Alloc | SectionAttribute.Write; break;
					case SectionKind.ROData: sheader.Type = SectionType.ProgBits; sheader.Flags = SectionAttribute.Alloc; break;
					case SectionKind.BSS: sheader.Type = SectionType.NoBits; sheader.Flags = SectionAttribute.Alloc | SectionAttribute.Write; break;
				}

				sheader.Address = (uint)section.VirtualAddress;
				sheader.Offset = section.FileOffset;
				sheader.Size = section.AlignedSize;
				sheader.Link = 0;
				sheader.Info = 0;
				sheader.AddressAlignment = section.SectionAlignment;
				sheader.EntrySize = 0;
				sheader.Write(writer);
			}

			WriteStringHeaderSection(writer);
		}

		private static void WriteNullHeaderSection(EndianAwareBinaryWriter writer)
		{
			var nullsection = new SectionHeader();
			nullsection.Name = 0;
			nullsection.Type = SectionType.Null;
			nullsection.Flags = 0;
			nullsection.Address = 0;
			nullsection.Offset = 0;
			nullsection.Size = 0;
			nullsection.Link = 0;
			nullsection.Info = 0;
			nullsection.AddressAlignment = 0;
			nullsection.EntrySize = 0;
			nullsection.Write(writer);
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
			stringSection.Write(writer);
		}

		private void WriteStringSection(BinaryWriter writer)
		{
			writer.BaseStream.Position = stringSection.Offset;
			writer.Write(stringTable.ToArray());
		}

		#region Internals

		/// <summary>
		/// Counts the valid sections.
		/// </summary>
		/// <returns>Determines the number of sections.</returns>
		private uint CountNonEmptySections()
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

		public uint AddToStringTable(string text)
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

		#endregion Internals
	}
}
