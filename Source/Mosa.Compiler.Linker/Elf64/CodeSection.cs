﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

namespace Mosa.Compiler.Linker.Elf64
{
	/// <summary>
	///
	/// </summary>
	public class CodeSection : Elf64LinkerSection
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CodeSection"/> class.
		/// </summary>
		public CodeSection()
			: base(Mosa.Compiler.Linker.SectionKind.Text, @".text", 0)
		{
			header.Type = SectionType.ProgBits;
			header.Flags = SectionAttribute.AllocExecute;
		}
	}
}