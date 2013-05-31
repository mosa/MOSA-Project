/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 */

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
			AddSection(new TestLinkerSection(SectionKind.BSS, "BSS"));
			AddSection(new TestLinkerSection(SectionKind.Data, "Data"));
			AddSection(new TestLinkerSection(SectionKind.ROData, "ReadOnlyData"));
			AddSection(new TestLinkerSection(SectionKind.Text, "Text"));

			LoadSectionAlignment = 1;
			SectionAlignment = 1;
			Endianness = Endianness.Little;	// x86
		}

		#endregion Construction
	}
}