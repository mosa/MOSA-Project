/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Mosa.Runtime.Metadata.Tables 
{
    /// <summary>
    /// 
    /// </summary>
	public struct EventRow {
		#region Data members

        /// <summary>
        /// 
        /// </summary>
        private EventAttributes _flags;

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _nameStringIdx;

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _eventTypeTableIdx;

		#endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="EventRow"/> struct.
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <param name="nameStringIdx">The name string idx.</param>
        /// <param name="eventTypeTableIdx">The event type table idx.</param>
        public EventRow(EventAttributes flags, TokenTypes nameStringIdx, TokenTypes eventTypeTableIdx)
        {
            _flags = flags;
            _nameStringIdx = nameStringIdx;
            _eventTypeTableIdx = eventTypeTableIdx;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the flags.
        /// </summary>
        /// <value>The flags.</value>
        public EventAttributes Flags
        {
            get { return _flags; }
        }

        /// <summary>
        /// Gets the name string idx.
        /// </summary>
        /// <value>The name string idx.</value>
        public TokenTypes NameStringIdx
        {
            get { return _nameStringIdx; }
        }

        /// <summary>
        /// Gets the event type table idx.
        /// </summary>
        /// <value>The event type table idx.</value>
        public TokenTypes EventTypeTableIdx
        {
            get { return _eventTypeTableIdx; }
        }

        #endregion // Properties
	}
}
