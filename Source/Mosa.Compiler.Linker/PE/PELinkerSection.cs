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
	public class PELinkerSection : LinkerSectionExtended
	{

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="PELinkerSection"/> class.
		/// </summary>
		/// <param name="kind">The kind of the section.</param>
		/// <param name="name">The name of the section.</param>
		/// <param name="virtualAddress">The virtualAddress of the section.</param>
		public PELinkerSection(SectionKind kind, string name, long virtualAddress) :
			base(kind, name, virtualAddress)
		{
			stream = new MemoryStream();
		}

		#endregion // Construction

		#region Methods

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
				case LinkType.NativeI1:
					stream.WriteByte((byte)value);
					break;

				case LinkType.NativeI2:
					stream.Write((ushort)value, true); // FIXME
					break;

				case LinkType.NativeI4:
					stream.Write((uint)value, true); // FIXME
					break;

				case LinkType.NativeI8:
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

	}
}
