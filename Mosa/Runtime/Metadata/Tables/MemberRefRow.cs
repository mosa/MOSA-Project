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
	public struct MemberRefRow 
    {
		#region Data members

        private TokenTypes _classTableIdx;

        private TokenTypes _nameStringIdx;

        private TokenTypes _signatureBlobIdx;

		#endregion // Data members

        #region Construction

        public MemberRefRow(TokenTypes classTableIdx, TokenTypes nameStringIdx, TokenTypes signatureBlobIdx)
        {
            _classTableIdx = classTableIdx;
            _nameStringIdx = nameStringIdx;
            _signatureBlobIdx = signatureBlobIdx;
        }

        #endregion // Construction

        #region Properties

        public TokenTypes ClassTableIdx
        {
            get { return _classTableIdx; }
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
