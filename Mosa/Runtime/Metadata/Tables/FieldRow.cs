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
	public struct FieldRow {
		#region Data members

        private FieldAttributes _flags;

        private TokenTypes _nameStringIdx;

        private TokenTypes _signatureBlobIdx;

		#endregion // Data members

        #region Construction

        public FieldRow(FieldAttributes flags, TokenTypes nameStringIdx, TokenTypes signatureBlobIdx)
        {
            _flags = flags;
            _nameStringIdx = nameStringIdx;
            _signatureBlobIdx = signatureBlobIdx;
        }

        #endregion // Construction

        #region Properties

        public FieldAttributes Flags
        {
            get { return _flags; }
        }

        public TokenTypes NameStringIdx
        {
            get { return _nameStringIdx; }
        }

        public TokenTypes SignatureBlobIdx
        {
            get { return _signatureBlobIdx; }
        }

        #endregion // Properties
	}
}
