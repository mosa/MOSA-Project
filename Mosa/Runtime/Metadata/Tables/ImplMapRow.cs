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
	public struct ImplMapRow {
		#region Data members

        private PInvokeAttributes _mappingFlags;

        private TokenTypes _memberForwardedTableIdx;

        private TokenTypes _importNameStringIdx;

        private TokenTypes _importScopeTableIdx;

		#endregion // Data members

        #region Construction

        public ImplMapRow(PInvokeAttributes mappingFlags, TokenTypes memberForwardedTableIdx, 
            TokenTypes importNameStringIdx, TokenTypes importScopeTableIdx)
        {
            _mappingFlags = mappingFlags;
            _memberForwardedTableIdx = memberForwardedTableIdx;
            _importNameStringIdx = importNameStringIdx;
            _importScopeTableIdx = importScopeTableIdx;
        }

        #endregion // Construction

        #region Properties

        public PInvokeAttributes MappingFlags
        {
            get { return _mappingFlags; }
        }

        public TokenTypes MemberForwardedTableIdx
        {
            get { return _memberForwardedTableIdx; }
        }

        public TokenTypes ImportNameStringIdx
        {
            get { return _importNameStringIdx; }
        }

        public TokenTypes ImportScopeTableIdx
        {
            get { return _importScopeTableIdx; }
        }

        #endregion // Properties
	}
}
