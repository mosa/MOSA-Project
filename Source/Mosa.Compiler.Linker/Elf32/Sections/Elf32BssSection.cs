/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;

namespace Mosa.Compiler.Linker.Elf32.Sections
{
	/// <summary>
	/// 
	/// </summary>
	public class Elf32BssSection : Elf32Section
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Elf32BssSection"/> class.
		/// </summary>
		public Elf32BssSection()
			: base(SectionKind.BSS, @".bss", IntPtr.Zero)
		{
			_header.Type = Elf32SectionType.NoBits;
			_header.Flags = Elf32SectionAttribute.Alloc | Elf32SectionAttribute.Write;
		}
	}
}
