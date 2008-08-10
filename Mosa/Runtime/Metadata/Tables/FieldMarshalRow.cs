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
	public struct FieldMarshalRow {
		#region Data members

        private TokenTypes _parentTableIdx;

        private TokenTypes _nativeTypeBlobIdx;

		#endregion // Data members

        #region Construction

        public FieldMarshalRow(TokenTypes parentTableIdx, TokenTypes nativeTypeBlobIdx)
        {
            _parentTableIdx = parentTableIdx;
            _nativeTypeBlobIdx = nativeTypeBlobIdx;
        }

        #endregion // Construction

        #region Properties

        public TokenTypes ParentTableIdx
        {
            get { return _parentTableIdx; }
        }
        
        public TokenTypes NativeTypeBlobIdx
        {
            get { return _nativeTypeBlobIdx; }
        }

        #endregion // Properties
	}
}
