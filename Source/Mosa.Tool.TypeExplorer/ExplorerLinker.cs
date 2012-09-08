/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;
using System.IO;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Linker;

namespace Mosa.Tool.TypeExplorer
{
	class ExplorerLinker : BaseLinker
	{

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ExplorerLinker"/> class.
		/// </summary>
		public ExplorerLinker()
		{
			LoadSectionAlignment = 1;
			SectionAlignment = 1;

			Sections.Add(new ExplorerLinkerSection(SectionKind.Text, @".text", this.BaseAddress + SectionAlignment));
			Sections.Add(new ExplorerLinkerSection(SectionKind.Data, @".data", 0));
			Sections.Add(new ExplorerLinkerSection(SectionKind.ROData, @".rodata", 0));
			Sections.Add(new ExplorerLinkerSection(SectionKind.BSS, @".bss", 0));
		}

		#endregion // Construction

		/// <summary>
		/// A request to patch already emitted code by storing the calculated virtualAddress value.
		/// </summary>
		/// <param name="linkType">Type of the link.</param>
		/// <param name="methodAddress">The method address.</param>
		/// <param name="methodOffset">The value to store at the position in code.</param>
		/// <param name="methodRelativeBase">The method relative base.</param>
		/// <param name="targetAddress">The position in code, where it should be patched.</param>
		protected override void ApplyPatch(LinkType linkType, long methodAddress, long methodOffset, long methodRelativeBase, long targetAddress)
		{
			return;
		}

	}

}
