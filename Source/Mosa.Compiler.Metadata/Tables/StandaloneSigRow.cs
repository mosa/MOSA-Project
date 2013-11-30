/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Metadata.Tables
{
	/// <summary>
	///
	/// </summary>
	public class StandAloneSigRow
	{
		#region Properties

		/// <summary>
		/// Gets the signature BLOB.
		/// </summary>
		/// <value>
		/// The signature BLOB.
		/// </value>
		public HeapIndexToken SignatureBlob { get; private set; }

		#endregion Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="StandAloneSigRow" /> struct.
		/// </summary>
		/// <param name="signatureBlob">The signature BLOB.</param>
		public StandAloneSigRow(HeapIndexToken signatureBlob)
		{
			SignatureBlob = signatureBlob;
		}

		#endregion Construction
	}
}