/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Mosa.Runtime.Linker.PE
{
    /// <summary>
    /// The CLI header embedded into a portable executable file.
    /// </summary>
	public struct CLI_HEADER 
    {
		#region Constants

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
			this.Cb = reader.ReadUInt32();
			this.MajorRuntimeVersion = reader.ReadUInt16();
			this.MinorRuntimeVersion = reader.ReadUInt16();
			this.Metadata.Read(reader);
			this.Flags = (RuntimeImageFlags)reader.ReadUInt32();
			this.EntryPointToken = reader.ReadUInt32();
			this.Resources.Read(reader);
			this.StrongNameSignature.Read(reader);
			this.CodeManagerTable.Read(reader);
			this.VTableFixups.Read(reader);
			this.ExportAddressTableJumps.Read(reader);
			this.ManagedNativeHeader.Read(reader);
		}

		#endregion // Methods
	}
}
