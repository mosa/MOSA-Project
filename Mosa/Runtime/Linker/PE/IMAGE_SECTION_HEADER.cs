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
    /// The _header of a section in a portable executable image.
    /// </summary>
	public struct IMAGE_SECTION_HEADER
    {
		#region Data members

        /// <summary>
        /// The name of the section.
        /// </summary>
		public string Name;

        /// <summary>
        /// The size of the section in virtual memory.
        /// </summary>
        public uint VirtualSize;

        /// <summary>
        /// The default address of the section in virtual memory.
        /// </summary>
		public uint VirtualAddress;

        /// <summary>
        /// Size of the raw data in the image file.
        /// </summary>
		public uint SizeOfRawData;

        /// <summary>
        /// Offset of raw data in the image file.
        /// </summary>
        public uint PointerToRawData;

        /// <summary>
        /// Offset of relocations in the image file for this section.
        /// </summary>
        public uint PointerToRelocations;

        /// <summary>
        /// Offset of the line numbers.
        /// </summary>
		public uint PointerToLinenumbers;

        /// <summary>
        /// The number of relocations.
        /// </summary>
        public ushort NumberOfRelocations;

        /// <summary>
        /// The number of line numbers.
        /// </summary>
        public ushort NumberOfLinenumbers;

        /// <summary>
        /// Section characteristic flags.
        /// </summary>
        public uint Characteristics;

		#endregion // Data members

		#region Methods

        /// <summary>
        /// Loads IMAGE_SECTION_HEADER From the reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
		public void Read(BinaryReader reader)
		{
			byte[] name = reader.ReadBytes(8);
            int length = Array.IndexOf<byte>(name, 0);
            if (-1 == length) length = 8;
			this.Name = Encoding.UTF8.GetString(name, 0, length);
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

        /// <summary>
        /// Writes the structure to the given writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void Write(BinaryWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(@"writer");

            byte[] name = Encoding.ASCII.GetBytes(this.Name);
            Array.Resize<byte>(ref name, 8);
            writer.Write(name, 0, 8);
            writer.Write(this.VirtualSize);
            writer.Write(this.VirtualAddress);
            writer.Write(this.SizeOfRawData);
            writer.Write(this.PointerToRawData);
            writer.Write(this.PointerToRelocations);
            writer.Write(this.PointerToLinenumbers);
            writer.Write(this.NumberOfRelocations);
            writer.Write(this.NumberOfLinenumbers);
            writer.Write(this.Characteristics);
        }

        #endregion // Methods
    }
}
