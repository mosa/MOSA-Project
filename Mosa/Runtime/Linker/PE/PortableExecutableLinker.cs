/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.IO;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Vm;
using System.Text;
using System.Diagnostics;

namespace Mosa.Runtime.Linker.PE
{
    /// <summary>
    /// A Linker, which creates portable executable files.
    /// </summary>
    public sealed class PortableExecutableLinker : AssemblyLinkerStageBase
    {
        #region Constants

        /// <summary>
        /// Specifies the default section alignment in a PE file.
        /// </summary>
        private const uint FILE_SECTION_ALIGNMENT = 0x1000;

        /// <summary>
        /// Specifies the default section alignment in virtual memory.
        /// </summary>
        private const uint SECTION_ALIGNMENT = FILE_SECTION_ALIGNMENT;

        #endregion // Constants

        #region Data members

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
        private List<LinkerSection> sections;

        /// <summary>
        /// Holds the entire size of the image.
        /// </summary>
        private uint sizeOfImage;

        /// <summary>
        /// Flag, if the symbols have been resolved.
        /// </summary>
        private bool symbolsResolved;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="PortableExecutableLinker"/> class.
        /// </summary>
        public PortableExecutableLinker()
        {
            this.sectionAlignment = SECTION_ALIGNMENT;
            this.fileAlignment = FILE_SECTION_ALIGNMENT;
            this.sections = new List<LinkerSection>();

            // Create the default section set
            LinkerSection[] sections = new LinkerSection[(int)SectionKind.Max];
            sections[(int)SectionKind.Text] = new PortableExecutableLinkerSection(SectionKind.Text, @".text", IntPtr.Zero);
            sections[(int)SectionKind.Data] = new PortableExecutableLinkerSection(SectionKind.Data, @".data", IntPtr.Zero);
            sections[(int)SectionKind.ROData] = new PortableExecutableLinkerSection(SectionKind.ROData, @".rodata", IntPtr.Zero);
            sections[(int)SectionKind.BSS] = new PortableExecutableLinkerSection(SectionKind.BSS, @".bss", IntPtr.Zero);
            this.sections.AddRange(sections);
        }

        #endregion // Construction

        #region AssembyLinkerStageBase Overrides

        /// <summary>
        /// A request to patch already emitted code by storing the calculated address value.
        /// </summary>
        /// <param name="linkType">Type of the link.</param>
        /// <param name="method">The method whose code is being patched.</param>
        /// <param name="methodOffset">The value to store at the position in code.</param>
        /// <param name="methodRelativeBase">The method relative base.</param>
        /// <param name="targetAddress">The position in code, where it should be patched.</param>
        protected override void ApplyPatch(LinkType linkType, RuntimeMethod method, long methodOffset, long methodRelativeBase, long targetAddress)
        {
            if (this.symbolsResolved == false)
                throw new InvalidOperationException(@"Can't apply patches - symbols not resolved.");

            if ((linkType & LinkType.KindMask) == LinkType.AbsoluteAddress)
            {
                // FIXME: Need a .reloc section with a relocation entry if the module is moved in virtual memory
                // the runtime loader must patch this link request, we'll just skip it.
                throw new NotSupportedException(@".reloc section not supported.");
            }
            else
            {
                // Retrieve the text section
                PortableExecutableLinkerSection text = (PortableExecutableLinkerSection)GetSection(SectionKind.Text);
                // Calculate the relative offset
                long value = targetAddress - (method.Address.ToInt64() + methodRelativeBase);
                // Calculate the patch offset
                long offset = (method.Address.ToInt64() - text.Address.ToInt64()) + methodOffset;
                // Save the stream position
                text.ApplyPatch(offset, linkType, value);
            }
        }

        /// <summary>
        /// Retrieves a linker section by its type.
        /// </summary>
        /// <param name="sectionKind">The type of the section to retrieve.</param>
        /// <returns>The retrieved linker section.</returns>
        protected override LinkerSection GetSection(SectionKind sectionKind)
        {
            try
            {
                return this.sections[(int)sectionKind];
            }
            catch
            {
            }

            return null;
        }

        /// <summary>
        /// Determines whether the specified symbol is resolved.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="address">The address.</param>
        /// <returns>
        /// 	<c>true</c> if the specified symbol is resolved; otherwise, <c>false</c>.
        /// </returns>
        protected override bool IsResolved(string symbol, out long address)
        {
            address = 0;
            return (this.symbolsResolved == true && base.IsResolved(symbol, out address) == true);
        }

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public override string Name
        {
            get { return @"Portable Executable File Linker"; }
        }

        /// <summary>
        /// Retrieves the collection of sections created during compilation.
        /// </summary>
        /// <value>The sections collection.</value>
        public override ICollection<LinkerSection> Sections
        {
            get { return this.sections; }
        }

