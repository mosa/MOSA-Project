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

namespace Mosa.Runtime.Loader.PE {
	struct IMAGE_DATA_DIRECTORY {
		#region Data members

		public uint VirtualAddress;
		public int Size;

		#endregion // Data members

		#region Methods

		public void Load(BinaryReader reader)
		{
			this.VirtualAddress = reader.ReadUInt32();
			this.Size = reader.ReadInt32();
		}

		#endregion // Methods
	}
}
