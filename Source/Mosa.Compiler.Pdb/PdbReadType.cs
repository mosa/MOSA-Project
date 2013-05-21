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

namespace Mosa.Compiler.Pdb
{
	/// <summary>
	/// A read-only PDB type.
	/// </summary>
	internal class PdbReadType : PdbType
	{
		#region Data Members

		/// <summary>
		/// Holds the PDB reader to access streams.
		/// </summary>
		private PdbReader pdbReader;

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="PdbReadType"/> class.
		/// </summary>
		/// <param name="pdbReader">The PDB reader.</param>
		/// <param name="reader">The reader.</param>
		public PdbReadType(PdbReader pdbReader, BinaryReader reader) :
			base(reader)
		{
			if (pdbReader == null)
				throw new ArgumentNullException(@"pdbReader");

			this.pdbReader = pdbReader;
		}

		#endregion Construction

		#region PdbType Overrides

		public override IEnumerable<CvLine> LineNumbers
		{
			get
			{
				return new CvLineEnumerator(this.pdbReader, this.Stream, this.SymbolSize);
			}
		}

		/// <summary>
		/// Gets the symbols.
		/// </summary>
		/// <value>The symbols.</value>
		public override IEnumerable<CvSymbol> Symbols
		{
			get
			{
				if (this.SymbolSize == 0)
					return new CvSymbol[0];

				return new CvTypeSymbolEnumerator(this.pdbReader.GetStream(this.Stream), this.SymbolSize);
			}
		}

		#endregion PdbType Overrides

		#region Types

		private class CvTypeSymbolEnumerator : CvSymbolEnumerator
		{
			private int size;

			public CvTypeSymbolEnumerator(PdbStream stream, int size) :
				base(stream)
			{
				this.size = size;
			}

			protected override bool IsComplete(object state)
			{
				BinaryReader reader = (BinaryReader)state;
				return (reader.BaseStream.Position >= this.size);
			}

			protected override object Prepare(BinaryReader reader)
			{
				int version = reader.ReadInt32();
				if (version != 4)
					throw new InvalidDataException(@"PDB symbol table has unsupported header.");

				return reader;
			}
		}

		#endregion Types
	}
}