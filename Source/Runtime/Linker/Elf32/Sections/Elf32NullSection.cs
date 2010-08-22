/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;

namespace Mosa.Runtime.Linker.Elf32.Sections
{
	/// <summary>
	/// 
	/// </summary>
	public class Elf32NullSection : Elf32Section
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
		/// Initializes a new instance of the <see cref="Elf32CodeSection"/> class.
		/// </summary>
		public Elf32NullSection()
			: base(SectionKind.Text, @"", IntPtr.Zero)
		{
			_header.Name = 0;
			_header.Type = Elf32SectionType.Null;
			_header.Flags = 0;
			_header.Address = 0;
			_header.Offset = 0;
			_header.Size = 0;
			_header.Link = 0;
			_header.Info = 0;
			_header.AddressAlignment = 0;
			_header.EntrySize = 0;
		}

		/// <summary>
		/// Writes the specified fs.
		/// </summary>
		/// <param name="writer">The writer.</param>
		public override void Write(System.IO.BinaryWriter writer)
		{
		}
	}
}
