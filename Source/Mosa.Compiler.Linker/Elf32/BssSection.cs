/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;

namespace Mosa.Compiler.Linker.Elf32
{
	/// <summary>
	/// 
	/// </summary>
	public class BssSection : Section
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BssSection"/> class.
		/// </summary>
		public BssSection()
			: base(SectionKind.BSS, @".bss", 0)
		{
			header.Type = SectionType.NoBits;
			header.Flags = SectionAttribute.Alloc | SectionAttribute.Write;
		}
	}
}
