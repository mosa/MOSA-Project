/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.LinkerFormat.PE;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Mosa.Compiler.Linker.PE
{
	/// <summary>
	/// A Linker, which creates portable executable files.
	/// </summary>
	public class PELinker : BaseLinker
	{
		#region Constants

		/// <summary>
		/// Specifies the default section alignment in a PE file.
		/// </summary>
		private const uint FILE_SECTION_ALIGNMENT = 4096; // FIXME?: 0x200;

		/// <summary>
		/// Specifies the default section alignment in virtual memory.
		/// </summary>
		private const uint SECTION_ALIGNMENT = 0x1000;

		#endregion Constants

		#region Data members

		/// <summary>
		/// Holds the DOS header of the generated PE file.
		/// </summary>
		private ImageDosHeader dosHeader;

		/// <summary>
		/// Holds the PE headers.
		/// </summary>
		private ImageNtHeaders ntHeaders;

		/// <summary>
		/// Determines if the checksum of the generated executable must be set.
		/// </summary>
		private bool setChecksum;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="PELinker"/> class.
		/// </summary>
		public PELinker()
		{
			this.dosHeader = new ImageDosHeader();
			this.ntHeaders = new ImageNtHeaders();
			this.SectionAlignment = SECTION_ALIGNMENT;
			this.LoadSectionAlignment = FILE_SECTION_ALIGNMENT;
			this.setChecksum = true;

			AddSection(new PELinkerSection(SectionKind.Text, @".text", this.BaseAddress + SectionAlignment));
			AddSection(new PELinkerSection(SectionKind.Data, @".data", 0));
			AddSection(new PELinkerSection(SectionKind.ROData, @".rodata", 0));
			AddSection(new PELinkerSection(SectionKind.BSS, @".bss", 0));
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets or sets a value indicating whether a checksum is calculated for the linked binary.
		/// </summary>
		/// <value><c>true</c> if a checksum is calculated; otherwise, <c>false</c>.</value>
		public bool SetChecksum
		{
			get { return this.setChecksum; }
			set { this.setChecksum = value; }
		}

		#endregion Properties

		#region BaseLinker Overrides

		/// <summary>
		/// Verifies the parameters.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="System.ArgumentException"></exception>
		protected override void VerifyParameters()
		{
			base.VerifyParameters();

			if (LoadSectionAlignment < FILE_SECTION_ALIGNMENT)
				throw new ArgumentException(@"Section alignment must not be less than 512 bytes.", @"value");
			if ((LoadSectionAlignment & unchecked(FILE_SECTION_ALIGNMENT - 1)) != 0)
				throw new ArgumentException(@"Section alignment must be a multiple of 512 bytes.", @"value");

			if (SectionAlignment < SECTION_ALIGNMENT)
				throw new ArgumentException(@"Section alignment must not be less than 4K.", @"value");
			if ((SectionAlignment & unchecked(SECTION_ALIGNMENT - 1)) != 0)
				throw new ArgumentException(@"Section alignment must be a multiple of 4K.", @"value");
		}

		/// <summary>
		/// Creates the final linked file.
		/// </summary>
		protected override void CreateFile()
		{
			// Open the output file
			using (FileStream fs = new FileStream(this.OutputFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
			{
				using (EndianAwareBinaryWriter writer = new EndianAwareBinaryWriter(fs, Encoding.Unicode, Endianness))
				{
					// Write the PE headers
					WriteDosHeader(writer);
					WritePEHeader(writer);

					// Iterate all sections and store their data
					long position = writer.BaseStream.Position;
					foreach (PELinkerSection section in Sections)
					{
						if (section.Length > 0)
						{
							// Write the section
							section.Write(writer);

							// Add padding...
							position += section.Length;
							position += (this.LoadSectionAlignment - (position % this.LoadSectionAlignment));
							WritePaddingToPosition(writer, position);
						}
					}

					// Do we need to set the checksum?
					if (this.setChecksum)
					{
						// Flush all data to disk
						writer.Flush();

						// Write the checksum to the file
						ntHeaders.OptionalHeader.CheckSum = CalculateChecksum(this.OutputFile);
						fs.Position = this.dosHeader.e_lfanew;
						ntHeaders.Write(writer);
					}
				}
			}
		}

		#endregion BaseLinker Overrides

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
			this.dosHeader.e_magic = ImageDosHeader.DOS_HEADER_MAGIC;
			this.dosHeader.e_cblp = 0x0090;
			this.dosHeader.e_cp = 0x0003;
			this.dosHeader.e_cparhdr = 0x0004;
			this.dosHeader.e_maxalloc = 0xFFFF;
			this.dosHeader.e_sp = 0xb8;
			this.dosHeader.e_lfarlc = 0x0040;
			this.dosHeader.e_lfanew = 0x00000080;
			this.dosHeader.Write(writer);

			// Write the following 64 bytes, which represent the default x86 code to
			// print a message on the screen.
			byte[] message = new byte[] {
				0x0E, 0x1F, 0xBA, 0x0E, 0x00, 0xB4, 0x09, 0xCD, 0x21, 0xB8, 0x01, 0x4C, 0xCD, 0x21, 0x54, 0x68,
				0x69, 0x73, 0x20, 0x70, 0x72, 0x6F, 0x67, 0x72, 0x61, 0x6D, 0x20, 0x72, 0x65, 0x71, 0x75, 0x69,
				0x72, 0x65, 0x73, 0x20, 0x61, 0x20, 0x4D, 0x4F, 0x53, 0x41, 0x20, 0x70, 0x6F, 0x77, 0x65, 0x72,
				0x65, 0x64, 0x20, 0x4F, 0x53, 0x2E, 0x0D, 0x0D, 0x0A, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
			};
			writer.Write(message);
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
			ntHeaders.FileHeader.NumberOfSections = CountSections();
			ntHeaders.FileHeader.TimeDateStamp = (uint)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
			ntHeaders.FileHeader.PointerToSymbolTable = 0;
			ntHeaders.FileHeader.NumberOfSymbols = 0;
			ntHeaders.FileHeader.SizeOfOptionalHeader = 0x00E0;
			ntHeaders.FileHeader.Characteristics = 0x010E; // FIXME: Use an enum here

			// Prepare the "optional" headers
			ntHeaders.OptionalHeader.Magic = ImageOptionalHeader.IMAGE_OPTIONAL_HEADER_MAGIC;
			ntHeaders.OptionalHeader.MajorLinkerVersion = 6;
			ntHeaders.OptionalHeader.MinorLinkerVersion = 0;
			ntHeaders.OptionalHeader.SizeOfCode = (uint)AlignValue(GetSectionLength(SectionKind.Text), SectionAlignment);
			ntHeaders.OptionalHeader.SizeOfInitializedData = (uint)AlignValue(GetSectionLength(SectionKind.Data) + GetSectionLength(SectionKind.ROData), SectionAlignment);
			ntHeaders.OptionalHeader.SizeOfUninitializedData = (uint)AlignValue(GetSectionLength(SectionKind.BSS), SectionAlignment);
			ntHeaders.OptionalHeader.AddressOfEntryPoint = (uint)(EntryPoint.VirtualAddress - BaseAddress);
			ntHeaders.OptionalHeader.BaseOfCode = (uint)(GetSectionAddress(SectionKind.Text) - BaseAddress);

			long sectionAddress = GetSectionAddress(SectionKind.Data);
			if (sectionAddress != 0)
				ntHeaders.OptionalHeader.BaseOfData = (uint)(sectionAddress - this.BaseAddress);

			ntHeaders.OptionalHeader.ImageBase = (uint)this.BaseAddress; // FIXME: Linker Script/cmdline
			ntHeaders.OptionalHeader.SectionAlignment = SectionAlignment; // FIXME: Linker Script/cmdline
			ntHeaders.OptionalHeader.FileAlignment = LoadSectionAlignment; // FIXME: Linker Script/cmdline
			ntHeaders.OptionalHeader.MajorOperatingSystemVersion = 4;
			ntHeaders.OptionalHeader.MinorOperatingSystemVersion = 0;
			ntHeaders.OptionalHeader.MajorImageVersion = 0;
			ntHeaders.OptionalHeader.MinorImageVersion = 0;
			ntHeaders.OptionalHeader.MajorSubsystemVersion = 4;
			ntHeaders.OptionalHeader.MinorSubsystemVersion = 0;
			ntHeaders.OptionalHeader.Win32VersionValue = 0;
			ntHeaders.OptionalHeader.SizeOfImage = CalculateSizeOfImage();
			ntHeaders.OptionalHeader.SizeOfHeaders = LoadSectionAlignment; // FIXME: Use the full header size
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
			ntHeaders.OptionalHeader.DataDirectory[14].VirtualAddress = 0;// (uint)GetSymbol(CLI_HEADER.SymbolName).VirtualAddress.ToInt64();
			ntHeaders.OptionalHeader.DataDirectory[14].Size = 0; // CLI_HEADER.Length;

			ntHeaders.Write(writer);

			// Write the section headers
			uint address = this.LoadSectionAlignment;
			
			foreach (LinkerSection section in Sections)
			{
				if (section.Length == 0)
					continue;

				ImageSectionHeader image = new ImageSectionHeader();
				image.Name = section.Name;
				image.VirtualSize = (uint)section.Length;
				image.VirtualAddress = (uint)(section.VirtualAddress - this.BaseAddress);

				if (section.SectionKind != SectionKind.BSS)
					image.SizeOfRawData = (uint)section.Length;

				image.PointerToRawData = address;
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

				address += (uint)section.Length;
				address = AlignValue(address, LoadSectionAlignment);
			}

			WritePaddingToPosition(writer, LoadSectionAlignment);
		}

		/// <summary>
		/// Counts the valid sections.
		/// </summary>
		/// <returns>Determines the number of sections.</returns>
		private ushort CountSections()
		{
			ushort sections = 0;
			foreach (LinkerSection section in Sections)
			{
				if (section.Length > 0)
					sections++;
			}
			return sections;
		}

		private uint CalculateSizeOfImage()
		{
			// Reset the size of the image
			uint virtualSizeOfImage = SectionAlignment, sectionEnd;

			// Move all sections to their right positions
			foreach (LinkerSection sections in Sections)
			{
				// Only use a section with something inside
				if (sections.Length > 0)
				{
					sectionEnd = (uint)(sections.VirtualAddress + AlignValue((uint)sections.Length, SectionAlignment));

					if (sectionEnd > virtualSizeOfImage)
						virtualSizeOfImage = sectionEnd;
				}
			}

			return virtualSizeOfImage - (uint)BaseAddress;
		}

		private uint GetSectionAddress(SectionKind sectionKind)
		{
			return (uint)GetSection(sectionKind).VirtualAddress;
		}

		private uint GetSectionLength(SectionKind sectionKind)
		{
			return (uint)GetSection(sectionKind).Length;
		}

		private uint AlignValue(uint value, uint alignment)
		{
			uint off = (value % alignment);
			if (off != 0)
				value += (alignment - off);

			return value;
		}

		/// <summary>
		/// Adds padding to the writer to ensure the next write starts at a specific virtualAddress.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="address">The address.</param>
		private void WritePaddingToPosition(BinaryWriter writer, long address)
		{
			long position = writer.BaseStream.Position;
			Debug.Assert(position <= address, @"Passed the address.");
			if (position < address)
			{
				//writer.Write(new byte[address - position]);
				for (int i = (int)(address - position); i > 0; i--)
				{
					writer.Write((byte)0);
				}
			}
		}

		private uint CalculateChecksum(string file)
		{
			uint csum = 0;

			using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				using (BinaryReader reader = new EndianAwareBinaryReader(stream, Endianness.Little))
				{
					uint l = (uint)stream.Length;
					for (uint p = 0; p < l; p += 2)
					{
						csum += reader.ReadUInt16();
						if (csum > 0x0000FFFF)
						{
							csum = (csum & 0xFFFF) + (csum >> 16);
						}
					}

					csum = (csum & 0xFFFF) + (csum >> 16);
					csum += l;
				}
			}

			return csum;
		}

		#endregion Internals
	}
}