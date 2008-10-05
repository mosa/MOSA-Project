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
	public struct CustomAttributeRow 
    {
		#region Data members

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _parentTableIdx;

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _typeIdx;

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _valueBlobIdx;

		#endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomAttributeRow"/> struct.
        /// </summary>
        /// <param name="parentTableIdx">The parent table idx.</param>
        /// <param name="typeIdx">The type idx.</param>
        /// <param name="valueBlobIdx">The value BLOB idx.</param>
        public CustomAttributeRow(TokenTypes parentTableIdx, TokenTypes typeIdx, TokenTypes valueBlobIdx)
        {
            _parentTableIdx = parentTableIdx;
            _typeIdx = typeIdx;
            _valueBlobIdx = valueBlobIdx;
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
        /// Gets the type idx.
        /// </summary>
        /// <value>The type idx.</value>
        public TokenTypes TypeIdx
        {
            get { return _typeIdx; }
        }

        /// <summary>
        /// Gets the value BLOB idx.
        /// </summary>
        /// <value>The value BLOB idx.</value>
        public TokenTypes ValueBlobIdx
        {
            get { return _valueBlobIdx; }
        }

        #endregion // Properties
	}
}
