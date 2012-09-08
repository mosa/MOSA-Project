/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Bruce Markham (illuminus) <illuminus86@gmail.com>
 */

using System;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Linker;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Lays out sections and symbols sequentially in an object file.
	/// </summary>
	public sealed class ObjectFileLayoutStage : BaseCompilerStage, ICompilerStage, IPipelineStage
	{

		#region ICompilerStage Overrides

		void ICompilerStage.Setup(BaseCompiler compiler)
		{
			base.Setup(compiler);
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void ICompilerStage.Run()
		{
			LayoutSections();
			LayoutSymbols();
		}

		#endregion // ICompilerStage Overrides

		#region Methods

		/// <summary>
		/// Lays out the sections in order of appearance.
		/// </summary>
		private void LayoutSections()
		{
			long fileAlignment = linker.LoadSectionAlignment;
			long sectionAlignment = linker.SectionAlignment;

			// Reset the size of the image
			long virtualSizeOfImage = sectionAlignment;	// FIXME: Why not 0? Zero aligns to everything
			long fileSizeOfImage = fileAlignment;		// FIXME: Why not 0? Zero aligns to everything

			// Move all sections to their right positions
			foreach (LinkerSection section in linker.Sections)
			{
				// Only use a section with something inside
				if (section.Length == 0)
					continue;

				// Set the section virtualAddress
				section.VirtualAddress = linker.BaseAddress + virtualSizeOfImage;
				section.Offset = fileSizeOfImage;

				// Update the file size
				fileSizeOfImage += section.Length;
				fileSizeOfImage = AlignValue(fileSizeOfImage, fileAlignment);

				// Update the virtual size
				virtualSizeOfImage += section.Length;
				virtualSizeOfImage = AlignValue(virtualSizeOfImage, sectionAlignment);
			}
		}

		/// <summary>
		/// Lays the symbols out according to their offset in the section.
		/// </summary>
		private void LayoutSymbols()
		{
			// Adjust the symbol addresses
			foreach (LinkerSymbol symbol in linker.Symbols)
			{
				LinkerSection section = linker.GetSection(symbol.Section);
				symbol.Offset = section.Offset + symbol.SectionAddress;
				symbol.VirtualAddress = section.VirtualAddress + symbol.SectionAddress;
			}
		}

		/// <summary>
		/// Aligns the value to the requested alignment.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="alignment">The alignment.</param>
		/// <returns>The aligned value.</returns>
		private long AlignValue(long value, long alignment)
		{
			return (value + (alignment - (value % alignment)));
		}

		#endregion // Methods

	}
}
