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
using System.Text;
using System.IO;

namespace Mosa.Runtime.Metadata.Tables
{
	/// <summary>
	/// 
	/// </summary>
	public struct PropertyRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private PropertyAttributes _flags;

		/// <summary>
		/// 
		/// </summary>
		private TokenTypes _nameStringIdx;

		/// <summary>
		/// 
		/// </summary>
		private TokenTypes _typeBlobIdx;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="PropertyRow"/> struct.
		/// </summary>
		/// <param name="flags">The flags.</param>
		/// <param name="nameStringIdx">The name string idx.</param>
		/// <param name="typeBlobIdx">The type BLOB idx.</param>
		public PropertyRow(PropertyAttributes flags, TokenTypes nameStringIdx, TokenTypes typeBlobIdx)
		{
			_flags = flags;
			_nameStringIdx = nameStringIdx;
			_typeBlobIdx = typeBlobIdx;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public PropertyAttributes Flags
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
		/// Gets the type BLOB idx.
		/// </summary>
		/// <value>The type BLOB idx.</value>
		public TokenTypes TypeBlobIdx
		{
			get { return _typeBlobIdx; }
		}

		#endregion // Properties
	}
}
