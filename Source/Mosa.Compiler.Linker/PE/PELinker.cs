// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.LinkerFormat.PE;
using System;
using System.IO;
using System.Text;

namespace Mosa.Compiler.Linker.PE
{
	public class PELinker : BaseLinker
	{
		#region Constants

		/// <summary>
		/// Specifies the default section alignment in a PE file.
		/// </summary>
		private const uint FILE_SECTION_ALIGNMENT = 0x1000;

		// Write the following 64 bytes, which represent the default x86 code to
		// print a message on the screen.
		private static readonly byte[] dosmessage = new byte[] {
				0x0E, 0x1F, 0xBA, 0x0E, 0x00, 0xB4, 0x09, 0xCD, 0x21, 0xB8, 0x01, 0x4C, 0xCD, 0x21, 0x54, 0x68,
				0x69, 0x73, 0x20, 0x70, 0x72, 0x6F, 0x67, 0x72, 0x61, 0x6D, 0x20, 0x72, 0x65, 0x71, 0x75, 0x69,
				0x72, 0x65, 0x73, 0x20, 0x61, 0x20, 0x4D, 0x4F, 0x53, 0x41, 0x20, 0x70, 0x6F, 0x77, 0x65, 0x72,
				0x65, 0x64, 0x20, 0x4F, 0x53, 0x2E, 0x0D, 0x0D, 0x0A, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
			};

		#endregion Constants

		#region Data members

		private ImageNtHeaders ntHeaders = new ImageNtHeaders();

		private ImageDosHeader dosHeader = new ImageDosHeader();

		#endregion Data members

		public PELinker()
		{
			// Set the default section alignment in virtual memory.
			SectionAlignment = 0x1000;
			BaseFileOffset = FILE_SECTION_ALIGNMENT;

			AddSection(new LinkerSection(SectionKind.Text, ".text", SectionAlignment));
			AddSection(new LinkerSection(SectionKind.Data, ".data", SectionAlignment));
			AddSection(new LinkerSection(SectionKind.ROData, ".rodata", SectionAlignment));
			AddSection(new LinkerSection(SectionKind.BSS, ".bss", SectionAlignment));
		}

		public virtual void Initalize(ulong baseAddress, Endianness endianness, ushort machineID)
		{
			base.Initialize(baseAddress, endianness, machineID);
			Endianness = Common.Endianness.Little;
		}

		protected override void EmitImplementation(Stream stream)
		{
			var writer = new EndianAwareBinaryWriter(stream, Encoding.Unicode, Endianness);

			// Write the PE headers
			WriteDosHeader(writer);
			WritePEHeader(writer);

			foreach (var section in Sections)
			{
				stream.Position = (long)FILE_SECTION_ALIGNMENT;
				section.WriteTo(stream);
			}
		}

		#region Internals

		/// <summary>
		/// Writes the dos header of the PE file.
		/// </summary>
		/// <param name="writer">The writer.</param>
		private void WriteDosHeader(EndianAwareBinaryWriter writer)
		{
			/*
			 * This code block generates the default DOS header of a PE image.
			 * These constants are not further documented here, please consult
			 * MSDN for their meaning.
			 */

			dosHeader.e_magic = ImageDosHeader.DOS_HEADER_MAGIC;
			dosHeader.e_cblp = 0x0090;
			dosHeader.e_cp = 0x0003;
			dosHeader.e_cparhdr = 0x0004;
			dosHeader.e_maxalloc = 0xFFFF;
			dosHeader.e_sp = 0xb8;
			dosHeader.e_lfarlc = 0x0040;
			dosHeader.e_lfanew = 0x00000080;
			dosHeader.Write(writer);

			writer.Write(dosmessage);
		}

