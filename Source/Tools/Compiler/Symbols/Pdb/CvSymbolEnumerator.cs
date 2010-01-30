using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Mosa.Tools.Compiler.Symbols.Pdb
{
    /// <summary>
    /// Enumerator for CodeView symbols in a PDB file.
    /// </summary>
    abstract class CvSymbolEnumerator : IEnumerable<CvSymbol>
    {
        #region Data Members

		/// <summary>
		/// Holds the pdb stream, that contains the symbol information.
		/// </summary>
        private PdbStream stream;

        #endregion // Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CvSymbolEnumerator"/> class.
		/// </summary>
		/// <param name="stream">The stream holding the pdb symbols.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="stream"/> is null.</exception>
		public CvSymbolEnumerator(PdbStream stream)
		{
			if (stream == null)
				throw new ArgumentNullException(@"stream");

			this.stream = stream;
		}

		#endregion // Construction

        #region Methods

        protected abstract bool IsComplete(object state);

        protected abstract object Prepare(BinaryReader reader);

        #endregion // Methods

        #region IEnumerable<CvSymbol> Members

        public IEnumerator<CvSymbol> GetEnumerator()
		{
			CvStream cvStream = new CvStream(this.stream);
			using (BinaryReader reader = new BinaryReader(cvStream))
			{
                object state = Prepare(reader);

                do
                {
                    // Read the len+id of the symbol
					long startPos = cvStream.Position;
					CvSymbol symbol = CvSymbol.Read(reader);
					yield return symbol;

                    // Skip to the next 4 byte boundary
                    CvUtil.PadToBoundary(reader, 4);

					long nextPos = startPos + symbol.Length + 2;
					if (nextPos < cvStream.Length)
                    {
                        // Move to the next symbol
						cvStream.Seek(nextPos, SeekOrigin.Begin);
                    }
                    else
                    {
                        break;
                    }
                }
				while (IsComplete(state) == false);
			}
		}

		#endregion // IEnumerable<CvSymbol> Members

		#region IEnumerable Members

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion // IEnumerable Members
	}
}
