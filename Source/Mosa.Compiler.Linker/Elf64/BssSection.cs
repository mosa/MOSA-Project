/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;

namespace Mosa.Compiler.Linker.Elf64
{
	/// <summary>
	/// 
	/// </summary>
	public class BssSection : Elf64LinkerSection
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BssSection"/> class.
		/// </summary>
		public BssSection()
			: base(Mosa.Compiler.Linker.SectionKind.BSS, @".bss", 0)
		{
			header.Type = SectionType.NoBits;
			header.Flags = SectionAttribute.Alloc | SectionAttribute.Write;
		}
	}
}
