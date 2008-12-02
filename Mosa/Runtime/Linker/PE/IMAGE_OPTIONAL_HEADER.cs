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
    /// Structure of the optional header in a portable executable file.
    /// </summary>
	public struct IMAGE_OPTIONAL_HEADER 
    {	
		#region Constants

        /// <summary>
        /// The number of directory entries.
        /// </summary>
		public const int IMAGE_NUMBEROF_DIRECTORY_ENTRIES = 16;

        /// <summary>
        /// The magic value at the start of the optional header.
        /// </summary>
        public const ushort IMAGE_OPTIONAL_HEADER_MAGIC = 0x10b;

		#endregion // Constants

		#region Data members

		//
		// Standard fields.
		//

        /// <summary>
        /// Holds the magic value of the optional header.
        /// </summary>
		public ushort Magic;

        /// <summary>
        /// The major version number of the linker.
        /// </summary>
		public byte MajorLinkerVersion;

        /// <summary>
        /// The minor version number of the linker.
        /// </summary>
		public byte MinorLinkerVersion;

        /// <summary>
        /// The size of the code section in bytes.
        /// </summary>
		public uint SizeOfCode;

        /// <summary>
        /// The size of the initialized data section.
        /// </summary>
		public uint SizeOfInitializedData;

        /// <summary>
        /// The size of the uninitialized data section.
        /// </summary>
		public uint SizeOfUninitializedData;

        /// <summary>
        /// The address of the entry point relative to the load address.
        /// </summary>
		public uint AddressOfEntryPoint;

        /// <summary>
        /// The offset from the load address, where the code section starts.
        /// </summary>
		public uint BaseOfCode;

        /// <summary>
        /// The offset from the load address, where the data section starts.
        /// </summary>
		public uint BaseOfData;

		//
		// NT additional fields.
		//

        /// <summary>
        /// The preferred address of the first byte of the image.
        /// </summary>
		public uint ImageBase;

        /// <summary>
        /// The alignment of the sections.
        /// </summary>
        public uint SectionAlignment;

        /// <summary>
        /// The file alignment.
        /// </summary>
		public uint FileAlignment;

        /// <summary>
        /// The major OS version.
        /// </summary>
		public ushort MajorOperatingSystemVersion;

        /// <summary>
        /// The minor OS version.
        /// </summary>
		public ushort MinorOperatingSystemVersion;

        /// <summary>
        /// The major image version.
        /// </summary>
		public ushort MajorImageVersion;

        /// <summary>
        /// The minor image version.
        /// </summary>
		public ushort MinorImageVersion;

        /// <summary>
        /// The major subsystem version.
        /// </summary>
		public ushort MajorSubsystemVersion;

        /// <summary>
        /// The minor subsystem version.
        /// </summary>
		public ushort MinorSubsystemVersion;

        /// <summary>
        /// Must be zero.
        /// </summary>
		public uint Win32VersionValue;

        /// <summary>
        /// The full size of the image.
        /// </summary>
		public uint SizeOfImage;

        /// <summary>
        /// The size of the headers.
        /// </summary>
		public uint SizeOfHeaders;

        /// <summary>
        /// The checksum of the image.
        /// </summary>
		public uint CheckSum;

        /// <summary>
        /// The subsystem to execute the image.
        /// </summary>
		public ushort Subsystem;

        /// <summary>
        /// Flags that control DLL characteristics.
        /// </summary>
		public ushort DllCharacteristics;

        /// <summary>
        /// The size of the stack reserve.
        /// </summary>
		public uint SizeOfStackReserve;

        /// <summary>
        /// The size of the commited stack.
        /// </summary>
		public uint SizeOfStackCommit;

        /// <summary>
        /// Size of the heap reserve.
        /// </summary>
		public uint SizeOfHeapReserve;

        /// <summary>
        /// Size of the committed heap.
        /// </summary>
		public uint SizeOfHeapCommit;

        /// <summary>
        /// Unused. Must be zero.
        /// </summary>
		public uint LoaderFlags;

        /// <summary>
        /// Number of data directories.
        /// </summary>
		public uint NumberOfRvaAndSizes;

        /// <summary>
        /// Array of data directories after the optional header.
        /// </summary>
		public IMAGE_DATA_DIRECTORY[] DataDirectory;

		#endregion // Data members

		#region Methods

        /// <summary>
        /// Loads the header from the reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
		public void Read(BinaryReader reader)
		{
			this.Magic = reader.ReadUInt16();
			if (IMAGE_OPTIONAL_HEADER_MAGIC != Magic)
				throw new BadImageFormatException();

            this.MajorLinkerVersion = reader.ReadByte();
            this.MinorLinkerVersion = reader.ReadByte();
            this.SizeOfCode = reader.ReadUInt32();
            this.SizeOfInitializedData = reader.ReadUInt32();
            this.SizeOfUninitializedData = reader.ReadUInt32();
            this.AddressOfEntryPoint = reader.ReadUInt32();
            this.BaseOfCode = reader.ReadUInt32();
            this.BaseOfData = reader.ReadUInt32();

            this.ImageBase = reader.ReadUInt32();
            this.SectionAlignment = reader.ReadUInt32();
            this.FileAlignment = reader.ReadUInt32();
            this.MajorOperatingSystemVersion = reader.ReadUInt16();
            this.MinorOperatingSystemVersion = reader.ReadUInt16();
            this.MajorImageVersion = reader.ReadUInt16();
            this.MinorImageVersion = reader.ReadUInt16();
            this.MajorSubsystemVersion = reader.ReadUInt16();
            this.MinorSubsystemVersion = reader.ReadUInt16();
            this.Win32VersionValue = reader.ReadUInt32();
            this.SizeOfImage = reader.ReadUInt32();
            this.SizeOfHeaders = reader.ReadUInt32();
            this.CheckSum = reader.ReadUInt32();
            this.Subsystem = reader.ReadUInt16();
            this.DllCharacteristics = reader.ReadUInt16();
            this.SizeOfStackReserve = reader.ReadUInt32();
            this.SizeOfStackCommit = reader.ReadUInt32();
            this.SizeOfHeapReserve = reader.ReadUInt32();
            this.SizeOfHeapCommit = reader.ReadUInt32();
            this.LoaderFlags = reader.ReadUInt32();
            this.NumberOfRvaAndSizes = reader.ReadUInt32();

            this.DataDirectory = new IMAGE_DATA_DIRECTORY[NumberOfRvaAndSizes];
			for (int i = 0; i < this.NumberOfRvaAndSizes; i++)
			{
                this.DataDirectory[i].Read(reader);
			}
		}

        /// <summary>
        /// Writes the structure to the given writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void Write(BinaryWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(@"writer");

            writer.Write(this.Magic);

            writer.Write(this.MajorLinkerVersion);
            writer.Write(this.MinorLinkerVersion);
            writer.Write(this.SizeOfCode);
            writer.Write(this.SizeOfInitializedData);
            writer.Write(this.SizeOfUninitializedData);
            writer.Write(this.AddressOfEntryPoint);
            writer.Write(this.BaseOfCode);
            writer.Write(this.BaseOfData);

            writer.Write(this.ImageBase);
            writer.Write(this.SectionAlignment);
            writer.Write(this.FileAlignment);
            writer.Write(this.MajorOperatingSystemVersion);
            writer.Write(this.MinorOperatingSystemVersion);
            writer.Write(this.MajorImageVersion);
            writer.Write(this.MinorImageVersion);
            writer.Write(this.MajorSubsystemVersion);
            writer.Write(this.MinorSubsystemVersion);
            writer.Write(this.Win32VersionValue);
            writer.Write(this.SizeOfImage);
            writer.Write(this.SizeOfHeaders);
            writer.Write(this.CheckSum);
            writer.Write(this.Subsystem);
            writer.Write(this.DllCharacteristics);
            writer.Write(this.SizeOfStackReserve);
            writer.Write(this.SizeOfStackCommit);
            writer.Write(this.SizeOfHeapReserve);
            writer.Write(this.SizeOfHeapCommit);
            writer.Write(this.LoaderFlags);
            writer.Write(this.NumberOfRvaAndSizes);

            foreach (IMAGE_DATA_DIRECTORY dd in this.DataDirectory)
                dd.Write(writer);
        }

        #endregion // Methods
    }
}
