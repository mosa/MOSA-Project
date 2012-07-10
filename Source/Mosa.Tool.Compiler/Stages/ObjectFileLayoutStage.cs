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

namespace Mosa.Tool.Compiler.Stages
{
	/// <summary>
	/// Lays out sections and symbols sequentially in an object file.
	/// </summary>
	public class ObjectFileLayoutStage : BaseCompilerStage, ICompilerStage, IPipelineStage
	{
		private ILinker linker;

		#region Methods

		/// <summary>
		/// Lays out the sections in order of appearance.
		/// </summary>
		protected virtual void LayoutSections()
		{
			long fileAlignment = this.linker.LoadSectionAlignment;
			long sectionAlignment = this.linker.VirtualSectionAlignment;

			// Reset the size of the image
			long virtualSizeOfImage = sectionAlignment;
			long fileSizeOfImage = fileAlignment;

			// Move all sections to their right positions
			foreach (LinkerSection ls in this.linker.Sections)
			{
				// Only use a section with something inside
				if (ls.Length > 0)
				{
					// Set the section virtualAddress
					ls.VirtualAddress = new IntPtr(this.linker.BaseAddress + virtualSizeOfImage);
					ls.Offset = fileSizeOfImage;

					// Update the file size
					fileSizeOfImage += ls.Length;
					fileSizeOfImage = AlignValue(fileSizeOfImage, fileAlignment);

					// Update the virtual size
					virtualSizeOfImage += ls.Length;
					virtualSizeOfImage = AlignValue(virtualSizeOfImage, sectionAlignment);
				}
			}
		}

		/// <summary>
		/// Lays the symbols out according to their offset in the section.
		/// </summary>
		protected virtual void LayoutSymbols()
		{
			// Adjust the symbol addresses
			foreach (LinkerSymbol symbol in this.linker.Symbols)
			{
				LinkerSection ls = linker.GetSection(symbol.Section);
				symbol.Offset = ls.Offset + symbol.SectionAddress;
				symbol.VirtualAddress = new IntPtr(ls.VirtualAddress.ToInt64() + symbol.SectionAddress);
			}
		}

		/// <summary>
		/// Aligns the value to the requested alignment.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="alignment">The alignment.</param>
		/// <returns>The aligned value.</returns>
		protected long AlignValue(long value, long alignment)
		{
			return (value + (alignment - (value % alignment)));
		}

		#endregion // Methods

		#region ICompilerStage Overrides

		void ICompilerStage.Setup(BaseCompiler compiler)
		{
			base.Setup(compiler);
			linker = RetrieveLinkerFromCompiler();
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
	}
}
