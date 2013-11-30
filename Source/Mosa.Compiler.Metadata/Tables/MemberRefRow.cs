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
	public class MemberRefRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="MemberRefRow" /> struct.
		/// </summary>
		/// <param name="class">The class.</param>
		/// <param name="nameString">The name string.</param>
		/// <param name="signatureBlob">The signature BLOB.</param>
		public MemberRefRow(Token @class, HeapIndexToken nameString, HeapIndexToken signatureBlob)
		{
			Class = @class;
			NameString = nameString;
			SignatureBlob = signatureBlob;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the class.
		/// </summary>
		/// <value>
		/// The class.
		/// </value>
		public Token Class { get; private set; }

		/// <summary>
		/// Gets the name string.
		/// </summary>
		/// <value>
		/// The name string.
		/// </value>
		public HeapIndexToken NameString { get; private set; }

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