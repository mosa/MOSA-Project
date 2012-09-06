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
		}

		#endregion // Construction

		/// <summary>
		/// Allocates a symbol of the given name in the specified section.
		/// </summary>
		/// <param name="section">The executable section to allocate from.</param>
		/// <param name="size">The number of bytes to allocate. If zero, indicates an unknown amount of memory is required.</param>
		/// <param name="alignment">The alignment. A value of zero indicates the use of a default alignment for the section.</param>
		/// <returns>
		/// A stream, which can be used to populate the section.
		/// </returns>
		protected override Stream Allocate(SectionKind section, int size, int alignment)
		{
			return new MemoryStream();
		}

		public override Stream Allocate(string name, SectionKind section, int size, int alignment)
		{
			return new MemoryStream();
		}

		protected override void ApplyPatch(LinkType linkType, long methodAddress, long methodOffset, long methodRelativeBase, long targetAddress)
		{
			return;
		}

	}

}
