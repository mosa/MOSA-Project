/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mono.Cecil;

namespace Mosa.Runtime.Metadata.Tables
{
	/// <summary>
	/// 
	/// </summary>
	public struct FieldRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private readonly FieldAttributes _flags;

		/// <summary>
		/// 
		/// </summary>
		private readonly TokenTypes _nameStringIdx;

		/// <summary>
		/// 
		/// </summary>
		private readonly TokenTypes _signatureBlobIdx;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldRow"/> struct.
		/// </summary>
		/// <param name="flags">The flags.</param>
		/// <param name="nameStringIdx">The name string idx.</param>
		/// <param name="signatureBlobIdx">The signature BLOB idx.</param>
		public FieldRow(FieldAttributes flags, TokenTypes nameStringIdx, TokenTypes signatureBlobIdx)
		{
			_flags = flags;
			_nameStringIdx = nameStringIdx;
			_signatureBlobIdx = signatureBlobIdx;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public FieldAttributes Flags
		{
			get { return _flags; }
		}

		/// <summary>
		/// Gets the name string idx.
		/// </summary>
		/// <value>The name string idx.</value>
		public TokenTypes NameStringIdx
		{
			get { return _nameStringIdx; }
		}

		/// <summary>
		/// Gets the signature BLOB idx.
		/// </summary>
		/// <value>The signature BLOB idx.</value>
		public TokenTypes SignatureBlobIdx
		{
			get { return _signatureBlobIdx; }
		}

		#endregion // Properties
	}
}
