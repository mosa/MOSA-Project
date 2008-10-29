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
    /// Structure of the COFF file header at the start of a portable executable file.
    /// </summary>
	public struct IMAGE_FILE_HEADER
    {
		#region Constants

		private const ushort IMAGE_FILE_MACHINE_I386 = 0x014c;

		#endregion // Constants

		#region Data members

        /// <summary>
        /// Identifies the target machine of the object file.
        /// </summary>
		public ushort Machine;

        /// <summary>
        /// Number of sections in the section table.
        /// </summary>
		public ushort NumberOfSections;

        /// <summary>
        /// The low 32-bits of the number of seconds since 00:00 January 1, 1970 that indicates when the file was created.
        /// </summary>
        public uint TimeDateStamp;

        /// <summary>
        /// The file offset of the COFF symbol table.
        /// </summary>
		public uint PointerToSymbolTable;

        /// <summary>
        /// The number of entries in the symbol table.
        /// </summary>
		public uint NumberOfSymbols;

        /// <summary>
        /// The size of the optional header.
        /// </summary>
		public ushort SizeOfOptionalHeader;

        /// <summary>
        /// Set of flags, that indicate the attributes of the object file.
        /// </summary>
        public ushort Characteristics;

		#endregion // Data members

		#region Methods

		/// <summary>
		/// Loads and validates the image file header.
		/// </summary>
		/// <param name="reader">The reader, to read from.</param>
		public void Load(BinaryReader reader)
		{
			Machine = reader.ReadUInt16();
			NumberOfSections = reader.ReadUInt16();
			TimeDateStamp = reader.ReadUInt32();
			PointerToSymbolTable = reader.ReadUInt32();
			NumberOfSymbols = reader.ReadUInt32();
			SizeOfOptionalHeader = reader.ReadUInt16();
			Characteristics = reader.ReadUInt16();

			if (Machine != IMAGE_FILE_MACHINE_I386)
				throw new BadImageFormatException(@"Unknown machine identifier type.");
		}

		#endregion // Methods
	}
}
