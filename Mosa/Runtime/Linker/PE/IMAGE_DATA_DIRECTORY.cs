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

namespace Mosa.Runtime.Linker.PE
{
    /// <summary>
    /// Represents a data directory in a portable executable image.
    /// </summary>
	public struct IMAGE_DATA_DIRECTORY
    {
		#region Data members

        /// <summary>
        /// The virtual address of the data.
        /// </summary>
		public uint VirtualAddress;

        /// <summary>
        /// The size of the data.
        /// </summary>
        public int Size;

		#endregion // Data members

		#region Methods

        /// <summary>
        /// Loads the IMAGE_DATA_DIRECTORY from the reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
		public void Load(BinaryReader reader)
		{
			this.VirtualAddress = reader.ReadUInt32();
			this.Size = reader.ReadInt32();
		}

		#endregion // Methods
	}
}
