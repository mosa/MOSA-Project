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
		public const uint PE_SIGNATURE = 0x00004550;

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
		public void Read(BinaryReader reader)
		{
            this.Signature = reader.ReadUInt32();
            if (this.Signature != PE_SIGNATURE)
				throw new BadImageFormatException();

            this.FileHeader.Read(reader);
            this.OptionalHeader.Read(reader);
		}

        /// <summary>
        /// Writes the structure to the given writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void Write(BinaryWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(@"writer");

            writer.Write(this.Signature);
            this.FileHeader.Write(writer);
            this.OptionalHeader.Write(writer);
        }

		#endregion // Methods
    }
}
