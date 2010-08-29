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
using System.Text;

using Mosa.Runtime.Linker;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Runtime.Linker.Elf64
{
	/// <summary>
	/// 
	/// </summary>
	public class Elf64Linker : AssemblyLinkerStageBase, IPipelineStage
	{

		#region IPipelineStage

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"Executable and Linking Format (ELF) Linker"; } }

		#endregion // IPipelineStage Members

		/// <summary>
		/// 
		/// </summary>
		private List<Mosa.Runtime.Linker.LinkerSection> sections;
		/// <summary>
		/// 
		/// </summary>
		private Elf64.Sections.Elf64NullSection nullSection;
		/// <summary>
		/// 
		/// </summary>
		private Elf64.Sections.Elf64StringTableSection stringTableSection;

		/// <summary>
		/// Retrieves the collection of sections created during compilation.
		/// </summary>
		/// <value>The sections collection.</value>
		public override ICollection<Mosa.Runtime.Linker.LinkerSection> Sections
		{
			get
			{
				return this.sections;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Elf64Linker"/> class.
		/// </summary>
		public Elf64Linker()
		{
			this.sections = new List<LinkerSection>();

			// Create the default section set
			Sections.Elf64Section[] sections = new Sections.Elf64Section[(int)SectionKind.Max];
			sections[(int)SectionKind.Text] = new Sections.Elf64CodeSection();
			sections[(int)SectionKind.Data] = new Sections.Elf64DataSection();
			sections[(int)SectionKind.ROData] = new Sections.Elf64RoDataSection();
			sections[(int)SectionKind.BSS] = new Sections.Elf64BssSection();
			this.sections.AddRange(sections);

			nullSection = new Mosa.Runtime.Linker.Elf64.Sections.Elf64NullSection();
			stringTableSection = new Mosa.Runtime.Linker.Elf64.Sections.Elf64StringTableSection();
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public override void Run()
		{
			// Resolve all symbols first
			base.Run();

			// Persist the Elf32 file now
			CreateElf64File(this.Compiler);
		}

		/// <summary>
		/// Gets the load alignment of sections.
		/// </summary>
		/// <value>The load alignment.</value>
		public override long LoadSectionAlignment
		{
			// TODO
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets the virtual alignment of sections.
		/// </summary>
		/// <value>The virtual section alignment.</value>
		public override long VirtualSectionAlignment
		{
			// TODO
			get { throw new NotImplementedException(); }
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
			Elf64.Sections.Elf64Section linkerSection = (Elf64.Sections.Elf64Section)GetSection(section);
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
			// Retrieve the text section
			Elf64.Sections.Elf64Section text = (Elf64.Sections.Elf64Section)GetSection(SectionKind.Text);
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
			return this.sections[(int)sectionKind];
		}

		/// <summary>
		/// Determines whether the specified symbol is resolved.
		/// </summary>
		/// <param name="symbol">The symbol.</param>
		/// <param name="address">The virtualAddress.</param>
		/// <returns>
		/// 	<c>true</c> if the specified symbol is resolved; otherwise, <c>false</c>.
		/// </returns>
		protected override bool IsResolved(string symbol, out long address)
		{
			address = 0;
			return true;
			//return base.IsResolved(symbol, out virtualAddress);*/
		}

		/// <summary>
		/// Creates the elf32 file.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		private void CreateElf64File(Mosa.Runtime.CompilerFramework.AssemblyCompiler compiler)
		{
			using (System.IO.FileStream fs = new System.IO.FileStream(this.OutputFile, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
			{
				Elf64Header header = new Elf64Header();
				header.Type = Elf64FileType.Executable;
				header.Machine = Elf64MachineType.Intel386;
				header.SectionHeaderNumber = (ushort)(Sections.Count + 2);
				header.SectionHeaderOffset = header.ElfHeaderSize;

				header.CreateIdent(Elf64IdentClass.Class64, Elf64IdentData.Data2LSB, null);

				// Calculate the concatenated size of all section's data
				uint offset = 0;
				foreach (Mosa.Runtime.Linker.Elf64.Sections.Elf64Section section in Sections)
				{
					offset += (uint)section.Length;
				}
				offset += (uint)nullSection.Length;
				offset += (uint)stringTableSection.Length;

				// Calculate offsets
				header.ProgramHeaderOffset = (uint)header.ElfHeaderSize + (uint)header.SectionHeaderEntrySize * (uint)header.SectionHeaderNumber + offset;
				header.SectionHeaderStringIndex = (ushort)((ushort)header.ProgramHeaderOffset + (ushort)header.ProgramHeaderNumber * (ushort)header.ProgramHeaderEntrySize);

				System.IO.BinaryWriter writer = new System.IO.BinaryWriter(fs);

				// Write the ELF Header
				header.Write(writer);

				// Overjump the Section Header Table and write the section's data first
				long tmp = fs.Position;
				writer.Seek((int)(tmp + header.SectionHeaderNumber * header.SectionHeaderEntrySize), System.IO.SeekOrigin.Begin);

				nullSection.Write(writer);
				stringTableSection.Write(writer);

				// Write the sections
				foreach (Mosa.Runtime.Linker.Elf64.Sections.Elf64Section section in Sections)
					section.Write(writer);

				// Jump back to the Section Header Table
				writer.Seek((int)tmp, System.IO.SeekOrigin.Begin);

				nullSection.WriteHeader(writer);
				stringTableSection.WriteHeader(writer);

				// Write the section headers
				foreach (Mosa.Runtime.Linker.Elf64.Sections.Elf64Section section in Sections)
					section.WriteHeader(writer);
			}
		}
	}
}
