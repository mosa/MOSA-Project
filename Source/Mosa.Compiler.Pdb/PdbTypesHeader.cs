/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Mosa.Compiler.Pdb
{
	/// <summary>
	/// 
	/// </summary>
	public struct PdbTypesHeader
	{
		#region Data Members

		private int version;
		private int type_offset;
		private int first_index;
		private int last_index;
		private int type_size;
		private short file;
		private short pad;
		private int hash_size;
		private int hash_base;
		private int hash_offset;
		private int hash_len;
		private int search_offset;
		private int search_len;
		private int unknown_offset;
		private int unknown_len;

		#endregion // Data Members

		#region Methods

		/// <summary>
		/// Reads the specified reader.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="header">The header.</param>
		/// <returns></returns>
		public static bool Read(BinaryReader reader, out PdbTypesHeader header)
		{
			if (reader == null)
				throw new ArgumentNullException(@"reader");

			header.version = reader.ReadInt32();
			header.type_offset = reader.ReadInt32();
			header.first_index = reader.ReadInt32();
			header.last_index = reader.ReadInt32();
			header.type_size = reader.ReadInt32();
			header.file = reader.ReadInt16();
			header.pad = reader.ReadInt16();
			header.hash_size = reader.ReadInt32();
			header.hash_base = reader.ReadInt32();
			header.hash_offset = reader.ReadInt32();
			header.hash_len = reader.ReadInt32();
			header.search_offset = reader.ReadInt32();
			header.search_len = reader.ReadInt32();
			header.unknown_offset = reader.ReadInt32();
			header.unknown_len = reader.ReadInt32();

			return true;
		}

		#endregion // Methods
	}
}
