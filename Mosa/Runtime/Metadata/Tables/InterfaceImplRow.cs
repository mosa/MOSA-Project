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
	public struct InterfaceImplRow 
    {
		#region Data members

        private TokenTypes _classTableIdx;

        private TokenTypes _interfaceTableIdx;

		#endregion // Data members

        #region Construction

        public InterfaceImplRow(TokenTypes classTableIdx, TokenTypes interfaceTableIdx)
        {
            _classTableIdx = classTableIdx;
            _interfaceTableIdx = interfaceTableIdx;
        }

        #endregion // Construction

        #region Properties

        public TokenTypes ClassTableIdx
        {
            get { return _classTableIdx; }
        }

        public TokenTypes InterfaceTableIdx
        {
            get { return _interfaceTableIdx; }
        }

        #endregion // Properties
	}
}
