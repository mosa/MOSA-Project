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

namespace Mosa.Compiler.Linker
{
	/// <summary>
	/// Abstract class, that represents sections in an executable file provided by the linker.
	/// </summary>
	public abstract class LinkerSectionExtended : LinkerSection
	{
		#region Data members

		/// <summary>
		/// Holds the sections data.
		/// </summary>
		protected Stream stream; 

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LinkerSection"/> class.
		/// </summary>
		/// <param name="kind">The kind of the section.</param>
		/// <param name="name">The name.</param>
		/// <param name="virtualAddress">The virtualAddress.</param>
		protected LinkerSectionExtended(SectionKind kind, string name, long virtualAddress)
			: base(kind, name, virtualAddress)
		{
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the length of the section in bytes.
		/// </summary>
		/// <value>The length of the section in bytes.</value>
		public override long Length { get { return stream.Length; } }

		#endregion // Properties

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
			if (alignment > 1)
				InsertPadding(alignment);

			return stream;
		}

		/// <summary>
		/// Pads the stream with zeros until the specific alignment is reached.
		/// </summary>
		/// <param name="alignment">The alignment.</param>
		protected void InsertPadding(int alignment)
		{
			long address = VirtualAddress + stream.Length;
			int pad = (int)(alignment - (address % alignment));
			stream.Write(new byte[pad], 0, pad);
		}

		#endregion // Methods

	}
}
