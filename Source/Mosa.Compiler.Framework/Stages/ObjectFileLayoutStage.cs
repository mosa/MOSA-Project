/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Bruce Markham (illuminus) <illuminus86@gmail.com>
 */

using Mosa.Compiler.Linker;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Lays out sections and symbols sequentially in an object file.
	/// </summary>
	public sealed class ObjectFileLayoutStage : BaseCompilerStage
	{
		protected override void Run()
		{
			LayoutSections();
		}

		/// <summary>
		/// Lays out the sections in order of appearance.
		/// </summary>
		private void LayoutSections()
		{
			long fileAlignment = Linker.LoadSectionAlignment;
			long sectionAlignment = Linker.SectionAlignment;

			// Reset the size of the image
			long virtualSizeOfImage = sectionAlignment;	// FIXME: Why not 0? Zero aligns to everything
			long fileSizeOfImage = fileAlignment;		// FIXME: Why not 0? Zero aligns to everything

			// Move all sections to their right positions
			foreach (LinkerSection section in Linker.Sections)
			{
				// Only use a section with something inside
				if (section.Length == 0)
					continue;

				// Set the section virtualAddress
				section.VirtualAddress = Linker.BaseAddress + virtualSizeOfImage;
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
		/// Aligns the value to the requested alignment.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="alignment">The alignment.</param>
		/// <returns>The aligned value.</returns>
		private long AlignValue(long value, long alignment)
		{
			return (value + (alignment - (value % alignment)));
		}
	}
}