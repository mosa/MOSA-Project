/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.IO;
using Mosa.Compiler.Common;

namespace Mosa.Compiler.Linker
{
	/// <summary>
	/// Abstract class, that represents sections in an executable file provided by the linker.
	/// </summary>
	public abstract class ExtendedLinkerSection : LinkerSection
	{
		#region Data members

		/// <summary>
		/// Holds the sections data.
		/// </summary>
		protected Stream stream;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LinkerSection"/> class.
		/// </summary>
		/// <param name="kind">The kind of the section.</param>
		/// <param name="name">The name.</param>
		/// <param name="virtualAddress">The virtualAddress.</param>
		protected ExtendedLinkerSection(SectionKind kind, string name, long virtualAddress)
			: base(kind, name, virtualAddress)
		{
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the length of the section in bytes.
		/// </summary>
		/// <value>The length of the section in bytes.</value>
		public override long Length { get { return stream.Length; } }

		#endregion Properties

		#region Methods

		/// <summary>
		/// Allocates the specified size.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="alignment">The alignment.</param>
		/// <returns></returns>
		public virtual Stream Allocate(int size, int alignment)
		{
			// Do we need to ensure a specific alignment?
			InsertPadding(alignment);

			return stream;
		}

		/// <summary>
		/// Pads the stream with zeros until the specific alignment is reached.
		/// </summary>
		/// <param name="alignment">The alignment.</param>
		protected void InsertPadding(int alignment)
		{
			if (alignment == 0)
				return;

			long address = VirtualAddress + stream.Length;
			int mod = (int)(address % alignment);

			if (mod == 0)
				return;

			stream.WriteZeroBytes(alignment - mod);
		}

		/// <summary>
		/// Sizes the type of the of link.
		/// </summary>
		/// <param name="linkType">Type of the link.</param>
		/// <returns></returns>
		/// <exception cref="System.InvalidProgramException"></exception>
		private int SizeOfLinkType(LinkType linkType)
		{
			switch (linkType & LinkType.SizeMask)
			{
				case LinkType.I1: return 1;
				case LinkType.I2: return 2;
				case LinkType.I4: return 4;
				case LinkType.I8: return 8;
			}

			throw new InvalidProgramException();
		}

		/// <summary>
		/// Applies the patch.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="linkType">Type of the link.</param>
		/// <param name="value">The value.</param>
		/// <param name="mask">The mask.</param>
		/// <param name="endianness">The endianness.</param>
		public void ApplyPatch(long offset, LinkType linkType, ulong value, ulong mask, Endianness endianness)
		{
			long originalPosition = stream.Position;
			stream.Position = offset;

			ulong current = 0;

			int linkSize = SizeOfLinkType(linkType);
			if (stream.Position + linkSize > stream.Length)
			{
				stream.SetLength(stream.Position + linkSize);
			}

			switch (linkType & LinkType.SizeMask)
			{
				case LinkType.I1:
					current = (ulong)stream.ReadByte();
					break;

				case LinkType.I2:
					current = (ulong)stream.ReadUInt16(endianness);
					break;

				case LinkType.I4:
					current = (ulong)stream.ReadUInt32(endianness);
					break;

				case LinkType.I8:
					current = (ulong)stream.ReadUInt64(endianness);
					break;
			}

			stream.Position = offset;
			current = (current & ~mask) | value;

			// Apply the patch
			switch (linkType & LinkType.SizeMask)
			{
				case LinkType.I1:
					stream.WriteByte((byte)current);
					break;

				case LinkType.I2:
					stream.Write((ushort)current, endianness);
					break;

				case LinkType.I4:
					stream.Write((uint)current, endianness);
					break;

				case LinkType.I8:
					stream.Write((ulong)current, endianness);
					break;
			}

			stream.Position = originalPosition;
		}

		#endregion Methods
	}
}