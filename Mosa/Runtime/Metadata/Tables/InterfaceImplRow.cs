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
    /// <summary>
    /// 
    /// </summary>
	public struct InterfaceImplRow 
    {
		#region Data members

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _classTableIdx;

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _interfaceTableIdx;

		#endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="InterfaceImplRow"/> struct.
        /// </summary>
        /// <param name="classTableIdx">The class table idx.</param>
        /// <param name="interfaceTableIdx">The interface table idx.</param>
        public InterfaceImplRow(TokenTypes classTableIdx, TokenTypes interfaceTableIdx)
        {
            _classTableIdx = classTableIdx;
            _interfaceTableIdx = interfaceTableIdx;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the class table idx.
        /// </summary>
        /// <value>The class table idx.</value>
        public TokenTypes ClassTableIdx
        {
            get { return _classTableIdx; }
        }

        /// <summary>
        /// Gets the interface table idx.
        /// </summary>
        /// <value>The interface table idx.</value>
        public TokenTypes InterfaceTableIdx
        {
            get { return _interfaceTableIdx; }
        }

        #endregion // Properties
	}
}
