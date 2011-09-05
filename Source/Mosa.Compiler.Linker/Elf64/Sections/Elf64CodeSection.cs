/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;

namespace Mosa.Compiler.Linker.Elf64.Sections
{
	/// <summary>
	/// 
	/// </summary>
	public class Elf64CodeSection : Elf64Section
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Elf64CodeSection"/> class.
		/// </summary>
		public Elf64CodeSection()
			: base(Mosa.Compiler.Linker.SectionKind.Text, @".text", IntPtr.Zero)
		{
			header.Type = Elf64SectionType.ProgBits;
			header.Flags = Elf64SectionAttribute.AllocExecute;
		}
	}
}
