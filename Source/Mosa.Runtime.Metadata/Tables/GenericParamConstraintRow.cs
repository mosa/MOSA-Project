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
	public struct GenericParamConstraintRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private TokenTypes _ownerTableIdx;

		/// <summary>
		/// 
		/// </summary>
		private TokenTypes _constraintTableIdx;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericParamConstraintRow"/> struct.
		/// </summary>
		/// <param name="ownerTableIdx">The owner table idx.</param>
		/// <param name="constraintTableIdx">The constraint table idx.</param>
		public GenericParamConstraintRow(TokenTypes ownerTableIdx, TokenTypes constraintTableIdx)
		{
			_ownerTableIdx = ownerTableIdx;
			_constraintTableIdx = constraintTableIdx;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the owner table idx.
		/// </summary>
		/// <value>The owner table idx.</value>
		public TokenTypes OwnerTableIdx
		{
			get { return _ownerTableIdx; }
		}

		/// <summary>
		/// Gets the constraint table idx.
		/// </summary>
		/// <value>The constraint table idx.</value>
		public TokenTypes ConstraintTableIdx
		{
			get { return _constraintTableIdx; }
		}

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Loads a <see cref="Mosa.Runtime.Metadata.TokenTypes.GenericParamConstraint"/> from the given table heap.
		/// </summary>
		/// <param name="provider">The <see cref="Mosa.Runtime.Metadata.IMetadataProvider"/>, which contains the row.</param>
		/// <param name="reader">The reader to read the row From.</param>
		/// <param name="table">The table token type to read From.</param>
		public void Load(IMetadataProvider provider, BinaryReader reader, TokenTypes table)
		{
			if (table != TokenTypes.GenericParamConstraint)
				throw new ArgumentException("Invalid token type.", @"table");

		}

		#endregion // Methods
	}
}