		/// <summary>
		/// Writes the PE header.
		/// </summary>
		/// <param name="writer">The writer.</param>
		private void WritePEHeader(EndianAwareBinaryWriter writer)
		{
			// Write the PE signature and headers
			ntHeaders.Signature = ImageNtHeaders.PE_SIGNATURE;

			// Prepare the file header
			ntHeaders.FileHeader.Machine = ImageFileHeader.IMAGE_FILE_MACHINE_I386;
			ntHeaders.FileHeader.NumberOfSections = (ushort)CountNonEmptySections();
			ntHeaders.FileHeader.TimeDateStamp = (uint)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
			ntHeaders.FileHeader.PointerToSymbolTable = 0;
			ntHeaders.FileHeader.NumberOfSymbols = 0;
			ntHeaders.FileHeader.SizeOfOptionalHeader = 0x00E0;
			ntHeaders.FileHeader.Characteristics = 0x010E;

			// Prepare the "optional" headers
			ntHeaders.OptionalHeader.Magic = ImageOptionalHeader.IMAGE_OPTIONAL_HEADER_MAGIC;
			ntHeaders.OptionalHeader.MajorLinkerVersion = 6;
			ntHeaders.OptionalHeader.MinorLinkerVersion = 0;
			ntHeaders.OptionalHeader.SizeOfCode = GetSection(SectionKind.Text).AlignedSize;
			ntHeaders.OptionalHeader.SizeOfInitializedData = GetSection(SectionKind.Data).AlignedSize + GetSection(SectionKind.ROData).AlignedSize;
			ntHeaders.OptionalHeader.SizeOfUninitializedData = GetSection(SectionKind.BSS).AlignedSize;
			ntHeaders.OptionalHeader.AddressOfEntryPoint = (uint)(EntryPoint.VirtualAddress - BaseAddress);
			ntHeaders.OptionalHeader.BaseOfCode = (uint)(GetSection(SectionKind.Text).VirtualAddress - BaseAddress);

			ulong sectionAddress = GetSection(SectionKind.Data).VirtualAddress;
			if (sectionAddress != 0)
			{
				ntHeaders.OptionalHeader.BaseOfData = (uint)(sectionAddress - BaseAddress);
			}

			ntHeaders.OptionalHeader.ImageBase = (uint)BaseAddress;
			ntHeaders.OptionalHeader.SectionAlignment = SectionAlignment;
			ntHeaders.OptionalHeader.FileAlignment = FILE_SECTION_ALIGNMENT;
			ntHeaders.OptionalHeader.MajorOperatingSystemVersion = 4;
			ntHeaders.OptionalHeader.MinorOperatingSystemVersion = 0;
			ntHeaders.OptionalHeader.MajorImageVersion = 0;
			ntHeaders.OptionalHeader.MinorImageVersion = 0;
			ntHeaders.OptionalHeader.MajorSubsystemVersion = 4;
			ntHeaders.OptionalHeader.MinorSubsystemVersion = 0;
			ntHeaders.OptionalHeader.Win32VersionValue = 0;
			ntHeaders.OptionalHeader.SizeOfImage = CalculateSizeOfImage();
			ntHeaders.OptionalHeader.SizeOfHeaders = FILE_SECTION_ALIGNMENT;
			ntHeaders.OptionalHeader.CheckSum = 0;
			ntHeaders.OptionalHeader.Subsystem = 0x03;
			ntHeaders.OptionalHeader.DllCharacteristics = 0x0540;
			ntHeaders.OptionalHeader.SizeOfStackReserve = 0x100000;
			ntHeaders.OptionalHeader.SizeOfStackCommit = 0x1000;
			ntHeaders.OptionalHeader.SizeOfHeapReserve = 0x100000;
			ntHeaders.OptionalHeader.SizeOfHeapCommit = 0x1000;
			ntHeaders.OptionalHeader.LoaderFlags = 0;
			ntHeaders.OptionalHeader.NumberOfRvaAndSizes = ImageOptionalHeader.IMAGE_NUMBEROF_DIRECTORY_ENTRIES;
			ntHeaders.OptionalHeader.DataDirectory = new ImageDataDirectory[ImageOptionalHeader.IMAGE_NUMBEROF_DIRECTORY_ENTRIES];

			// Populate the CIL data directory
			ntHeaders.OptionalHeader.DataDirectory[14].VirtualAddress = 0;
			ntHeaders.OptionalHeader.DataDirectory[14].Size = 0;

			ntHeaders.Write(writer);

			foreach (var section in Sections)
			{
				if (section.Size == 0)
					continue;

				ImageSectionHeader image = new ImageSectionHeader();
				image.Name = section.Name;
				image.VirtualSize = section.Size;
				image.VirtualAddress = (uint)(section.VirtualAddress - BaseAddress);
				image.SizeOfRawData = (section.SectionKind == SectionKind.BSS) ? 0 : section.Size;
				image.PointerToRawData = section.FileOffset;
				image.PointerToRelocations = 0;
				image.PointerToLinenumbers = 0;
				image.NumberOfRelocations = 0;
				image.NumberOfLinenumbers = 0;

				switch (section.SectionKind)
				{
					case SectionKind.BSS: image.Characteristics = 0x40000000 | 0x80000000 | 0x00000080; break;
					case SectionKind.Data: image.Characteristics = 0x40000000 | 0x80000000 | 0x00000040; break;
					case SectionKind.ROData: image.Characteristics = 0x40000000 | 0x00000040; break;
					case SectionKind.Text: image.Characteristics = 0x20000000 | 0x40000000 | 0x80000000 | 0x00000020; break;
				}

				image.Write(writer);
			}
		}

		/// <summary>
		/// Counts the valid sections.
		/// </summary>
		/// <returns>Determines the number of sections.</returns>
		private uint CountNonEmptySections()
		{
			uint sections = 0;

			foreach (var section in Sections)
			{
				if (section.Size != 0)
				{
					sections++;
				}
			}

			return sections;
		}

		private uint CalculateSizeOfImage()
		{
			uint size = FILE_SECTION_ALIGNMENT;

			foreach (var sections in Sections)
			{
				size = size + sections.AlignedSize;
			}

			return size;
		}

		#endregion Internals
	}
}