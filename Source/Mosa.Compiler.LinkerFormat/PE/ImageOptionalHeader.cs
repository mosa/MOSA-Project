/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using Mosa.Compiler.Common;

namespace Mosa.Compiler.LinkerFormat.PE
{
	/// <summary>
	/// Structure of the optional header in a portable executable file.
	/// </summary>
	public struct ImageOptionalHeader
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
		public ImageDataDirectory[] DataDirectory;

		#endregion // Data members

		#region Methods

		/// <summary>
		/// Loads the header from the reader.
		/// </summary>
		/// <param name="reader">The reader.</param>
		public void Read(EndianAwareBinaryReader reader)
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

			DataDirectory = new ImageDataDirectory[NumberOfRvaAndSizes];
			for (int i = 0; i < NumberOfRvaAndSizes; i++)
			{
				DataDirectory[i].Read(reader);
			}
		}

		/// <summary>
		/// Writes the structure to the given writer.
		/// </summary>
		/// <param name="writer">The writer.</param>
		public void Write(EndianAwareBinaryWriter writer)
		{
			if (writer == null)
				throw new ArgumentNullException(@"writer");

			writer.Write(Magic);
			writer.Write(MajorLinkerVersion);
			writer.Write(MinorLinkerVersion);
			writer.Write(SizeOfCode);
			writer.Write(SizeOfInitializedData);
			writer.Write(SizeOfUninitializedData);
			writer.Write(AddressOfEntryPoint);
			writer.Write(BaseOfCode);
			writer.Write(BaseOfData);

			writer.Write(ImageBase);
			writer.Write(SectionAlignment);
			writer.Write(FileAlignment);
			writer.Write(MajorOperatingSystemVersion);
			writer.Write(MinorOperatingSystemVersion);
			writer.Write(MajorImageVersion);
			writer.Write(MinorImageVersion);
			writer.Write(MajorSubsystemVersion);
			writer.Write(MinorSubsystemVersion);
			writer.Write(Win32VersionValue);
			writer.Write(SizeOfImage);
			writer.Write(SizeOfHeaders);
			writer.Write(CheckSum);
			writer.Write(Subsystem);
			writer.Write(DllCharacteristics);
			writer.Write(SizeOfStackReserve);
			writer.Write(SizeOfStackCommit);
			writer.Write(SizeOfHeapReserve);
			writer.Write(SizeOfHeapCommit);
			writer.Write(LoaderFlags);
			writer.Write(NumberOfRvaAndSizes);

			foreach (ImageDataDirectory dd in DataDirectory)
				dd.Write(writer);
		}

		#endregion // Methods
	}
}
