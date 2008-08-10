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
	public struct AssemblyRefRow 
    {
		#region Data members

        private ushort _majorVersion;

        private ushort _minorVersion;

        private ushort _buildNumber;

        private ushort _revision;

        private AssemblyFlags _flags;

        private TokenTypes _publicKeyOrTokenIdx;

        private TokenTypes _nameIdx;

        private TokenTypes _cultureIdx;

        private TokenTypes _hashValueIdx;

		#endregion // Data members

        #region Construction

        public AssemblyRefRow(ushort majorVersion, ushort minorVersion, ushort buildNumber, ushort revisionNumber, 
                                AssemblyFlags flags, TokenTypes publicKeyOrTokenIdx, TokenTypes nameIdx, 
                                TokenTypes cultureIdx, TokenTypes hashValueIdx)
        {
            _majorVersion = majorVersion;
            _minorVersion = minorVersion;
            _buildNumber = buildNumber;
            _revision = revisionNumber;
            _flags = flags;
            _publicKeyOrTokenIdx = publicKeyOrTokenIdx;
            _nameIdx = nameIdx;
            _cultureIdx = cultureIdx;
            _hashValueIdx = hashValueIdx;
        }

        #endregion // Construction

        #region Properties

        public ushort MajorVersion
        {
            get { return _majorVersion; }
        }

        public ushort MinorVersion
        {
            get { return _minorVersion; }
        }

        public ushort BuildNumber
        {
            get { return _buildNumber; }
        }

        public ushort Revision
        {
            get { return _revision; }
        }

        public AssemblyFlags Flags
        {
            get { return _flags; }
        }

        public TokenTypes PublicKeyOrTokenIdx
        {
            get { return _publicKeyOrTokenIdx; }
        }

        public TokenTypes NameIdx
        {
            get { return _nameIdx; }
        }

        public TokenTypes CultureIdx
        {
            get { return _cultureIdx; }
        }

        public TokenTypes HashValueIdx
        {
            get { return _hashValueIdx; }
        }

        #endregion // Properties
	}
}
