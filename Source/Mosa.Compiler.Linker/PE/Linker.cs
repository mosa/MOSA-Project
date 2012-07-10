/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Mosa.Compiler.Common;
using Mosa.Compiler.LinkerFormat.PE;

namespace Mosa.Compiler.Linker.PE
{
	/// <summary>
	/// A Linker, which creates portable executable files.
	/// </summary>
	public class Linker : BaseLinker
	{
		#region Constants

		/// <summary>
		/// Specifies the default section alignment in a PE file.
		/// </summary>
		private const uint FILE_SECTION_ALIGNMENT = 0x200;

		/// <summary>
		/// Specifies the default section alignment in virtual memory.
		/// </summary>
		private const uint SECTION_ALIGNMENT = 0x1000;

		#endregion // Constants

		#region Data members

		/// <summary>
		/// Holds the DOS header of the generated PE file.
		/// </summary>
		private ImageDosHeader dosHeader;

		/// <summary>
		/// Holds the PE headers.
		/// </summary>
		private ImageNtHeaders ntHeaders;

		///// <summary>
		///// Holds the CLI header.
		///// </summary>
		//private CLI_HEADER cilHeader;

		/// <summary>
		/// Holds the file alignment used for this PE file.
		/// </summary>
		private uint fileAlignment;

		/// <summary>
		/// Holds the section alignment used for this PE file.
		/// </summary>
		private uint sectionAlignment;

		/// <summary>
		/// Holds the sections of the PE file.
		/// </summary>
		private Dictionary<SectionKind, LinkerSection> sections;

		/// <summary>
		/// Determines if the checksum of the generated executable must be set.
		/// </summary>
		private bool setChecksum;

		/// <summary>
		/// Flag, if the symbols have been resolved.
		/// </summary>
		private bool symbolsResolved;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Linker"/> class.
		/// </summary>
		public Linker()
		{
			this.dosHeader = new ImageDosHeader();
			this.ntHeaders = new ImageNtHeaders();
			this.sectionAlignment = SECTION_ALIGNMENT;
			this.fileAlignment = FILE_SECTION_ALIGNMENT;
			this.setChecksum = true;

			// Create the default section set
			this.sections = new Dictionary<SectionKind, LinkerSection>() 
			{
				{ SectionKind.Text, new Section(SectionKind.Text, @".text", new IntPtr(this.BaseAddress + this.sectionAlignment)) },
				{ SectionKind.Data, new Section(SectionKind.Data, @".data", IntPtr.Zero) },
				{ SectionKind.ROData, new Section(SectionKind.ROData, @".rodata", IntPtr.Zero) },
				{ SectionKind.BSS, new Section(SectionKind.BSS, @".bss", IntPtr.Zero) }
			};
		}

		#endregion // Construction

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

		/// <summary>
		/// Gets or sets the file alignment in bytes.
		/// </summary>
		/// <value>The file alignment in bytes.</value>
		public uint FileAlignment
		{
			get { return this.fileAlignment; }
			set
			{
				if (value < FILE_SECTION_ALIGNMENT)
					throw new ArgumentException(@"Section alignment must not be less than 512 bytes.", @"value");
				if ((value & unchecked(FILE_SECTION_ALIGNMENT - 1)) != 0)
					throw new ArgumentException(@"Section alignment must be a multiple of 512 bytes.", @"value");

				this.fileAlignment = value;
			}
		}

		/// <summary>
		/// Gets or sets the section alignment in bytes.
		/// </summary>
		/// <value>The section alignment in bytes.</value>
		public uint SectionAlignment
		{
			get { return this.sectionAlignment; }
			set
			{
				if (value < SECTION_ALIGNMENT)
					throw new ArgumentException(@"Section alignment must not be less than 4K.", @"value");
				if ((value & unchecked(SECTION_ALIGNMENT - 1)) != 0)
					throw new ArgumentException(@"Section alignment must be a multiple of 4K.", @"value");

				this.sectionAlignment = value;
			}
		}

		#endregion // Properties

