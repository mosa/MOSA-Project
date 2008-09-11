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
	public struct ConstantRow 
    {
		#region Data members

        /// <summary>
        /// 
        /// </summary>
        private CilElementType _type; // FIXME: Enum?

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _parent;

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _valueBlobIdx;

		#endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantRow"/> struct.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="valueBlobIdx">The value BLOB idx.</param>
        public ConstantRow(CilElementType type, TokenTypes parent, TokenTypes valueBlobIdx)
        {
            _type = type;
            _parent = parent;
            _valueBlobIdx = valueBlobIdx;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public CilElementType Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public TokenTypes Parent
        {
            get { return _parent; }
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
