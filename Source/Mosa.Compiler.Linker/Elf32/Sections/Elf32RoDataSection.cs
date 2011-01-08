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
	public class Elf32RoDataSection : Elf32Section
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Elf32RoDataSection"/> class.
		/// </summary>
		public Elf32RoDataSection()
			: base(SectionKind.ROData, @".rodata", IntPtr.Zero)
		{
			_header.Type = Elf32SectionType.ProgBits;
			_header.Flags = Elf32SectionAttribute.Alloc;
		}
	}
}
