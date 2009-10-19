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
	public struct PropertyMapRow {
		#region Data members

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _parentTableIdx;

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _propertyTableIdx;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMapRow"/> struct.
        /// </summary>
        /// <param name="parentTableIdx">The parent table idx.</param>
        /// <param name="propertyTableIdx">The property table idx.</param>
        public PropertyMapRow(TokenTypes parentTableIdx, TokenTypes propertyTableIdx)
        {
            _parentTableIdx = parentTableIdx;
            _propertyTableIdx = propertyTableIdx;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the parent table idx.
        /// </summary>
        /// <value>The parent table idx.</value>
        public TokenTypes ParentTableIdx
        {
            get { return _parentTableIdx; }
        }

        /// <summary>
        /// Gets the property table idx.
        /// </summary>
        /// <value>The property table idx.</value>
        public TokenTypes PropertyTableIdx
        {
            get { return _propertyTableIdx; }
        }

        #endregion // Properties
	}
}
