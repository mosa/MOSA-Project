// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System;

namespace Mosa.Compiler.LinkerFormat.PE
{
	/// <summary>
	/// Represents a data directory in a portable executable image.
	/// </summary>
	public struct ImageDataDirectory
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

		#endregion Data members

		#region Methods

		/// <summary>
		/// Loads the IMAGE_DATA_DIRECTORY from the reader.
		/// </summary>
		/// <param name="reader">The reader.</param>
		public void Read(EndianAwareBinaryReader reader)
		{
			VirtualAddress = reader.ReadUInt32();
			Size = reader.ReadInt32();
		}

		/// <summary>
		/// Writes the structure to the given writer.
		/// </summary>
		/// <param name="writer">The writer.</param>
		public void Write(EndianAwareBinaryWriter writer)
		{
			if (writer == null)
				throw new ArgumentNullException(@"writer");

			writer.Write(this.VirtualAddress);
			writer.Write(this.Size);
		}

		#endregion Methods
	}
}