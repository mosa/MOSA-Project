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
	public struct MemberRefRow 
    {
		#region Data members

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _classTableIdx;

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _nameStringIdx;

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _signatureBlobIdx;

		#endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRefRow"/> struct.
        /// </summary>
        /// <param name="classTableIdx">The class table idx.</param>
        /// <param name="nameStringIdx">The name string idx.</param>
        /// <param name="signatureBlobIdx">The signature BLOB idx.</param>
        public MemberRefRow(TokenTypes classTableIdx, TokenTypes nameStringIdx, TokenTypes signatureBlobIdx)
        {
            _classTableIdx = classTableIdx;
            _nameStringIdx = nameStringIdx;
            _signatureBlobIdx = signatureBlobIdx;
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
        /// Gets the name string idx.
        /// </summary>
        /// <value>The name string idx.</value>
        public TokenTypes NameStringIdx
        {
            get { return _nameStringIdx; }
        }

        /// <summary>
        /// Gets the signature BLOB idx.
        /// </summary>
        /// <value>The signature BLOB idx.</value>
        public TokenTypes SignatureBlobIdx
        {
            get { return _signatureBlobIdx; }
        }

        #endregion // Properties
	}
}
