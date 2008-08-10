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
	public struct PropertyRow
    {
		#region Data members

        private PropertyAttributes _flags;

        private TokenTypes _nameStringIdx;

        private TokenTypes _typeBlobIdx;

        #endregion // Data members

        #region Construction

        public PropertyRow(PropertyAttributes flags, TokenTypes nameStringIdx, TokenTypes typeBlobIdx)
        {
            _flags = flags;
            _nameStringIdx = nameStringIdx;
            _typeBlobIdx = typeBlobIdx;
        }

        #endregion // Construction

        #region Properties

        public PropertyAttributes Flags
        {
            get { return _flags; }
        }

        public TokenTypes NameStringIdx
        {
            get { return _nameStringIdx; }
        }

        public TokenTypes TypeBlobIdx
        {
            get { return _typeBlobIdx; }
        }

        #endregion // Properties
	}
}
