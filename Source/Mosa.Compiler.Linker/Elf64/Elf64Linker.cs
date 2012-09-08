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
using Mosa.Compiler.LinkerFormat.Elf64;
using Mosa.Compiler.LinkerFormat.Elf;

namespace Mosa.Compiler.Linker.Elf64
{
	/// <summary>
	/// 
	/// </summary>
	public class Elf64Linker : BaseLinker
	{

		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private NullSection nullSection;
		/// <summary>
		/// 
		/// </summary>
		private StringTableSection stringTableSection;

		#endregion // Data members

		/// <summary>
		/// Initializes a new instance of the <see cref="Elf64Linker"/> class.
		/// </summary>
		public Elf64Linker()
		{
			// Create the default section set
			Sections.Add(new CodeSection());
			Sections.Add(new DataSection());
			Sections.Add(new RoDataSection());
			Sections.Add(new BssSection());

			nullSection = new NullSection();
			stringTableSection = new StringTableSection();
		}

		/// <summary>
		/// Creates the final linked file.
		/// </summary>
		protected override void CreateFile()
		{
			using (FileStream fs = new FileStream(this.OutputFile, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				Header header = new Header();
				header.Type = FileType.Executable;
				header.Machine = (MachineType)MachineID;
				header.SectionHeaderNumber = (ushort)(Sections.Count + 2);
				header.SectionHeaderOffset = header.ElfHeaderSize;

				header.CreateIdent(IdentClass.Class64, IdentData.Data2LSB, null);

				// Calculate the concatenated size of all section's data
				uint offset = 0;
				foreach (Elf64LinkerSection section in Sections)
				{
					offset += (uint)section.Length;
				}
				offset += (uint)nullSection.Length;
				offset += (uint)stringTableSection.Length;

				// Calculate offsets
				header.ProgramHeaderOffset = (uint)header.ElfHeaderSize + (uint)header.SectionHeaderEntrySize * (uint)header.SectionHeaderNumber + offset;
				header.SectionHeaderStringIndex = (ushort)((ushort)header.ProgramHeaderOffset + (ushort)header.ProgramHeaderNumber * (ushort)header.ProgramHeaderEntrySize);

				EndianAwareBinaryWriter writer = new EndianAwareBinaryWriter(fs, IsLittleEndian);

				// Write the ELF Header
				header.Write(writer);

				// Overjump the Section Header Table and write the section's data first
				long tmp = fs.Position;
				writer.Seek((int)(tmp + header.SectionHeaderNumber * header.SectionHeaderEntrySize), SeekOrigin.Begin);

				nullSection.Write(writer);
				stringTableSection.Write(writer);

				// Write the sections
				foreach (Elf64LinkerSection section in Sections)
					section.Write(writer);

				// Jump back to the Section Header Table
				writer.Seek((int)tmp, SeekOrigin.Begin);

				nullSection.WriteHeader(writer);
				stringTableSection.WriteHeader(writer);

				// Write the section headers
				foreach (Elf64LinkerSection section in Sections)
					section.WriteHeader(writer);
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
