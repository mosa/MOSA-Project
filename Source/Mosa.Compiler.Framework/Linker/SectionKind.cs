// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Linker
{
	/// <summary>
	/// An enumeration identifying common linker sections.
	/// </summary>
	public enum SectionKind
	{
		Unknown = -1,

		/// <summary>
		/// Identifies the program text section.
		/// </summary>
		Text = 0,

		/// <summary>
		/// Identifies the read/write data section.
		/// </summary>
		Data = 1,

		/// <summary>
		/// Identifies the read-only data section.
		/// </summary>
		ROData = 2,

		/// <summary>
		/// Identifies the bss section.
		/// </summary>
		/// <remarks>
		/// The .bss section is a chunk of memory initialized to zero by the loader.
		/// </remarks>
		BSS = 3,
	}
}
