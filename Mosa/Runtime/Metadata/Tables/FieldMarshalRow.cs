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
	public struct FieldMarshalRow {
		#region Data members

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _parentTableIdx;

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _nativeTypeBlobIdx;

		#endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldMarshalRow"/> struct.
        /// </summary>
        /// <param name="parentTableIdx">The parent table idx.</param>
        /// <param name="nativeTypeBlobIdx">The native type BLOB idx.</param>
        public FieldMarshalRow(TokenTypes parentTableIdx, TokenTypes nativeTypeBlobIdx)
        {
            _parentTableIdx = parentTableIdx;
            _nativeTypeBlobIdx = nativeTypeBlobIdx;
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
        /// Gets the native type BLOB idx.
        /// </summary>
        /// <value>The native type BLOB idx.</value>
        public TokenTypes NativeTypeBlobIdx
        {
            get { return _nativeTypeBlobIdx; }
        }

        #endregion // Properties
	}
}
