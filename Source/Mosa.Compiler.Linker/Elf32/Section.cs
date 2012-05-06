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
using Mosa.Compiler.Common;

namespace Mosa.Compiler.Linker.Elf32
{
	/// <summary>
	/// 
	/// </summary>
	public class Section : LinkerSection
	{

		/// <summary>
		/// 
		/// </summary>
		protected SectionHeader header = new SectionHeader();
		/// <summary>
		/// 
		/// </summary>
		protected MemoryStream stream;

		/// <summary>
		/// Initializes a new instance of the <see cref="Section"/> class.
		/// </summary>
		/// <param name="kind">The kind of the section.</param>
		/// <param name="name">The name.</param>
		/// <param name="virtualAddress">The virtualAddress.</param>
		public Section(SectionKind kind, string name, IntPtr virtualAddress)
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
		/// Allocates the specified size.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="alignment">The alignment.</param>
		/// <returns></returns>
		public Stream Allocate(int size, int alignment)
		{
			// Do we need to ensure a specific alignment?
			if (alignment > 1)
				InsertPadding(alignment);

			return stream;
		}

		/// <summary>
		/// Writes the specified fs.
		/// </summary>
		/// <param name="writer">The writer.</param>
		public virtual void Write(BinaryWriter writer)
		{
			Header.Offset = (uint)writer.BaseStream.Position;
			stream.WriteTo(writer.BaseStream);
		}

		/// <summary>
		/// Writes the header.
		/// </summary>
		/// <param name="writer">The writer.</param>
		public virtual void WriteHeader(BinaryWriter writer)
		{
			Header.Size = (uint)Length;
			Header.Write(writer);
		}

		/// <summary>
		/// Patches the section at the given offset with the specified value.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="linkType">Type of the link.</param>
		/// <param name="value">The value.</param>
		public void ApplyPatch(long offset, LinkType linkType, long value, bool isLittleEndian)
		{
			long pos = stream.Position;
			stream.Position = offset;

			// Apply the patch
			switch (linkType & LinkType.SizeMask)
			{
				case LinkType.NativeI1:
					stream.WriteByte((byte)value);
					break;

				case LinkType.NativeI2:
					stream.Write((ushort)value, isLittleEndian);
					break;

				case LinkType.NativeI4:
					stream.Write((uint)value, isLittleEndian);
					break;

				case LinkType.NativeI8:
					stream.Write((ulong)value, isLittleEndian);
					break;
			}

			stream.Position = pos;
		}

		#region Internals

		/// <summary>
		/// Pads the stream with zeros until the specific alignment is reached.
		/// </summary>
		/// <param name="alignment">The alignment.</param>
		private void InsertPadding(int alignment)
		{
			long address = VirtualAddress.ToInt64() + stream.Length;
			int pad = (int)(alignment - (address % alignment));
			stream.Write(new byte[pad], 0, pad);
		}

		#endregion // Internals
	}
}
