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
	public class TypeSpecRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeSpecRow" /> struct.
		/// </summary>
		/// <param name="signatureBlob">The signature BLOB.</param>
		public TypeSpecRow(HeapIndexToken signatureBlob)
		{
			SignatureBlob = signatureBlob;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the signature BLOB.
		/// </summary>
		/// <value>
		/// The signature BLOB.
		/// </value>
		public HeapIndexToken SignatureBlob { get; private set; }

		#endregion Properties
	}
}