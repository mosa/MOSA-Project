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
	public struct CustomAttributeRow 
    {
		#region Data members

        private TokenTypes _parentTableIdx;

        private TokenTypes _typeIdx;

        private TokenTypes _valueBlobIdx;

		#endregion // Data members

        #region Construction

        public CustomAttributeRow(TokenTypes parentTableIdx, TokenTypes typeIdx, TokenTypes valueBlobIdx)
        {
            _parentTableIdx = parentTableIdx;
            _typeIdx = typeIdx;
            _valueBlobIdx = valueBlobIdx;
        }

        #endregion // Construction

        #region Properties

        public TokenTypes ParentTableIdx
        {
            get { return _parentTableIdx; }
        }

        public TokenTypes TypeIdx
        {
            get { return _typeIdx; }
        }

        public TokenTypes ValueBlobIdx
        {
            get { return _valueBlobIdx; }
        }

        #endregion // Properties
	}
}
