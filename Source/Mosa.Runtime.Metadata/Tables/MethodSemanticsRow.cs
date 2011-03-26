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

using Mosa.Runtime.Metadata;

namespace Mosa.Runtime.Metadata.Tables
{
	/// <summary>
	/// 
	/// </summary>
	public struct MethodSemanticsRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private MethodSemanticsAttributes _semantics;

		/// <summary>
		/// 
		/// </summary>
		private TokenTypes _methodTableIdx;

		/// <summary>
		/// 
		/// </summary>
		private TokenTypes _associationTableIdx;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="MethodSemanticsRow"/> struct.
		/// </summary>
		/// <param name="semantics">The semantics.</param>
		/// <param name="methodTableIdx">The method table idx.</param>
		/// <param name="associationTableIdx">The association table idx.</param>
		public MethodSemanticsRow(MethodSemanticsAttributes semantics, TokenTypes methodTableIdx,
									TokenTypes associationTableIdx)
		{
			_semantics = semantics;
			_methodTableIdx = methodTableIdx;
			_associationTableIdx = associationTableIdx;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the semantics.
		/// </summary>
		/// <value>The semantics.</value>
		public MethodSemanticsAttributes Semantics
		{
			get { return _semantics; }
		}

		/// <summary>
		/// Gets the method table idx.
		/// </summary>
		/// <value>The method table idx.</value>
		public TokenTypes MethodTableIdx
		{
			get { return _methodTableIdx; }
		}

		/// <summary>
		/// Gets the association table idx.
		/// </summary>
		/// <value>The association table idx.</value>
		public TokenTypes AssociationTableIdx
		{
			get { return _associationTableIdx; }
		}

		#endregion // Properties
	}
}
