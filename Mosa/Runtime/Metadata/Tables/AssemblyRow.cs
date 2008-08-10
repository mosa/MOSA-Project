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
	public struct AssemblyRow
    {
        #region Data members

        private AssemblyHashAlgorithm _hashAlgId;
        private ushort _majorVersion;
        private ushort _minorVersion;
        private ushort _buildNumber;
        private ushort _revision;
        private AssemblyFlags _flags;
        private TokenTypes _publicKeyIdx;
        private TokenTypes _nameIdx;
        private TokenTypes _cultureIdx;

        #endregion // Data members

        #region Construction

        public AssemblyRow(AssemblyHashAlgorithm hashAlgId,
                            ushort majorVersion, ushort minorVersion, ushort buildNumber, ushort revision,
                            AssemblyFlags flags, TokenTypes publicKeyIdx, TokenTypes nameIdx, TokenTypes cultureIdx)
        {
            _hashAlgId = hashAlgId;
            _majorVersion = majorVersion;
            _minorVersion = minorVersion;
            _buildNumber = buildNumber;
            _revision = revision;
            _flags = flags;
            _publicKeyIdx = publicKeyIdx;
            _nameIdx = nameIdx;
            _cultureIdx = cultureIdx;
        }

        #endregion // Construction

        #region Properties

        public AssemblyHashAlgorithm HashAlgId
        {
            get { return _hashAlgId; }
            set { _hashAlgId = value; }
        }

        public ushort MajorVersion
        {
            get { return _majorVersion; }
            set { _majorVersion = value; }
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

        public TokenTypes PublicKeyIdx
        {
            get { return _publicKeyIdx; }
        }

        public TokenTypes NameIdx
        {
            get { return _nameIdx; }
        }

        public TokenTypes CultureIdx
        {
            get { return _cultureIdx; }
        }

        #endregion // Properties
	}
}