        /// <summary>
        /// Allocates a symbol of the given name in the specified section.
        /// </summary>
        /// <param name="section">The executable section to allocate from.</param>
        /// <param name="size">The number of bytes to allocate. If zero, indicates an unknown amount of memory is required.</param>
        /// <param name="alignment">The alignment. A value of zero indicates the use of a default alignment for the section.</param>
        /// <returns>
        /// A stream, which can be used to populate the section.
        /// </returns>
        protected override Stream Allocate(SectionKind section, int size, int alignment)
        {
            PortableExecutableLinkerSection linkerSection = (PortableExecutableLinkerSection)this.sections[(int)section];
            return linkerSection.Allocate(size, alignment);
        }

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
        public override void Run(AssemblyCompiler compiler)
        {
            if (String.IsNullOrEmpty(this.OutputFile) == true)
                throw new ArgumentException(@"Invalid argument.", @"outputFile");

            // Layout the sections in memory
            LayoutSections();
            
            // Resolve all symbols first
            base.Run(compiler);

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
            using (FileStream fs = new FileStream(this.OutputFile, FileMode.Create, FileAccess.Write, FileShare.None))
            using (BinaryWriter writer = new BinaryWriter(fs, Encoding.Unicode))
            {
                // Write the PE headers
                WriteDosHeader(writer);
                WritePEHeader(writer);

                // Iterate all sections and store their data
                long position = writer.BaseStream.Position;
                foreach (PortableExecutableLinkerSection section in this.sections)
                {
                    // Write the section
                    section.Write(writer);

                    // Add padding...
                    position += section.Length;
                    position += (this.fileAlignment - (position % this.fileAlignment));                    
                    WritePaddingToPosition(writer, position);
                }
            }
        }

        /// <summary>
        /// Adjusts the section addresses and performs a proper layout.
        /// </summary>
        private void LayoutSections()
        {
            // Reset the size of the image
            this.sizeOfImage = this.fileAlignment;

            // Take the base address of the image
            long address = this.BaseAddress + this.sectionAlignment;

            // Move all sections to their right positions
            List<LinkerSection> usedSections = new List<LinkerSection>();
            foreach (LinkerSection ls in this.sections)
            {
                // Only use a section with something inside
                if (ls.Length != 0)
                {
                    // Set the section address
                    ls.Address = new IntPtr(address);
                    ls.Offset = this.sizeOfImage;

                    // Calculate the address of the next section
                    address += ls.Length;
                    this.sizeOfImage += (uint)ls.Length;

                    // Perform the section alignment
                    address += (this.sectionAlignment - (address % this.sectionAlignment));
                    this.sizeOfImage = AlignValue(this.sizeOfImage, this.fileAlignment);

                    // Copy the section
                    usedSections.Add(ls);
                }
            }

            this.sections = usedSections;

            // We've resolved all symbols, allow IsResolved to succeed
            this.symbolsResolved = true;
        }

        /// <summary>
        /// Writes the dos header of the PE file.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void WriteDosHeader(BinaryWriter writer)
        {
            /*
             * This code block generates the default DOS header of a PE image.
             * These constants are not further documented here, please consult
             * MSDN for their meaning.
             */
            IMAGE_DOS_HEADER dosHeader = new IMAGE_DOS_HEADER();
            dosHeader.e_magic = IMAGE_DOS_HEADER.DOS_HEADER_MAGIC;
            dosHeader.e_cblp = 0x0090;
            dosHeader.e_cp = 0x0003;
            dosHeader.e_cparhdr = 0x0004;
            dosHeader.e_maxalloc = 0xFFFF;
            dosHeader.e_sp = 0xb8;
            dosHeader.e_lfarlc = 0x0040;
            dosHeader.e_lfanew = 0x00000080;
            dosHeader.Write(writer);

            // Write the following 64 bytes, which represent the default x86 code to
            // print a message on the screen.
/*
            byte[] message = new byte[] {
                0x0E, 0x1F, 0xBA, 0x0E, 0x00, 0xB4, 0x09, 0xCD, 0x21, 0xB8, 0x01, 0x4C, 0xCD, 0x21, 0x54, 0x68,
                0x69, 0x73, 0x20, 0x70, 0x72, 0x6F, 0x67, 0x72, 0x61, 0x6D, 0x20, 0x72, 0x65, 0x71, 0x75, 0x69,
                0x72, 0x65, 0x73, 0x20, 0x61, 0x20, 0x4D, 0x4F, 0x53, 0x41, 0x20, 0x70, 0x6F, 0x77, 0x65, 0x72,
                0x65, 0x64, 0x20, 0x4F, 0x53, 0x2E, 0x0D, 0x0D, 0x0A, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };
 */
            byte[] message = {
	            0x0E, 0x1F, 0xBA, 0x0E, 0x00, 0xB4, 0x09, 0xCD, 0x21, 0xB8, 0x01, 0x4C, 0xCD, 0x21, 0x54, 0x68,
	            0x69, 0x73, 0x20, 0x70, 0x72, 0x6F, 0x67, 0x72, 0x61, 0x6D, 0x20, 0x63, 0x61, 0x6E, 0x6E, 0x6F,
	            0x74, 0x20, 0x62, 0x65, 0x20, 0x72, 0x75, 0x6E, 0x20, 0x69, 0x6E, 0x20, 0x44, 0x4F, 0x53, 0x20,
	            0x6D, 0x6F, 0x64, 0x65, 0x2E, 0x0D, 0x0D, 0x0A, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };
            writer.Write(message);
        }

