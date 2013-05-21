/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Compiler.Metadata.Tables
{
	/// <summary>
	///
	/// </summary>
	public struct StandAloneSigRow
	{
		#region Data members

		/// <summary>
		///
		/// </summary>
		private HeapIndexToken _signatureBlobIdx;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="StandAloneSigRow"/> struct.
		/// </summary>
		/// <param name="signatureBlobIdx">The signature BLOB idx.</param>
		public StandAloneSigRow(HeapIndexToken signatureBlobIdx)
		{
			_signatureBlobIdx = signatureBlobIdx;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the signature BLOB idx.
		/// </summary>
		/// <value>The signature BLOB idx.</value>
		public HeapIndexToken SignatureBlobIdx
		{
			get { return _signatureBlobIdx; }
		}

		#endregion Properties
	}
}