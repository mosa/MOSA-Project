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
	public struct PropertyMapRow {
		#region Data members

        private TokenTypes _parentTableIdx;

        private TokenTypes _propertyTableIdx;

        #endregion // Data members

        #region Construction

        public PropertyMapRow(TokenTypes parentTableIdx, TokenTypes propertyTableIdx)
        {
            _parentTableIdx = parentTableIdx;
            _propertyTableIdx = propertyTableIdx;
        }

        #endregion // Construction

        #region Properties

        public TokenTypes ParentTableIdx
        {
            get { return _parentTableIdx; }
        }

        public TokenTypes PropertyTableIdx
        {
            get { return _propertyTableIdx; }
        }

        #endregion // Properties
	}
}
