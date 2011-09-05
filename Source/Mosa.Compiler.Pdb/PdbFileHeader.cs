/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Mosa.Compiler.Pdb
{
	/// <summary>
	/// Represents the header of PDB files.
	/// </summary>
	public struct PdbFileHeader
	{
		#region Constants

		/// <summary>
		/// Specifies the length of the PDB signature in the file header.
		/// </summary>
		private const int PdbSignatureLength = 0x20;

		/// <summary>
		/// The PDB signature of Microsoft C/C++ 7.0 PDB files.
		/// </summary>
		private static readonly byte[] PdbSignature = new byte[0x20] {
			0x4D, 0x69, 0x63, 0x72, 0x6F, 0x73, 0x6F, 0x66,
			0x74, 0x20, 0x43, 0x2F, 0x43, 0x2B, 0x2B, 0x20,
			0x4D, 0x53, 0x46, 0x20, 0x37, 0x2E, 0x30, 0x30,
			0x0D, 0x0A, 0x1A, 0x44, 0x53, 0x00, 0x00, 0x00
		};

		#endregion // Constants

		#region Data Members

		/// <summary>
		/// Holds the signature of the PDB file.
		/// </summary>
		public byte[] signature;

		/// <summary>
		/// Size of a PDB page in bytes.
		/// </summary>
		public int dwPageSize;

		/// <summary>
		/// The page, which holds the PDB bitmap.
		/// </summary>
		public int dwBitmapPage;

		/// <summary>
		/// Number of pages in the file.
		/// </summary>
		public int dwFilePages;

		/// <summary>
		/// The number of bytes in the root stream.
		/// </summary>
		public int dwRootBytes;

		/// <summary>
		/// Always zero.
		/// </summary>
		public int dwReserved;

		/// <summary>
		/// Holds the index page.
		/// </summary>
		public int dwIndexPage;

		#endregion // Data Members

		#region Methods

		/// <summary>
		/// Reads the PDB file header from the given reader.
		/// </summary>
		/// <param name="reader">The reader to read From.</param>
		/// <param name="header">The header to populate.</param>
		/// <returns><c>true</c> if reading was successful, otherwise <c>false</c>.</returns>
		public static bool Read(BinaryReader reader, out PdbFileHeader header)
		{
			if (reader == null)
				throw new ArgumentNullException(@"reader");

			// Read & compare the signature
			header.signature = reader.ReadBytes(0x20);
			if (ArrayCompare(header.signature, PdbFileHeader.PdbSignature) == true)
			{
				header.dwPageSize = reader.ReadInt32();
				header.dwBitmapPage = reader.ReadInt32();
				header.dwFilePages = reader.ReadInt32();
				header.dwRootBytes = reader.ReadInt32();
				header.dwReserved = reader.ReadInt32();
				header.dwIndexPage = reader.ReadInt32();

				string sig = Encoding.ASCII.GetString(header.signature);
				sig = sig.Substring(0, sig.IndexOf('\r'));
				Debug.WriteLine(String.Format("PdbFileHeader:\n\tSignature={0}\n\tPageSize={1}\n\tBitmapPage={2} (at offset {3})\n\tFilePages={4}\n\tRootBytes={5}\n\tReserved={6}\n\tIndexPage={7} (at offset {8})", sig, header.dwPageSize, header.dwBitmapPage, header.dwBitmapPage * header.dwPageSize, header.dwFilePages, header.dwRootBytes, header.dwReserved, header.dwIndexPage, header.dwIndexPage * header.dwPageSize));

				// Some more sanity checks at the end
				if (reader.BaseStream.Length == (header.dwPageSize * header.dwFilePages))
					return true;
			}

			// Clear what we've read
			header = new PdbFileHeader();
			return false;
		}

		/// <summary>
		/// Compares two arrays.
		/// </summary>
		/// <param name="first">The first.</param>
		/// <param name="second">The second.</param>
		/// <returns></returns>
		private static bool ArrayCompare(byte[] first, byte[] second)
		{
			for (int i = 0; i < Math.Min(first.Length, second.Length); i++)
			{
				if (first[i] != second[i])
				{
					return false;
				}
			}

			return true;
		}

		#endregion // Methods
	}
}
