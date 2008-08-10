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
	struct IMAGE_SECTION_HEADER {

		#region Data members

		public string name;
		public uint VirtualSize;
		public uint VirtualAddress;
		public uint SizeOfRawData;
		public uint PointerToRawData;
		public uint PointerToRelocations;
		public uint PointerToLinenumbers;
		public ushort NumberOfRelocations;
		public ushort NumberOfLinenumbers;
		public uint Characteristics;

		#endregion // Data members

		#region Methods

		public void Load(BinaryReader reader)
		{
			byte[] name = reader.ReadBytes(8);
            int length = Array.IndexOf<byte>(name, 0);
            if (-1 == length) length = 8;
			this.name = Encoding.UTF8.GetString(name, 0, length);
			this.VirtualSize = reader.ReadUInt32();
			this.VirtualAddress = reader.ReadUInt32();
			this.SizeOfRawData = reader.ReadUInt32();
			this.PointerToRawData = reader.ReadUInt32();
			this.PointerToRelocations = reader.ReadUInt32();
			this.PointerToLinenumbers = reader.ReadUInt32();
			this.NumberOfRelocations = reader.ReadUInt16();
			this.NumberOfLinenumbers = reader.ReadUInt16();
			this.Characteristics = reader.ReadUInt32();
		}

		#endregion // Methods

	}
}
