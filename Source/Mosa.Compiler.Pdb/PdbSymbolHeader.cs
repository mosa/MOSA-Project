// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.IO;

namespace Mosa.Compiler.Pdb
{
	/// <summary>
	///
	/// </summary>
	public struct PdbSymbolHeader
	{
		#region Data Members

		private int signature;
		private int version;
		private int unknown;
		private int hash1_file;
		private int hash2_file;

		/// <summary>
		/// Defines the PDB stream, that contains the global symbols.
		/// </summary>
		public short gsym_stream;

		private short unknown1;

		/// <summary>
		/// Defines the size of the modules following this symbol header.
		/// </summary>
		public int module_size;

		private int offset_size;
		private int hash_size;
		private int srcmodule_size;
		private int pdbimport_size;
		private int[] resvd;

		#endregion Data Members

		#region Methods

		/// <summary>
		/// Reads the specified reader.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="header">The header.</param>
		/// <returns></returns>
		public static bool Read(BinaryReader reader, out PdbSymbolHeader header)
		{
			if (reader == null)
				throw new ArgumentNullException(@"reader");

			header.signature = reader.ReadInt32();
			header.version = reader.ReadInt32();
			header.unknown = reader.ReadInt32();
			header.hash1_file = reader.ReadInt32();
			header.hash2_file = reader.ReadInt32();
			header.gsym_stream = reader.ReadInt16();
			header.unknown1 = reader.ReadInt16();
			header.module_size = reader.ReadInt32();
			header.offset_size = reader.ReadInt32();
			header.hash_size = reader.ReadInt32();
			header.srcmodule_size = reader.ReadInt32();
			header.pdbimport_size = reader.ReadInt32();

			header.resvd = new int[5];
			for (int i = 0; i < 5; i++)
				header.resvd[i] = reader.ReadInt32();

			return true;
		}

		#endregion Methods
	}
}