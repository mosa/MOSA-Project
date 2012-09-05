/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using System.IO;

namespace Mosa.Compiler.Linker.Elf64
{
	/// <summary>
	/// 
	/// </summary>
	public class NullSection : Section
	{
		/// <summary>
		/// Gets the length of the section in bytes.
		/// </summary>
		/// <value>The length of the section in bytes.</value>
		public override long Length
		{
			get
			{
				return 0;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CodeSection"/> class.
		/// </summary>
		public NullSection()
			: base(Mosa.Compiler.Linker.SectionKind.Text, @"", 0)
		{
			header.Name = 0;
			header.Type = SectionType.Null;
			header.Flags = (SectionAttribute)0;
			header.Address = 0;
			header.Offset = 0;
			header.Size = 0;
			header.Link = 0;
			header.Info = 0;
			header.AddressAlignment = 0;
			header.EntrySize = 0;
		}

		/// <summary>
		/// Writes the specified fs.
		/// </summary>
		/// <param name="writer">The writer.</param>
		public override void Write(BinaryWriter writer)
		{
		}
	}
}
