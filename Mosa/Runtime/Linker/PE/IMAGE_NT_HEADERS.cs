/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.IO;

namespace Mosa.Runtime.Linker.PE
{
    /// <summary>
    /// Combines the PE signature with the image file and optional headers.
    /// </summary>
	public struct IMAGE_NT_HEADERS
    {
		#region Constants

        /// <summary>
        /// The signature for PE files.
        /// </summary>
		private const uint PE_SIGNATURE = 0x00004550;

		#endregion // Constants

		#region Data members

        /// <summary>
        /// Holds the portable executable signature.
        /// </summary>
		public uint Signature;

        /// <summary>
        /// Holds the image file header.
        /// </summary>
        public IMAGE_FILE_HEADER FileHeader;

        /// <summary>
        /// Holds the image optional header.
        /// </summary>
        public IMAGE_OPTIONAL_HEADER OptionalHeader;

		#endregion // Data members

		#region Methods

		/// <summary>
		/// Loads and validates the image file header.
		/// </summary>
		/// <param name="reader">The reader, to read from.</param>
		public void Load(BinaryReader reader)
		{
			Signature = reader.ReadUInt32();
			if (Signature != PE_SIGNATURE)
				throw new BadImageFormatException();

			FileHeader.Load(reader);
			OptionalHeader.Load(reader);
		}

		#endregion // Methods
	}
}
