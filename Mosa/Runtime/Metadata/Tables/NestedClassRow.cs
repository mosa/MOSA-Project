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
	public struct NestedClassRow 
    {
		#region Data members

        private TokenTypes _nestedClassTableIdx;

        private TokenTypes _enclosingClassTableIdx;

		#endregion // Data members

        #region Construction

        public NestedClassRow(TokenTypes nestedClassTableIdx, TokenTypes enclosingClassTableIdx)
        {
            _nestedClassTableIdx = nestedClassTableIdx;
            _enclosingClassTableIdx = enclosingClassTableIdx;
        }

        #endregion // Construction

        #region Properties

        public TokenTypes NestedClassTableIdx
        {
            get { return _nestedClassTableIdx; }
        }

        public TokenTypes EnclosingClassTableIdx
        {
            get { return _enclosingClassTableIdx; }
        }

        #endregion // Properties
	}
}
