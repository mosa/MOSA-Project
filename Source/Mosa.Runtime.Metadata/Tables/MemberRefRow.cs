/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */


namespace Mosa.Runtime.Metadata.Tables
{
	/// <summary>
	/// 
	/// </summary>
	public struct MemberRefRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private Token @class;

		/// <summary>
		/// 
		/// </summary>
		private HeapIndexToken nameStringIdx;

		/// <summary>
		/// 
		/// </summary>
		private HeapIndexToken signatureBlobIdx;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="MemberRefRow"/> struct.
		/// </summary>
		/// <param name="classTableIdx">The class table idx.</param>
		/// <param name="nameStringIdx">The name string idx.</param>
		/// <param name="signatureBlobIdx">The signature BLOB idx.</param>
		public MemberRefRow(Token @class, HeapIndexToken nameStringIdx, HeapIndexToken signatureBlobIdx)
		{
			this.@class = @class;
			this.nameStringIdx = nameStringIdx;
			this.signatureBlobIdx = signatureBlobIdx;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the class table idx.
		/// </summary>
		/// <value>The class table idx.</value>
		public Token Class
		{
			get { return @class; }
		}

		/// <summary>
		/// Gets the name string idx.
		/// </summary>
		/// <value>The name string idx.</value>
		public HeapIndexToken NameStringIdx
		{
			get { return nameStringIdx; }
		}

		/// <summary>
		/// Gets the signature BLOB idx.
		/// </summary>
		/// <value>The signature BLOB idx.</value>
		public HeapIndexToken SignatureBlobIdx
		{
			get { return signatureBlobIdx; }
		}

		#endregion // Properties
	}
}