        /// <summary>
        /// Writes the PE header.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void WritePEHeader(BinaryWriter writer)
        {
            // Write the PE signature and headers
            IMAGE_NT_HEADERS ntHeaders = new IMAGE_NT_HEADERS();
            ntHeaders.Signature = IMAGE_NT_HEADERS.PE_SIGNATURE;

            // Prepare the file header
            ntHeaders.FileHeader.Machine = IMAGE_FILE_HEADER.IMAGE_FILE_MACHINE_I386;
            ntHeaders.FileHeader.NumberOfSections = (ushort)this.Sections.Count;
            ntHeaders.FileHeader.TimeDateStamp = (uint)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
            ntHeaders.FileHeader.PointerToSymbolTable = 0;
            ntHeaders.FileHeader.NumberOfSymbols = 0;
            ntHeaders.FileHeader.SizeOfOptionalHeader = 0x00E0; // FIXME: Really this way?
            ntHeaders.FileHeader.Characteristics = 0x010E; // FIXME: Use an enum here

            // Prepare the "optional" headers
            ntHeaders.OptionalHeader.Magic = IMAGE_OPTIONAL_HEADER.IMAGE_OPTIONAL_HEADER_MAGIC;
            ntHeaders.OptionalHeader.MajorLinkerVersion = 6;
            ntHeaders.OptionalHeader.MinorLinkerVersion = 0;
            ntHeaders.OptionalHeader.SizeOfCode = AlignValue(GetSectionLength(SectionKind.Text), this.sectionAlignment);
            ntHeaders.OptionalHeader.SizeOfInitializedData = AlignValue(GetSectionLength(SectionKind.Data) + GetSectionLength(SectionKind.ROData), this.sectionAlignment);
            ntHeaders.OptionalHeader.SizeOfUninitializedData = AlignValue(GetSectionLength(SectionKind.BSS), this.sectionAlignment);
            // FIXME: Lookup the entry point symbol address
            ntHeaders.OptionalHeader.AddressOfEntryPoint = (uint)(GetSectionAddress(SectionKind.Text) - this.BaseAddress);
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
            ntHeaders.OptionalHeader.SizeOfImage = this.sizeOfImage;
            ntHeaders.OptionalHeader.SizeOfHeaders = this.fileAlignment; // FIXME: Use the full header size
            ntHeaders.OptionalHeader.CheckSum = 0; // FIXME: We will checksum native code, determine algorithm used by Win32 here
            ntHeaders.OptionalHeader.Subsystem = 0x03;
            ntHeaders.OptionalHeader.DllCharacteristics = 0x0540;
            ntHeaders.OptionalHeader.SizeOfStackReserve = 0x100000;
            ntHeaders.OptionalHeader.SizeOfStackCommit = 0x1000;
            ntHeaders.OptionalHeader.SizeOfHeapReserve = 0x100000;
            ntHeaders.OptionalHeader.SizeOfHeapCommit = 0x1000;
            ntHeaders.OptionalHeader.LoaderFlags = 0;
            ntHeaders.OptionalHeader.NumberOfRvaAndSizes = IMAGE_OPTIONAL_HEADER.IMAGE_NUMBEROF_DIRECTORY_ENTRIES;
            ntHeaders.OptionalHeader.DataDirectory = new IMAGE_DATA_DIRECTORY[IMAGE_OPTIONAL_HEADER.IMAGE_NUMBEROF_DIRECTORY_ENTRIES];

            // Populate the CIL data directory (FIXME)
            ntHeaders.OptionalHeader.DataDirectory[14].VirtualAddress = 0;
            ntHeaders.OptionalHeader.DataDirectory[14].Size = 0;

            ntHeaders.Write(writer);

            // Write the section headers
            uint address = this.fileAlignment;
            foreach (LinkerSection section in this.sections)
            {
                IMAGE_SECTION_HEADER ish = new IMAGE_SECTION_HEADER();
                ish.Name = section.Name;
                ish.VirtualSize = (uint)section.Length;
                ish.VirtualAddress = (uint)(section.Address.ToInt64() - this.BaseAddress);

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

            WritePaddingToPosition(writer, this.fileAlignment);
        }

        private long GetSectionAddress(SectionKind sectionKind)
        {
            LinkerSection section = GetSection(sectionKind);
            if (section != null)
                return (uint)section.Address.ToInt64();

            return 0L;
        }

        private uint GetSectionLength(SectionKind sectionKind)
        {
            LinkerSection section = GetSection(sectionKind);
            if (section != null)
                return (uint)section.Length;

            return 0;
        }

        private uint AlignValue(uint value, uint alignment)
        {
            return (value + (alignment - (value % alignment)));
        }

        /// <summary>
        /// Adds padding to the writer to ensure the next write starts at a specific address.
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

        #endregion // Internals
    }
}
