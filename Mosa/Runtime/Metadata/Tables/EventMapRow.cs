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
	public struct EventMapRow {
		#region Data members

        private TokenTypes _typeDefTableIdx;

        private TokenTypes _eventListTableIdx;

		#endregion // Data members

        #region Construction

        public EventMapRow(TokenTypes typeDefTableIdx, TokenTypes eventListTableIdx)
        {
            _typeDefTableIdx = typeDefTableIdx;
            _eventListTableIdx = eventListTableIdx;
        }

        #endregion // Construction

        #region Properties

        public TokenTypes TypeDefTableIdx
        {
            get { return _typeDefTableIdx; }
        }

        public TokenTypes EventListTableIdx
        {
            get { return _eventListTableIdx; }
        }

        #endregion // Properties
	}
}
