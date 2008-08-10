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
	public struct FileRow {
		#region Data members

        private FileAttributes _flags;

        private TokenTypes _nameStringIdx;

        private TokenTypes _hashValueBlobIdx;

		#endregion // Data members

        #region Construction

        public FileRow(FileAttributes flags, TokenTypes nameStringIdx, TokenTypes hashValueBlobIdx)
        {
            _flags = flags;
            _nameStringIdx = nameStringIdx;
            _hashValueBlobIdx = hashValueBlobIdx;
        }

        #endregion // Construction

        #region Properties

        public FileAttributes Flags
        {
            get { return _flags; }
        }

        public TokenTypes NameStringIdx
        {
            get { return _nameStringIdx; }
        }

        public TokenTypes HashValueBlobIdx
        {
            get { return _hashValueBlobIdx; }
        }

        #endregion // Properties
	}
}
