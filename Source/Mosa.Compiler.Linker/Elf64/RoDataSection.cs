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
	public class RoDataSection : Elf64LinkerSection
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RoDataSection"/> class.
		/// </summary>
		public RoDataSection()
			: base(Mosa.Compiler.Linker.SectionKind.ROData, @".rodata", 0)
		{
			header.Type = SectionType.ProgBits;
			header.Flags = SectionAttribute.Alloc;
		}
	}
}
