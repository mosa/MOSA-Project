/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Mosa.Runtime.Metadata.Tables 
{
	public struct GenericParamConstraintRow {
		#region Data members

        private TokenTypes _ownerTableIdx;

        private TokenTypes _constraintTableIdx;

		#endregion // Data members

        #region Construction

        public GenericParamConstraintRow(TokenTypes ownerTableIdx, TokenTypes constraintTableIdx)
        {
            _ownerTableIdx = ownerTableIdx;
            _constraintTableIdx = constraintTableIdx;
        }

        #endregion // Construction

        #region Properties

        public TokenTypes OwnerTableIdx
        {
            get { return _ownerTableIdx; }
        }

        public TokenTypes ConstraintTableIdx
        {
            get { return _constraintTableIdx; }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
		/// Loads a <see cref="NetOS.Runtime.Metadata.GenericParamConstraintRow"/> from the given table heap.
		/// </summary>
        /// <param name="provider">The <see cref="NetOS.Runtime.Metadata.IMetadataProvider"/>, which contains the row.</param>
        /// <param name="reader">The reader to read the row from.</param>
		/// <param name="table">The table token type to read from.</param>
        public void Load(IMetadataProvider provider, BinaryReader reader, TokenTypes table)
		{
			if (table != TokenTypes.GenericParamConstraint)
				throw new ArgumentException("Invalid token type.", @"table");

		}

		#endregion // Methods
	}
}
