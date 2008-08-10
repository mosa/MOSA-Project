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
	public struct EventRow {
		#region Data members

        private EventAttributes _flags;

        private TokenTypes _nameStringIdx;

        private TokenTypes _eventTypeTableIdx;

		#endregion // Data members

        #region Construction

        public EventRow(EventAttributes flags, TokenTypes nameStringIdx, TokenTypes eventTypeTableIdx)
        {
            _flags = flags;
            _nameStringIdx = nameStringIdx;
            _eventTypeTableIdx = eventTypeTableIdx;
        }

        #endregion // Construction

        #region Properties

        public EventAttributes Flags
        {
            get { return _flags; }
        }

        public TokenTypes NameStringIdx
        {
            get { return _nameStringIdx; }
        }

        public TokenTypes EventTypeTableIdx
        {
            get { return _eventTypeTableIdx; }
        }

        #endregion // Properties
	}
}
