/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Linker;
using System.IO;

namespace Mosa.Tool.Explorer
{
	internal class ExplorerLinker : BaseLinker
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ExplorerLinker"/> class.
		/// </summary>
		public ExplorerLinker()
		{
			SectionAlignment = 1;

			AddSection(new LinkerSection(SectionKind.Text, ".text", SectionAlignment));
			AddSection(new LinkerSection(SectionKind.Data, ".data", SectionAlignment));
			AddSection(new LinkerSection(SectionKind.ROData, ".rodata", SectionAlignment));
			AddSection(new LinkerSection(SectionKind.BSS, ".bss", SectionAlignment));
		}

		protected override void EmitImplementation(Stream stream)
		{
		}

		#endregion Construction
	}
}