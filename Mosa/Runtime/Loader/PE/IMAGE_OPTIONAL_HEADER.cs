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
	struct IMAGE_OPTIONAL_HEADER {
		
		#region Constants

		private const int IMAGE_NUMBEROF_DIRECTORY_ENTRIES = 16;
		private const ushort IMAGE_OPTIONAL_HEADER_MAGIC = 0x10b;

		#endregion // Constants

		#region Data members

		//
		// Standard fields.
		//

		public ushort Magic;
		public byte MajorLinkerVersion;
		public byte MinorLinkerVersion;
		public uint SizeOfCode;
		public uint SizeOfInitializedData;
		public uint SizeOfUninitializedData;
		public uint AddressOfEntryPoint;
		public uint BaseOfCode;
		public uint BaseOfData;

		//
		// NT additional fields.
		//

		public uint ImageBase;
		public uint SectionAlignment;
		public uint FileAlignment;
		public ushort MajorOperatingSystemVersion;
		public ushort MinorOperatingSystemVersion;
		public ushort MajorImageVersion;
		public ushort MinorImageVersion;
		public ushort MajorSubsystemVersion;
		public ushort MinorSubsystemVersion;
		public uint Win32VersionValue;
		public uint SizeOfImage;
		public uint SizeOfHeaders;
		public uint CheckSum;
		public ushort Subsystem;
		public ushort DllCharacteristics;
		public uint SizeOfStackReserve;
		public uint SizeOfStackCommit;
		public uint SizeOfHeapReserve;
		public uint SizeOfHeapCommit;
		public uint LoaderFlags;
		public uint NumberOfRvaAndSizes;
		public IMAGE_DATA_DIRECTORY[] DataDirectory;

		#endregion // Data members

		#region Methods

		public void Load(BinaryReader reader)
		{
			Magic = reader.ReadUInt16();
			if (IMAGE_OPTIONAL_HEADER_MAGIC != Magic)
				throw new BadImageFormatException();

			MajorLinkerVersion = reader.ReadByte();
			MinorLinkerVersion = reader.ReadByte();
			SizeOfCode = reader.ReadUInt32();
			SizeOfInitializedData = reader.ReadUInt32();
			SizeOfUninitializedData = reader.ReadUInt32();
			AddressOfEntryPoint = reader.ReadUInt32();
			BaseOfCode = reader.ReadUInt32();
			BaseOfData = reader.ReadUInt32();

			ImageBase = reader.ReadUInt32();
			SectionAlignment = reader.ReadUInt32();
			FileAlignment = reader.ReadUInt32();
			MajorOperatingSystemVersion = reader.ReadUInt16();
			MinorOperatingSystemVersion = reader.ReadUInt16();
			MajorImageVersion = reader.ReadUInt16();
			MinorImageVersion = reader.ReadUInt16();
			MajorSubsystemVersion = reader.ReadUInt16();
			MinorSubsystemVersion = reader.ReadUInt16();
			Win32VersionValue = reader.ReadUInt32();
			SizeOfImage = reader.ReadUInt32();
			SizeOfHeaders = reader.ReadUInt32();
			CheckSum = reader.ReadUInt32();
			Subsystem = reader.ReadUInt16();
			DllCharacteristics = reader.ReadUInt16();
			SizeOfStackReserve = reader.ReadUInt32();
			SizeOfStackCommit = reader.ReadUInt32();
			SizeOfHeapReserve = reader.ReadUInt32();
			SizeOfHeapCommit = reader.ReadUInt32();
			LoaderFlags = reader.ReadUInt32();
			NumberOfRvaAndSizes = reader.ReadUInt32();

			DataDirectory = new IMAGE_DATA_DIRECTORY[NumberOfRvaAndSizes];
			for (int i = 0; i < NumberOfRvaAndSizes; i++)
			{
				DataDirectory[i].Load(reader);
			}
		}

		#endregion // Methods
	}
}
