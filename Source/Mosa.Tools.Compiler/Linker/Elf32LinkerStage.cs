/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using System.Collections.Generic;

using Mosa.Compiler.Linker;
using Mosa.Compiler.Linker.Elf32;
using Mosa.Compiler.Linker.Elf32.Sections;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Tools.Compiler.Linker
{
	/// <summary>
	/// 
	/// </summary>
	public class Elf32LinkerStage : BaseAssemblyLinkerStage, IPipelineStage
	{

		#region IPipelineStage

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"Executable and Linking Format (ELF) Linker [32 Bit]"; } }

		#endregion // IPipelineStage Members

		/// <summary>
		/// 
		/// </summary>
		private const uint FileSectionAlignment = 0x200;

		/// <summary>
		/// Specifies the default section alignment in virtual memory.
		/// </summary>
		private const uint SectionAlignment = 0x1000;
		/// <summary>
		/// 
		/// </summary>
		private readonly List<LinkerSection> _sections;
		/// <summary>
		/// 
		/// </summary>
		private readonly Elf32NullSection _nullSection;
		/// <summary>
		/// 
		/// </summary>
		private readonly Elf32StringTableSection _stringTableSection;
		/// <summary>
		/// Holds the file alignment used for this ELF32 file.
		/// </summary>
		private uint _fileAlignment;
		/// <summary>
		/// Holds the section alignment used for this ELF32 file.
		/// </summary>
		private readonly uint _sectionAlignment;
		/// <summary>
		/// Flag, if the symbols have been resolved.
		/// </summary>
		private bool _symbolsResolved;

		/// <summary>
		/// Retrieves the collection of _sections created during compilation.
		/// </summary>
		/// <value>The _sections collection.</value>
		public override ICollection<LinkerSection> Sections
		{
			get
			{
				return _sections;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Elf32LinkerStage"/> class.
		/// </summary>
		public Elf32LinkerStage()
		{
			_sections = new List<LinkerSection>();
			_fileAlignment = FileSectionAlignment;
			_sectionAlignment = SectionAlignment;

			// Create the default section set
			Elf32Section[] sections = new Elf32Section[(int)SectionKind.Max];
			sections[(int)SectionKind.Text] = new Elf32CodeSection();
			sections[(int)SectionKind.Data] = new Elf32DataSection();
			sections[(int)SectionKind.ROData] = new Elf32RoDataSection();
			sections[(int)SectionKind.BSS] = new Elf32BssSection();
			_sections.AddRange(sections);

			_nullSection = new Elf32NullSection();
			_stringTableSection = new Elf32StringTableSection();
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public override void Run()
		{
			if (String.IsNullOrEmpty(OutputFile))
				throw new ArgumentException(@"Invalid argument.", "compiler");

			// Layout the _sections in memory
			LayoutSections();

			// Resolve all symbols first
			base.Run();

			// Persist the Elf32 file now
			CreateElf32File(this.Compiler);
		}

		/// <summary>
		/// Gets the load alignment of _sections.
		/// </summary>
		/// <value>The load alignment.</value>
		public override long LoadSectionAlignment
		{
			get { return _fileAlignment; }
		}

		/// <summary>
		/// Gets the virtual alignment of _sections.
		/// </summary>
		/// <value>The virtual section alignment.</value>
		public override long VirtualSectionAlignment
		{
			get { return _sectionAlignment; }
		}

		/// <summary>
		/// Gets or sets the file alignment in bytes.
		/// </summary>
		/// <value>The file alignment in bytes.</value>
		public uint FileAlignment
		{
			get { return _fileAlignment; }
			set
			{
				if (value < FileSectionAlignment)
					throw new ArgumentException(@"Section alignment must not be less than 512 bytes.", @"value");
				if ((value & unchecked(FileSectionAlignment - 1)) != 0)
					throw new ArgumentException(@"Section alignment must be a multiple of 512 bytes.", @"value");

				_fileAlignment = value;
			}
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
		protected override System.IO.Stream Allocate(SectionKind section, int size, int alignment)
		{
			Elf32Section linkerSection = (Elf32Section)GetSection(section);
			return linkerSection.Allocate(size, alignment);
		}

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
			if (_symbolsResolved == false)
				throw new InvalidOperationException(@"Can't apply patches - symbols not resolved.");

			// Retrieve the text section
			Elf32Section text = (Elf32Section)GetSection(SectionKind.Text);
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
			return _sections[(int)sectionKind];
		}

		/// <summary>
		/// Determines whether the specified symbol is resolved.
		/// </summary>
		/// <param name="symbol">The symbol.</param>
		/// <param name="virtualAddress">The address.</param>
		/// <returns>
		/// 	<c>true</c> if the specified symbol is resolved; otherwise, <c>false</c>.
		/// </returns>
		protected override bool IsResolved(string symbol, out long virtualAddress)
		{
			virtualAddress = 0;
			return (_symbolsResolved && base.IsResolved(symbol, out virtualAddress));
		}

		/// <summary>
		/// Creates the elf32 file.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		private void CreateElf32File(AssemblyCompiler compiler)
		{
			using (System.IO.FileStream fs = new System.IO.FileStream(OutputFile, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
			{
				Elf32Header header = new Elf32Header();
				header.Type = Elf32FileType.Executable;
				header.Machine = Elf32MachineType.Intel386;
				header.SectionHeaderNumber = (ushort)(Sections.Count + 2);
				header.SectionHeaderOffset = header.ElfHeaderSize;

				header.CreateIdent(Elf32IdentClass.Class32, Elf32IdentData.Data2LSB, null);

				// Calculate the concatenated size of all section's data
				uint offset = 0;
				foreach (Elf32Section section in Sections)
				{
					offset += (uint)section.Length;
				}
				offset += (uint)_nullSection.Length;
				offset += (uint)_stringTableSection.Length;

				// Calculate offsets
				header.ProgramHeaderOffset = header.ElfHeaderSize + header.SectionHeaderEntrySize * (uint)header.SectionHeaderNumber + offset;
				header.ProgramHeaderNumber = 1;
				header.SectionHeaderStringIndex = 1;

				System.IO.BinaryWriter writer = new System.IO.BinaryWriter(fs);

				// Write the ELF Header
				header.Write(writer);

				// Overjump the Section Header Table and write the section's data first
				long tmp = fs.Position;
				writer.Seek((int)(tmp + header.SectionHeaderNumber * header.SectionHeaderEntrySize), System.IO.SeekOrigin.Begin);

				_nullSection.Write(writer);
				_stringTableSection.Write(writer);

				// Write the _sections
				foreach (Elf32Section section in Sections)
					section.Write(writer);

				// Jump back to the Section Header Table
				writer.Seek((int)tmp, System.IO.SeekOrigin.Begin);

				_nullSection.WriteHeader(writer);
				_stringTableSection.WriteHeader(writer);

				// Write the section headers
				foreach (Elf32Section section in Sections)
					section.WriteHeader(writer);

				Elf32ProgramHeader pheader = new Elf32ProgramHeader
												 {
													 Alignment = 0,
													 FileSize = (uint)GetSection(SectionKind.Text).Length
												 };
				pheader.MemorySize = pheader.FileSize;
				pheader.VirtualAddress = 0xFF0000;
				pheader.Flags = Elf32ProgramHeaderFlags.Execute | Elf32ProgramHeaderFlags.Read | Elf32ProgramHeaderFlags.Write;
				pheader.Offset = ((Elf32Section)GetSection(SectionKind.Text)).Header.Offset;
				pheader.Type = Elf32ProgramHeaderType.Load;

				writer.Seek((int)header.ProgramHeaderOffset, System.IO.SeekOrigin.Begin);
				pheader.Write(writer);
			}
		}

		/// <summary>
		/// Adjusts the section addresses and performs a proper layout.
		/// </summary>
		private void LayoutSections()
		{
			// We've resolved all symbols, allow IsResolved to succeed
			_symbolsResolved = true;
		}
	}
}