		#region AssembyLinkerStageBase Overrides

		/// <summary>
		/// A request to patch already emitted code by storing the calculated virtualAddress value.
		/// </summary>
		/// <param name="linkType">Type of the link.</param>
		/// <param name="methodAddress">The virtual virtualAddress of the method whose code is being patched.</param>
		/// <param name="methodOffset">The value to store at the position in code.</param>
		/// <param name="methodRelativeBase">The method relative base.</param>
		/// <param name="targetAddress">The position in code, where it should be patched.</param>
		protected override void ApplyPatch(LinkType linkType, long methodAddress, long methodOffset, long methodRelativeBase, long targetAddress)
		{
			if (!symbolsResolved)
				throw new InvalidOperationException(@"Can't apply patches - symbols not resolved.");

			// Retrieve the text section
			Section text = (Section)GetSection(SectionKind.Text);
			// Calculate the patch offset
			long offset = (methodAddress - text.VirtualAddress.ToInt64()) + methodOffset;

			if ((linkType & LinkType.KindMask) == LinkType.AbsoluteAddress)
			{
				// FIXME: Need a .reloc section with a relocation entry if the module is moved in virtual memory
				// the runtime loader must patch this link request, we'll fail it until we can do relocations.
				//throw new NotSupportedException(@".reloc section not supported.");
			}
			else
			{
				// Change the absolute into a relative offset
				targetAddress = targetAddress - (methodAddress + methodRelativeBase);
			}

			// Save the stream position
			text.ApplyPatch(offset, linkType, targetAddress);
		}

		/// <summary>
		/// Retrieves a linker section by its type.
		/// </summary>
		/// <param name="sectionKind">The type of the section to retrieve.</param>
		/// <returns>The retrieved linker section.</returns>
		public override LinkerSection GetSection(SectionKind sectionKind)
		{
			return this.sections[sectionKind];
		}

		/// <summary>
		/// Determines whether the specified symbol is resolved.
		/// </summary>
		/// <param name="symbol">The symbol.</param>
		/// <param name="virtualAddress">The virtualAddress.</param>
		/// <returns>
		/// 	<c>true</c> if the specified symbol is resolved; otherwise, <c>false</c>.
		/// </returns>
		protected override bool IsResolved(string symbol, out long virtualAddress)
		{
			virtualAddress = 0;
			return (this.symbolsResolved == true && base.IsResolved(symbol, out virtualAddress) == true);
		}

		/// <summary>
		/// Gets the load alignment of sections.
		/// </summary>
		/// <value>The load alignment.</value>
		public override long LoadSectionAlignment
		{
			get { return this.fileAlignment; }
		}

		/// <summary>
		/// Retrieves the collection of sections created during compilation.
		/// </summary>
		/// <value>The sections collection.</value>
		public override ICollection<LinkerSection> Sections
		{
			get { return this.sections.Values; }
		}

		/// <summary>
		/// Gets the virtual alignment of sections.
		/// </summary>
		/// <value>The virtual section alignment.</value>
		public override long VirtualSectionAlignment
		{
			get { return this.sectionAlignment; }
		}

