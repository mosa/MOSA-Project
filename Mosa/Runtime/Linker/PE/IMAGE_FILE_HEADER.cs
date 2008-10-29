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
	struct IMAGE_FILE_HEADER {

		#region Constants

		private const ushort IMAGE_FILE_MACHINE_I386 = 0x014c;

		#endregion // Constants

		#region Data members

		public ushort Machine;
		public ushort NumberOfSections;
		public uint TimeDateStamp;
		public uint PointerToSymbolTable;
		public uint NumberOfSymbols;
		public ushort SizeOfOptionalHeader;
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
