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
	public class Elf64DataSection : Elf64Section
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Elf64DataSection"/> class.
		/// </summary>
		public Elf64DataSection()
			: base(Mosa.Runtime.Linker.SectionKind.Data, @".data", IntPtr.Zero)
		{
			header.Type = Elf64SectionType.ProgBits;
			header.Flags = Elf64SectionAttribute.Alloc | Elf64SectionAttribute.Write;
		}
	}
}
