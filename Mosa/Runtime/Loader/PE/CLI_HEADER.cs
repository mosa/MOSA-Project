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

namespace Mosa.Runtime.Loader.PE {
	struct CLI_HEADER {

		#region Constants

		#endregion // Constants

		#region Data members

		public uint Cb;
		public ushort MajorRuntimeVersion;
		public ushort MinorRuntimeVersion;
		public IMAGE_DATA_DIRECTORY Metadata;
		public RuntimeImageFlags Flags;
		public uint EntryPointToken;
		public IMAGE_DATA_DIRECTORY Resources;
		public IMAGE_DATA_DIRECTORY StrongNameSignature;
		public IMAGE_DATA_DIRECTORY CodeManagerTable;
		public IMAGE_DATA_DIRECTORY VTableFixups;
		public IMAGE_DATA_DIRECTORY ExportAddressTableJumps;
		public IMAGE_DATA_DIRECTORY ManagedNativeHeader;

		// FIXME: public byte[] ImageHash;

		#endregion // Data members

		#region Methods

		public void Load(BinaryReader reader)
		{
			this.Cb = reader.ReadUInt32();
			this.MajorRuntimeVersion = reader.ReadUInt16();
			this.MinorRuntimeVersion = reader.ReadUInt16();
			this.Metadata.Load(reader);
			this.Flags = (RuntimeImageFlags)reader.ReadUInt32();
			this.EntryPointToken = reader.ReadUInt32();
			this.Resources.Load(reader);
			this.StrongNameSignature.Load(reader);
			this.CodeManagerTable.Load(reader);
			this.VTableFixups.Load(reader);
			this.ExportAddressTableJumps.Load(reader);
			this.ManagedNativeHeader.Load(reader);
		}

		#endregion // Methods

	}
}
