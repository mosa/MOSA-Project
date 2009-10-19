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
	public struct FileRow {
		#region Data members

        /// <summary>
        /// 
        /// </summary>
        private FileAttributes _flags;

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _nameStringIdx;

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _hashValueBlobIdx;

		#endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="FileRow"/> struct.
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <param name="nameStringIdx">The name string idx.</param>
        /// <param name="hashValueBlobIdx">The hash value BLOB idx.</param>
        public FileRow(FileAttributes flags, TokenTypes nameStringIdx, TokenTypes hashValueBlobIdx)
        {
            _flags = flags;
            _nameStringIdx = nameStringIdx;
            _hashValueBlobIdx = hashValueBlobIdx;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the flags.
        /// </summary>
        /// <value>The flags.</value>
        public FileAttributes Flags
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
        /// Gets the hash value BLOB idx.
        /// </summary>
        /// <value>The hash value BLOB idx.</value>
        public TokenTypes HashValueBlobIdx
        {
            get { return _hashValueBlobIdx; }
        }

        #endregion // Properties
	}
}
