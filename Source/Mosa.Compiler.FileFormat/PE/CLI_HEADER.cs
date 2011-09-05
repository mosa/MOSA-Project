/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Mosa.Compiler.FileFormat.PE
{
	/// <summary>
	/// The CLI header embedded into a portable executable file.
	/// </summary>
	public struct CLI_HEADER
	{
		#region Constants

		/// <summary>
		/// Size of the CLI Header
		/// </summary>
		public static int Length = 0x250 - 0x208;

		/// <summary>
		/// Name for symbol
		/// </summary>
		public const string SymbolName = @".cil.header";

		#endregion // Constants

		#region Data members

		/// <summary>
		/// The size of the CLI header in bytes.
		/// </summary>
		public uint Cb;

		/// <summary>
		/// The major runtime version needed to execute the image.
		/// </summary>
		public ushort MajorRuntimeVersion;

		/// <summary>
		/// The minor runtime version needed to execute the image.
		/// </summary>
		public ushort MinorRuntimeVersion;

		/// <summary>
		/// The metadata data directory.
		/// </summary>
		public IMAGE_DATA_DIRECTORY Metadata;

		/// <summary>
		/// Flags for the entire image.
		/// </summary>
		public RuntimeImageFlags Flags;

		/// <summary>
		/// The token of the method, that represents the entry point.
		/// </summary>
		public uint EntryPointToken;

		/// <summary>
		/// Data directory of the resources.
		/// </summary>
		public IMAGE_DATA_DIRECTORY Resources;

		/// <summary>
		/// The data directory of the strong name signature.
		/// </summary>
		public IMAGE_DATA_DIRECTORY StrongNameSignature;

		/// <summary>
		/// The data directory of the code manager table.
		/// </summary>
		public IMAGE_DATA_DIRECTORY CodeManagerTable;

		/// <summary>
		/// The data directory of vtable fixups.
		/// </summary>
		public IMAGE_DATA_DIRECTORY VTableFixups;

		/// <summary>
		/// The data directory of export addresses.
		/// </summary>
		public IMAGE_DATA_DIRECTORY ExportAddressTableJumps;

		/// <summary>
		/// The data directory of the managed native header.
		/// </summary>
		public IMAGE_DATA_DIRECTORY ManagedNativeHeader;

		// FIXME: public byte[] ImageHash;

		#endregion // Data members

		#region Methods

		/// <summary>
		/// Loads the CLI_HEADER from the reader.
		/// </summary>
		/// <param name="reader">The reader.</param>
		public void Read(BinaryReader reader)
		{
			Cb = reader.ReadUInt32();
			MajorRuntimeVersion = reader.ReadUInt16();
			MinorRuntimeVersion = reader.ReadUInt16();
			Metadata.Read(reader);
			Flags = (RuntimeImageFlags)reader.ReadUInt32();
			EntryPointToken = reader.ReadUInt32();
			Resources.Read(reader);
			StrongNameSignature.Read(reader);
			CodeManagerTable.Read(reader);
			VTableFixups.Read(reader);
			ExportAddressTableJumps.Read(reader);
			ManagedNativeHeader.Read(reader);
		}

		/// <summary>
		/// Writes the header to the given binary writer.
		/// </summary>
		/// <param name="writer">The binary writer to write to.</param>
		public void WriteTo(BinaryWriter writer)
		{
			writer.Write(Cb);
			writer.Write(MajorRuntimeVersion);
			writer.Write(MinorRuntimeVersion);
			Metadata.Write(writer);
			writer.Write((uint)Flags);
			writer.Write(EntryPointToken);
			Resources.Write(writer);
			StrongNameSignature.Write(writer);
			CodeManagerTable.Write(writer);
			VTableFixups.Write(writer);
			ExportAddressTableJumps.Write(writer);
			ManagedNativeHeader.Write(writer);
		}

		#endregion // Methods
	}
}
