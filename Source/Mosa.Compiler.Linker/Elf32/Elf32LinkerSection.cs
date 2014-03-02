/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using Mosa.Compiler.Common;
using System.IO;

namespace Mosa.Compiler.Linker.Elf32
{
	/// <summary>
	///
	/// </summary>
	public class Elf32LinkerSection : ExtendedLinkerSection
	{
		/// <summary>
		///
		/// </summary>
		protected SectionHeader header = new SectionHeader();

		/// <summary>
		/// Initializes a new instance of the <see cref="Elf32LinkerSection"/> class.
		/// </summary>
		/// <param name="kind">The kind of the section.</param>
		/// <param name="name">The name.</param>
		/// <param name="virtualAddress">The virtualAddress.</param>
		public Elf32LinkerSection(SectionKind kind, string name, long virtualAddress)
			: base(kind, name, virtualAddress)
		{
			header = new SectionHeader();
			header.Name = StringTableSection.AddString(name);
			stream = new MemoryStream();
		}

		/// <summary>
		/// Gets the length of the section in bytes.
		/// </summary>
		/// <value>The length of the section in bytes.</value>
		public override long Length { get { return stream.Length; } }

		/// <summary>
		/// Gets the header.
		/// </summary>
		/// <value>The header.</value>
		public SectionHeader Header { get { return header; } }

		/// <summary>
		/// Writes the specified fs.
		/// </summary>
		/// <param name="writer">The writer.</param>
		public virtual void Write(BinaryWriter writer)
		{
			Header.Offset = (uint)writer.BaseStream.Position;
			stream.Seek(0, SeekOrigin.Begin);
			stream.WriteTo(writer.BaseStream);
		}

		/// <summary>
		/// Writes the header.
		/// </summary>
		/// <param name="writer">The writer.</param>
		public virtual void WriteHeader(BinaryWriter writer)
		{
			Header.Size = (uint)Length;
			Header.Address = (uint)VirtualAddress;
			Header.Write(writer);
		}
	}
}