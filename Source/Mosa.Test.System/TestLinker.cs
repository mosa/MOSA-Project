/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 */

using System;
using Mosa.Compiler.Common;
using Mosa.Compiler.Linker;

namespace Mosa.Test.System
{
	/// <summary>
	/// A specialized linker for in-memory tests. This linker performs live linking in memory without
	/// respect to an executable format.
	/// </summary>
	public class TestLinker : BaseLinker
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="TestLinker"/> class.
		/// </summary>
		public TestLinker()
		{
			Sections.Add(new TestLinkerSection(SectionKind.BSS, String.Empty));
			Sections.Add(new TestLinkerSection(SectionKind.Data, String.Empty));
			Sections.Add(new TestLinkerSection(SectionKind.ROData, String.Empty));
			Sections.Add(new TestLinkerSection(SectionKind.Text, String.Empty));

			LoadSectionAlignment = 1;
			SectionAlignment = 1;
			Endianness = Endianness.Little;	// x86
		}

		/// <summary>
		/// Adjusts the section addresses and performs a proper layout.
		/// </summary>
		protected override void LayoutSections()
		{
			foreach (LinkerSymbol symbol in Symbols)
			{
				LinkerSection ls = GetSection(symbol.Section);
				symbol.Offset = ls.Offset + symbol.SectionAddress;
				symbol.VirtualAddress = ls.VirtualAddress + symbol.SectionAddress;
			}

			// We've resolved all symbols, allow IsResolved to succeed
			SymbolsResolved = true;
		}

		#endregion Construction
	}
}