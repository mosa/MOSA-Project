/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.IO;

using Mosa.Compiler.Common;

namespace Mosa.Compiler.Linker.PE
{
	/// <summary>
	/// An implementation of a portable executable linker section.
	/// </summary>
	public class Section : LinkerSection
	{
		private readonly DataConverter LittleEndianBitConverter = DataConverter.LittleEndian;

		#region Data members

		/// <summary>
		/// Holds the sections data.
		/// </summary>
		private MemoryStream sectionStream;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Section"/> class.
		/// </summary>
		/// <param name="kind">The kind of the section.</param>
		/// <param name="name">The name of the section.</param>
		/// <param name="virtualAddress">The virtualAddress of the section.</param>
		public Section(SectionKind kind, string name, IntPtr virtualAddress) :
			base(kind, name, virtualAddress)
		{
			sectionStream = new MemoryStream();
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Allocates the specified number of bytes in the section.
		/// </summary>
		/// <param name="size">The number of bytes to allocate.</param>
		/// <param name="alignment">The alignment.</param>
		/// <returns>A stream, used to write to the allocated memory region.</returns>
		public Stream Allocate(int size, int alignment)
		{
			// Do we need to ensure a specific alignment?
			if (alignment > 1)
				InsertPadding(alignment);

			return sectionStream;
		}

		/// <summary>
		/// Patches the section at the given offset with the specified value.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="linkType">Type of the link.</param>
		/// <param name="value">The value.</param>
		public void ApplyPatch(long offset, LinkType linkType, long value)
		{
			long pos = sectionStream.Position;
			sectionStream.Position = offset;

			// Apply the patch
			switch (linkType & LinkType.SizeMask)
			{
				case LinkType.I1:
					sectionStream.WriteByte((byte)value);
					break;

				case LinkType.I2:
					sectionStream.Write(LittleEndianBitConverter.GetBytes((ushort)value), 0, 2);
					break;

				case LinkType.I4:
					sectionStream.Write(LittleEndianBitConverter.GetBytes((uint)value), 0, 4);
					break;

				case LinkType.I8:
					sectionStream.Write(LittleEndianBitConverter.GetBytes(value), 0, 8);
					break;
			}

			sectionStream.Position = pos;
		}

		/// <summary>
		/// Stores the section contents to the given writer.
		/// </summary>
		/// <param name="writer">The writer.</param>
		public void Write(BinaryWriter writer)
		{
			sectionStream.WriteTo(writer.BaseStream);
		}

		#endregion // Methods

		#region LinkerSection Overrides

		/// <summary>
		/// Gets the length of the section in bytes.
		/// </summary>
		/// <value>The length of the section in bytes.</value>
		public override long Length
		{
			get { return sectionStream.Length; }
		}

		#endregion // LinkerSection Overrides

		#region Internals

		/// <summary>
		/// Pads the stream with zeros until the specific alignment is reached.
		/// </summary>
		/// <param name="alignment">The alignment.</param>
		private void InsertPadding(int alignment)
		{
			long address = this.VirtualAddress.ToInt64() + sectionStream.Length;
			int pad = (int)(alignment - (address % alignment));
			if (pad < alignment)
			{
				sectionStream.Write(new byte[pad], 0, pad);
			}
		}

		#endregion // Internals
	}
}
