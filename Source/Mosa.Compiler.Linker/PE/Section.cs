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

		#region Data members

		/// <summary>
		/// Holds the sections data.
		/// </summary>
		private MemoryStream stream;

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
			stream = new MemoryStream();
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

			return stream;
		}

		/// <summary>
		/// Patches the section at the given offset with the specified value.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="linkType">Type of the link.</param>
		/// <param name="value">The value.</param>
		public void ApplyPatch(long offset, LinkType linkType, long value)
		{
			long pos = stream.Position;
			stream.Position = offset;

			// Apply the patch
			switch (linkType & LinkType.SizeMask)
			{
				case LinkType.I1:
					stream.WriteByte((byte)value);
					break;

				case LinkType.I2:
					stream.Write((ushort)value, true); // FIXME
					break;

				case LinkType.I4:
					stream.Write((uint)value, true); // FIXME
					break;

				case LinkType.I8:
					stream.Write((ulong)value, true); // FIXME
					break;
			}

			stream.Position = pos;
		}

		/// <summary>
		/// Stores the section contents to the given writer.
		/// </summary>
		/// <param name="writer">The writer.</param>
		public void Write(BinaryWriter writer)
		{
			stream.WriteTo(writer.BaseStream);
		}

		#endregion // Methods

		#region LinkerSection Overrides

		/// <summary>
		/// Gets the length of the section in bytes.
		/// </summary>
		/// <value>The length of the section in bytes.</value>
		public override long Length
		{
			get { return stream.Length; }
		}

		#endregion // LinkerSection Overrides

		#region Internals

		/// <summary>
		/// Pads the stream with zeros until the specific alignment is reached.
		/// </summary>
		/// <param name="alignment">The alignment.</param>
		private void InsertPadding(int alignment)
		{
			long address = this.VirtualAddress.ToInt64() + stream.Length;
			int pad = (int)(alignment - (address % alignment));
			if (pad < alignment)
			{
				stream.Write(new byte[pad], 0, pad);
			}
		}

		#endregion // Internals
	}
}
