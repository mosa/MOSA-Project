// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Mosa.Compiler.Pdb
{
	/// <summary>
	/// Reads Microsoft program database files created by the .NET compilers.
	/// </summary>
	public class PdbReader : IDisposable
	{
		#region Data Members

		/// <summary>
		/// Holds the header of the PDB file.
		/// </summary>
		private PdbFileHeader header;

		/// <summary>
		/// The binary reader used to read the PDB file.
		/// </summary>
		private BinaryReader reader;

		/// <summary>
		/// The root stream.
		/// </summary>
		private PdbRootStream root;

		/// <summary>
		/// Holds the stream to read.
		/// </summary>
		private Stream stream;

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="PdbReader"/> class.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="stream"/> is null.</exception>
		/// <exception cref="System.ArgumentException"><paramref name="stream"/> must be readable and seekable.</exception>
		public PdbReader(Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException(@"stream");
			if (stream.CanRead == false)
				throw new ArgumentException(@"Stream is not readable", @"stream");
			if (stream.CanSeek == false)
				throw new ArgumentException(@"Stream must be seekable.", @"stream");

			this.stream = stream;
			this.reader = new BinaryReader(stream);

			// Read the file header
			if (PdbFileHeader.Read(this.reader, out this.header) == false)
				throw new InvalidDataException(@"Not a Microsoft program database v7.0 file.");

			LoadRootStream();
		}

		/// <summary>
		/// Loads the root stream.
		/// </summary>
		private void LoadRootStream()
		{
			// The root stream length
			int dwStreamLength = this.header.dwRootBytes;

			// Calculate the number of pages of the root stream
			int pageCount = (dwStreamLength / this.header.dwPageSize) + 1;

			// Allocate page list
			int[] pages = new int[pageCount];

			// Read the pages
			this.stream.Position = this.header.dwIndexPage * this.header.dwPageSize;
			for (int i = 0; i < pageCount; i++)
			{
				pages[i] = this.reader.ReadInt32();
				Debug.WriteLine(String.Format(@"PdbReader: Root stream page {0} (at offset {1})", pages[i], pages[i] * this.header.dwPageSize));
			}

			using (PdbStream pdbStream = GetStream(pages, dwStreamLength))
			using (BinaryReader rootReader = new BinaryReader(pdbStream))
			{
				PdbRootStream.Read(rootReader, this.header.dwPageSize, out this.root);
			}
		}

		/// <summary>
		/// Gets the stream.
		/// </summary>
		/// <param name="pdbStream">The PDB stream.</param>
		/// <returns></returns>
		public PdbStream GetStream(int pdbStream)
		{
			if (pdbStream > this.root.streamLength.Length || this.root.streamPages[pdbStream] == null)
				throw new ArgumentException(@"Invalid pdb stream index.", @"pdbStream");

			return GetStream(this.root.streamPages[pdbStream], this.root.streamLength[pdbStream]);
		}

		private PdbStream GetStream(int[] pages, int dwStreamLength)
		{
			return new PdbStream(this.stream, this.header.dwPageSize, pages, dwStreamLength);
		}

		#endregion Construction

		#region IDisposable Members

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			if (this.reader != null)
			{
				this.reader.Close();
				this.reader = null;
				this.stream = null;
			}
		}

		#endregion IDisposable Members

		#region Type Stream

		/// <summary>
		/// Gets the types described in this PDB file.
		/// </summary>
		/// <value>The types.</value>
		public IEnumerable<PdbType> Types
		{
			get
			{
				using (BinaryReader reader = new BinaryReader(GetStream(3)))
				{
					// Read the symbol header
					PdbSymbolHeader header;
					if (PdbSymbolHeader.Read(reader, out header) == true)
					{
						// Now iterate all symbol file headers, which are added per source file.
						// These map symbols to the files they're contained in via special PDB streams, which in
						// turn contain the line information.
						long fileEnd = reader.BaseStream.Position + header.module_size;
						while (reader.BaseStream.Position < fileEnd)
						{
							yield return new PdbReadType(this, reader);
						}

						yield break;
					}
				}

				throw new InvalidDataException(@"Invalid debug information.");
			}
		}

		/// <summary>
		/// Gets the global symbols.
		/// </summary>
		/// <value>The global symbols.</value>
		public IEnumerable<CvSymbol> GlobalSymbols
		{
			get
			{
				using (BinaryReader reader = new BinaryReader(GetStream(3)))
				{
					// Read the symbol header
					PdbSymbolHeader header;
					if (PdbSymbolHeader.Read(reader, out header) == true)
					{
						// Read the global symbol table
						return new CvGlobalSymbolEnumerator(GetStream(header.gsym_stream));
					}
				}

				throw new InvalidDataException(@"Invalid debug information.");
			}
		}

		#endregion Type Stream
	}
}
