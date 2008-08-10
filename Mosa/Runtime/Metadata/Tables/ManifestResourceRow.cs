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
	public struct ManifestResourceRow 
    {
		#region Data members

        private uint _offset;

        private ManifestResourceAttributes _flags;

        private TokenTypes _nameStringIdx;

        private TokenTypes _implementationTableIdx;

		#endregion // Data members

        #region Construction

        public ManifestResourceRow(uint offset, ManifestResourceAttributes flags, TokenTypes nameStringIndex,
            TokenTypes implementationTableIdx)
        {
            _offset = offset;
            _flags = flags;
            _nameStringIdx = nameStringIndex;
            _implementationTableIdx = implementationTableIdx;
        }

        #endregion // Construction

        #region Properties

        public uint Offset
        {
            get { return _offset; }
        }

        public ManifestResourceAttributes Flags
        {
            get { return _flags; }
        }

        public TokenTypes NameStringIdx
        {
            get { return _nameStringIdx; }
        }

        public TokenTypes ImplementationTableIdx
        {
            get { return _implementationTableIdx; }
        }

        #endregion // Properties
	}
}
