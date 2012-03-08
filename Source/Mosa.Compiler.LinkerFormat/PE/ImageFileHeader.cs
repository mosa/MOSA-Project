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

namespace Mosa.Compiler.LinkerFormat.PE
{
	/// <summary>
	/// Structure of the COFF file header at the start of a portable executable file.
	/// </summary>
	public struct ImageFileHeader
	{
		#region Constants

		/// <summary>
		/// The PE machine type for I386.
		/// </summary>
		public const ushort IMAGE_FILE_MACHINE_I386 = 0x014c;

		/// <summary>
		/// Characteristic flag, which indicates the file is a DLL.
		/// </summary>
		public const ushort IMAGE_FILE_DLL = 0x2000;

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
		/// <param name="reader">The reader, to read From.</param>
		public void Read(BinaryReader reader)
		{
			this.Machine = reader.ReadUInt16();
			this.NumberOfSections = reader.ReadUInt16();
			this.TimeDateStamp = reader.ReadUInt32();
			this.PointerToSymbolTable = reader.ReadUInt32();
			this.NumberOfSymbols = reader.ReadUInt32();
			this.SizeOfOptionalHeader = reader.ReadUInt16();
			this.Characteristics = reader.ReadUInt16();

			if (this.Machine != IMAGE_FILE_MACHINE_I386)
				throw new BadImageFormatException(@"Unknown machine identifier type.");
		}

		/// <summary>
		/// Writes the structure to the given writer.
		/// </summary>
		/// <param name="writer">The writer.</param>
		public void Write(BinaryWriter writer)
		{
			if (writer == null)
				throw new ArgumentNullException(@"writer");

			writer.Write(this.Machine);
			writer.Write(this.NumberOfSections);
			writer.Write(this.TimeDateStamp);
			writer.Write(this.PointerToSymbolTable);
			writer.Write(this.NumberOfSymbols);
			writer.Write(this.SizeOfOptionalHeader);
			writer.Write(this.Characteristics);
		}

		#endregion // Methods
	}
}
