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
	public struct CustomAttributeRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private Token parent;

		/// <summary>
		/// 
		/// </summary>
		private Token type;

		/// <summary>
		/// 
		/// </summary>
		private TokenTypes valueBlobIdx;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CustomAttributeRow"/> struct.
		/// </summary>
		/// <param name="parent">The parent table idx.</param>
		/// <param name="type">The type idx.</param>
		/// <param name="valueBlobIdx">The value BLOB idx.</param>
		public CustomAttributeRow(Token parent, Token type, TokenTypes valueBlobIdx)
		{
			this.parent = parent;
			this.type = type;
			this.valueBlobIdx = valueBlobIdx;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the parent table idx.
		/// </summary>
		/// <value>The parent table idx.</value>
		public Token Parent
		{
			get { return parent; }
		}

		/// <summary>
		/// Gets the type idx.
		/// </summary>
		/// <value>The type idx.</value>
		public Token Type
		{
			get { return type; }
		}

		/// <summary>
		/// Gets the value BLOB idx.
		/// </summary>
		/// <value>The value BLOB idx.</value>
		public TokenTypes ValueBlobIdx
		{
			get { return valueBlobIdx; }
		}

		#endregion // Properties
	}
}
