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
	public struct NestedClassRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private TokenTypes _nestedClassTableIdx;

		/// <summary>
		/// 
		/// </summary>
		private TokenTypes _enclosingClassTableIdx;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="NestedClassRow"/> struct.
		/// </summary>
		/// <param name="nestedClassTableIdx">The nested class table idx.</param>
		/// <param name="enclosingClassTableIdx">The enclosing class table idx.</param>
		public NestedClassRow(TokenTypes nestedClassTableIdx, TokenTypes enclosingClassTableIdx)
		{
			_nestedClassTableIdx = nestedClassTableIdx;
			_enclosingClassTableIdx = enclosingClassTableIdx;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the nested class table idx.
		/// </summary>
		/// <value>The nested class table idx.</value>
		public TokenTypes NestedClassTableIdx
		{
			get { return _nestedClassTableIdx; }
		}

		/// <summary>
		/// Gets the enclosing class table idx.
		/// </summary>
		/// <value>The enclosing class table idx.</value>
		public TokenTypes EnclosingClassTableIdx
		{
			get { return _enclosingClassTableIdx; }
		}

		#endregion // Properties
	}
}
