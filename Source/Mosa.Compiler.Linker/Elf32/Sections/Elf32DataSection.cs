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

namespace Mosa.Compiler.Linker.Elf32.Sections
{
	/// <summary>
	/// 
	/// </summary>
	public class Elf32DataSection : Elf32Section
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Elf32DataSection"/> class.
		/// </summary>
		public Elf32DataSection()
			: base(SectionKind.Data, @".data", IntPtr.Zero)
		{
			_header.Type = Elf32SectionType.ProgBits;
			_header.Flags = Elf32SectionAttribute.Alloc | Elf32SectionAttribute.Write;
		}
	}
}
