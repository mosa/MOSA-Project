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
using System.IO;
using System.Diagnostics;

namespace Mosa.Compiler.Pdb
{
	/// <summary>
	/// 
	/// </summary>
	public struct PdbRootStream
	{
		/// <summary>
		/// Holds the number of streams in the PDB file.
		/// </summary>
		public int streams;

		/// <summary>
		/// Holds the length of all streams.
		/// </summary>
		public int[] streamLength;

		/// <summary>
		/// Holds the stream pages.
		/// </summary>
		public int[][] streamPages;

		/// <summary>
		/// Reads the specified reader.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="pageSize">Holds the size of a single page in the file.</param>
		/// <param name="rootStream">The root stream.</param>
		/// <returns></returns>
		public static bool Read(BinaryReader reader, int pageSize, out PdbRootStream rootStream)
		{
			rootStream.streams = reader.ReadInt32();
			Debug.WriteLine(String.Format(@"PdbRootStream: PDB file contains {0} streams.", rootStream.streams));
			rootStream.streamLength = new int[rootStream.streams];
			rootStream.streamPages = new int[rootStream.streams][];
			for (int i = 0; i < rootStream.streams; i++)
				rootStream.streamLength[i] = reader.ReadInt32();

			for (int i = 0; i < rootStream.streams; i++)
			{
				Debug.WriteLine(String.Format("\tPDB Stream #{0} (Length {1} bytes)", i, rootStream.streamLength[i]));
				if (rootStream.streamLength[i] > 0)
				{
					rootStream.streamPages[i] = new int[(rootStream.streamLength[i] / pageSize) + 1];
					for (int j = 0; j < rootStream.streamPages[i].Length; j++)
					{
						rootStream.streamPages[i][j] = reader.ReadInt32();
						Debug.WriteLine(String.Format("\t\tPage {0} (at offset {1})", rootStream.streamPages[i][j], rootStream.streamPages[i][j] * pageSize));
					}
				}
			}

			return true;
		}
	}
}
