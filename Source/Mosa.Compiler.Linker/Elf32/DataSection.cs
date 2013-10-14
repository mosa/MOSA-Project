/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

namespace Mosa.Compiler.Linker.Elf32
{
	/// <summary>
	///
	/// </summary>
	public class DataSection : Elf32LinkerSection
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DataSection"/> class.
		/// </summary>
		public DataSection()
			: base(SectionKind.Data, @".data", 0)
		{
			header.Type = SectionType.ProgBits;
			header.Flags = SectionAttribute.Alloc | SectionAttribute.Write;
		}
	}
}