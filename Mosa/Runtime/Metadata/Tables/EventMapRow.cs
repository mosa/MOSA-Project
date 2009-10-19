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
	public struct EventMapRow {
		#region Data members

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _typeDefTableIdx;

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _eventListTableIdx;

		#endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="EventMapRow"/> struct.
        /// </summary>
        /// <param name="typeDefTableIdx">The type def table idx.</param>
        /// <param name="eventListTableIdx">The event list table idx.</param>
        public EventMapRow(TokenTypes typeDefTableIdx, TokenTypes eventListTableIdx)
        {
            _typeDefTableIdx = typeDefTableIdx;
            _eventListTableIdx = eventListTableIdx;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the type def table idx.
        /// </summary>
        /// <value>The type def table idx.</value>
        public TokenTypes TypeDefTableIdx
        {
            get { return _typeDefTableIdx; }
        }

        /// <summary>
        /// Gets the event list table idx.
        /// </summary>
        /// <value>The event list table idx.</value>
        public TokenTypes EventListTableIdx
        {
            get { return _eventListTableIdx; }
        }

        #endregion // Properties
	}
}
