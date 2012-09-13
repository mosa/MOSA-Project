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
using System.IO;
using Mosa.Compiler.Common;
using Mosa.Compiler.LinkerFormat.Elf32;
using Mosa.Compiler.LinkerFormat.Elf;

namespace Mosa.Compiler.Linker.Elf32
{
	/// <summary>
	/// 
	/// </summary>
	public class Elf32Linker : BaseLinker
	{

		#region Constants

		/// <summary>
		/// 
		/// </summary>
		private const uint FILE_SECTION_ALIGNMENT = 0x200;

		/// <summary>
		/// Specifies the default section alignment in virtual memory.
		/// </summary>
		private const uint SECTION_ALIGNMENT = 0x1000;

		#endregion // Constants

		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private readonly NullSection nullSection;
		/// <summary>
		/// 
		/// </summary>
		private readonly StringTableSection stringTableSection;

		#endregion // Data members

		/// <summary>
		/// Initializes a new instance of the <see cref="Elf32Linker"/> class.
		/// </summary>
		public Elf32Linker()
		{
			// Create the default section set
			Sections.Add(new CodeSection());
			Sections.Add(new DataSection());
			Sections.Add(new RoDataSection());
			Sections.Add(new BssSection());

			LoadSectionAlignment = FILE_SECTION_ALIGNMENT;
			SectionAlignment = SECTION_ALIGNMENT;

			nullSection = new NullSection();
			stringTableSection = new StringTableSection();
		}

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
		}

		/// <summary>
		/// Creates the final linked file.
		/// </summary>
		protected override void CreateFile()
		{
			using (FileStream fs = new FileStream(OutputFile, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				Header header = new Header();
				header.Type = FileType.Executable;
				header.Machine = (MachineType)MachineID;
				header.SectionHeaderNumber = (ushort)(Sections.Count + 2);
				header.SectionHeaderOffset = header.ElfHeaderSize;

				header.CreateIdent(IdentClass.Class32, Endianness == Endianness.Little ? IdentData.Data2LSB : IdentData.Data2MSB, null);

				// Calculate the concatenated size of all section's data
				uint offset = 0;
				foreach (Elf32LinkerSection section in Sections)
				{
					offset += (uint)section.Length;
				}
				offset += (uint)nullSection.Length;
				offset += (uint)stringTableSection.Length;

				// Calculate offsets
				header.ProgramHeaderOffset = header.ElfHeaderSize + header.SectionHeaderEntrySize * (uint)header.SectionHeaderNumber + offset;
				header.ProgramHeaderNumber = 1;
				header.SectionHeaderStringIndex = 1;

				EndianAwareBinaryWriter writer = new EndianAwareBinaryWriter(fs, Endianness);

				// Write the ELF Header
				header.Write(writer);

				// Overjump the Section Header Table and write the section's data first
				long tmp = fs.Position;
				writer.Seek((int)(tmp + header.SectionHeaderNumber * header.SectionHeaderEntrySize), SeekOrigin.Begin);

				nullSection.Write(writer);
				stringTableSection.Write(writer);

				// Write the _sections
				foreach (Elf32LinkerSection section in Sections)
					section.Write(writer);

				// Jump back to the Section Header Table
				writer.Seek((int)tmp, SeekOrigin.Begin);

				nullSection.WriteHeader(writer);
				stringTableSection.WriteHeader(writer);

				// Write the section headers
				foreach (Elf32LinkerSection section in Sections)
					section.WriteHeader(writer);

				ProgramHeader pheader = new ProgramHeader
				{
					Alignment = 0,
					FileSize = (uint)GetSection(SectionKind.Text).Length
				};

				pheader.MemorySize = pheader.FileSize;
				pheader.VirtualAddress = 0xFF0000;
				pheader.Flags = ProgramHeaderFlags.Execute | ProgramHeaderFlags.Read | ProgramHeaderFlags.Write;
				pheader.Offset = ((Elf32LinkerSection)GetSection(SectionKind.Text)).Header.Offset;
				pheader.Type = ProgramHeaderType.Load;

				writer.Seek((int)header.ProgramHeaderOffset, SeekOrigin.Begin);
				pheader.Write(writer);
			}
		}

		/// <summary>
		/// Adjusts the section addresses and performs a proper layout.
		/// </summary>
		protected override void LayoutSections()
		{
			// We've resolved all symbols, allow IsResolved to succeed
			SymbolsResolved = true;
		}
	}
}