		/// <summary>
		/// Allocates a symbol of the given name in the specified section.
		/// </summary>
		/// <param name="section">The executable section to allocate From.</param>
		/// <param name="size">The number of bytes to allocate. If zero, indicates an unknown amount of memory is required.</param>
		/// <param name="alignment">The alignment. A value of zero indicates the use of a default alignment for the section.</param>
		/// <returns>
		/// A stream, which can be used to populate the section.
		/// </returns>
		protected override Stream Allocate(SectionKind section, int size, int alignment)
		{
			Section linkerSection = (Section)GetSection(section);
			return linkerSection.Allocate(size, alignment);
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public override void Run()
		{
			if (String.IsNullOrEmpty(this.OutputFile))
				throw new ArgumentException(@"Invalid argument.", @"outputFile");

			// Layout the sections in memory
			LayoutSections();

			// Resolve all symbols first
			base.Run();

			// Persist the PE file now
			CreatePEFile();
		}

		#endregion // AssembyLinkerStageBase Overrides

		#region Internals

		/// <summary>
		/// Creates the PE file.
		/// </summary>
		private void CreatePEFile()
		{
			// Open the output file
			using (FileStream fs = new FileStream(this.OutputFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
			{
				using (EndianAwareBinaryWriter writer = new EndianAwareBinaryWriter(fs, Encoding.Unicode, true))
				{
					// Write the PE headers
					WriteDosHeader(writer);
					WritePEHeader(writer);

					// Iterate all sections and store their data
					long position = writer.BaseStream.Position;
					foreach (Section section in this.sections.Values)
					{
						if (section.Length > 0)
						{
							// Write the section
							section.Write(writer);

							// Add padding...
							position += section.Length;
							position += (this.fileAlignment - (position % this.fileAlignment));
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

		/// <summary>
		/// Adjusts the section addresses and performs a proper layout.
		/// </summary>
		private void LayoutSections()
		{
			/*
						// Reset the size of the image
						this.virtualSizeOfImage = this.sectionAlignment;
						this.fileSizeOfImage = this.fileAlignment;

						// Move all sections to their right positions
						Dictionary<SectionKind, LinkerSection> usedSections = new Dictionary<SectionKind, LinkerSection>();
						foreach (LinkerSection ls in this.sections.Values)
						{
							// Only use a section with something inside
							if (ls.Length != 0)
							{
								// Set the section virtualAddress
								ls.Address = new IntPtr(this.BaseAddress + this.virtualSizeOfImage);
								ls.Offset = this.fileSizeOfImage;

								// Update the file size
								this.fileSizeOfImage += (uint)ls.Length;
								this.fileSizeOfImage = AlignValue(this.fileSizeOfImage, this.fileAlignment);

								// Update the virtual size
								this.virtualSizeOfImage += (uint)ls.Length;
								this.virtualSizeOfImage = AlignValue(this.virtualSizeOfImage, this.sectionAlignment);

								// Copy the section
								usedSections.Add(ls.SectionKind, ls);
							}
						}

						this.sections = usedSections;
			*/
			// We've resolved all symbols, allow IsResolved to succeed
			symbolsResolved = true;
		}

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
			ntHeaders.OptionalHeader.SizeOfCode = AlignValue(GetSectionLength(SectionKind.Text), this.sectionAlignment);
			ntHeaders.OptionalHeader.SizeOfInitializedData = AlignValue(GetSectionLength(SectionKind.Data) + GetSectionLength(SectionKind.ROData), this.sectionAlignment);
			ntHeaders.OptionalHeader.SizeOfUninitializedData = AlignValue(GetSectionLength(SectionKind.BSS), this.sectionAlignment);
			ntHeaders.OptionalHeader.AddressOfEntryPoint = (uint)(this.EntryPoint.VirtualAddress.ToInt64() - this.BaseAddress);
			ntHeaders.OptionalHeader.BaseOfCode = (uint)(GetSectionAddress(SectionKind.Text) - this.BaseAddress);

			long sectionAddress = GetSectionAddress(SectionKind.Data);
			if (sectionAddress != 0)
				ntHeaders.OptionalHeader.BaseOfData = (uint)(sectionAddress - this.BaseAddress);

			ntHeaders.OptionalHeader.ImageBase = (uint)this.BaseAddress; // FIXME: Linker Script/cmdline
			ntHeaders.OptionalHeader.SectionAlignment = this.sectionAlignment; // FIXME: Linker Script/cmdline
			ntHeaders.OptionalHeader.FileAlignment = this.fileAlignment; // FIXME: Linker Script/cmdline
			ntHeaders.OptionalHeader.MajorOperatingSystemVersion = 4;
			ntHeaders.OptionalHeader.MinorOperatingSystemVersion = 0;
			ntHeaders.OptionalHeader.MajorImageVersion = 0;
			ntHeaders.OptionalHeader.MinorImageVersion = 0;
			ntHeaders.OptionalHeader.MajorSubsystemVersion = 4;
			ntHeaders.OptionalHeader.MinorSubsystemVersion = 0;
			ntHeaders.OptionalHeader.Win32VersionValue = 0;
			ntHeaders.OptionalHeader.SizeOfImage = CalculateSizeOfImage();
			ntHeaders.OptionalHeader.SizeOfHeaders = this.fileAlignment; // FIXME: Use the full header size
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
			uint address = this.fileAlignment;
			foreach (LinkerSection section in this.sections.Values)
			{
				if (section.Length > 0)
				{
					ImageSectionHeader ish = new ImageSectionHeader();
					ish.Name = section.Name;
					ish.VirtualSize = (uint)section.Length;
					ish.VirtualAddress = (uint)(section.VirtualAddress.ToInt64() - this.BaseAddress);

					if (section.SectionKind != SectionKind.BSS)
						ish.SizeOfRawData = (uint)section.Length;

					ish.PointerToRawData = address;
					ish.PointerToRelocations = 0;
					ish.PointerToLinenumbers = 0;
					ish.NumberOfRelocations = 0;
					ish.NumberOfLinenumbers = 0;

					switch (section.SectionKind)
					{
						case SectionKind.BSS:
							ish.Characteristics = 0x40000000 | 0x80000000 | 0x00000080;
							break;

						case SectionKind.Data:
							ish.Characteristics = 0x40000000 | 0x80000000 | 0x00000040;
							break;

						case SectionKind.ROData:
							ish.Characteristics = 0x40000000 | 0x00000040;
							break;

						case SectionKind.Text:
							ish.Characteristics = 0x20000000 | 0x40000000 | 0x80000000 | 0x00000020;
							break;
					}

					ish.Write(writer);

					address += (uint)section.Length;
					address = AlignValue(address, this.fileAlignment);
				}
			}

			WritePaddingToPosition(writer, this.fileAlignment);
		}

		/// <summary>
		/// Counts the valid sections.
		/// </summary>
		/// <returns>Determines the number of sections.</returns>
		private ushort CountSections()
		{
			ushort sections = 0;
			foreach (LinkerSection ls in this.sections.Values)
			{
				if (ls.Length > 0)
					sections++;
			}
			return sections;
		}

		private uint CalculateSizeOfImage()
		{
			// Reset the size of the image
			uint virtualSizeOfImage = this.sectionAlignment, sectionEnd;

			// Move all sections to their right positions
			foreach (LinkerSection ls in this.sections.Values)
			{
				// Only use a section with something inside
				if (ls.Length > 0)
				{
					sectionEnd = (uint)(ls.VirtualAddress.ToInt32() + AlignValue(ls.Length, this.sectionAlignment));
					if (sectionEnd > virtualSizeOfImage)
						virtualSizeOfImage = sectionEnd;
				}
			}

			return virtualSizeOfImage - (uint)this.BaseAddress;
		}

		private long GetSectionAddress(SectionKind sectionKind)
		{
			LinkerSection section;
			if (this.sections.TryGetValue(sectionKind, out section) && section.Length > 0)
			{
				return (uint)section.VirtualAddress.ToInt64();
			}

			return 0L;
		}

		private uint GetSectionLength(SectionKind sectionKind)
		{
			LinkerSection section;
			if (this.sections.TryGetValue(sectionKind, out section) && section.Length > 0)
				return (uint)section.Length;

			return 0;
		}

		private long AlignValue(long value, uint alignment)
		{
			long off = (value % alignment);
			if (0 != off)
				value += (alignment - off);

			return value;
		}

		private uint AlignValue(uint value, uint alignment)
		{
			uint off = (value % alignment);
			if (0 != off)
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
				writer.Write(new byte[address - position]);
			}
		}

		private uint CalculateChecksum(string file)
		{
			uint csum = 0;

			using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				using (BinaryReader reader = new EndianAwareBinaryReader(stream, true))
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

		#endregion // Internals
	}
}
