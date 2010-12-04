/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Linker.Elf64.Sections
{
	/// <summary>
	/// 
	/// </summary>
	public class Elf64BssSection : Elf64Section
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Elf64BssSection"/> class.
		/// </summary>
		public Elf64BssSection()
			: base(Mosa.Runtime.Linker.SectionKind.BSS, @".bss", IntPtr.Zero)
		{
			header.Type = Elf64SectionType.NoBits;
			header.Flags = Elf64SectionAttribute.Alloc | Elf64SectionAttribute.Write;
		}
	}
}
