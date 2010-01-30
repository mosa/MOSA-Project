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
	public struct AssemblyRow
    {
        #region Data members

        /// <summary>
        /// 
        /// </summary>
        private AssemblyHashAlgorithm _hashAlgId;
        /// <summary>
        /// 
        /// </summary>
        private ushort _majorVersion;
        /// <summary>
        /// 
        /// </summary>
        private ushort _minorVersion;
        /// <summary>
        /// 
        /// </summary>
        private ushort _buildNumber;
        /// <summary>
        /// 
        /// </summary>
        private ushort _revision;
        /// <summary>
        /// 
        /// </summary>
        private AssemblyFlags _flags;
        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _publicKeyIdx;
        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _nameIdx;
        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _cultureIdx;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyRow"/> struct.
        /// </summary>
        /// <param name="hashAlgId">The hash alg id.</param>
        /// <param name="majorVersion">The major version.</param>
        /// <param name="minorVersion">The minor version.</param>
        /// <param name="buildNumber">The build number.</param>
        /// <param name="revision">The revision.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="publicKeyIdx">The public key idx.</param>
        /// <param name="nameIdx">The name idx.</param>
        /// <param name="cultureIdx">The culture idx.</param>
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

        /// <summary>
        /// Gets or sets the hash alg id.
        /// </summary>
        /// <value>The hash alg id.</value>
        public AssemblyHashAlgorithm HashAlgId
        {
            get { return _hashAlgId; }
            set { _hashAlgId = value; }
        }

        /// <summary>
        /// Gets or sets the major version.
        /// </summary>
        /// <value>The major version.</value>
        public ushort MajorVersion
        {
            get { return _majorVersion; }
            set { _majorVersion = value; }
        }

        /// <summary>
        /// Gets the minor version.
        /// </summary>
        /// <value>The minor version.</value>
        public ushort MinorVersion
        {
            get { return _minorVersion; }
        }

        /// <summary>
        /// Gets the build number.
        /// </summary>
        /// <value>The build number.</value>
        public ushort BuildNumber
        {
            get { return _buildNumber; }
        }

        /// <summary>
        /// Gets the revision.
        /// </summary>
        /// <value>The revision.</value>
        public ushort Revision
        {
            get { return _revision; }
        }

        /// <summary>
        /// Gets the flags.
        /// </summary>
        /// <value>The flags.</value>
        public AssemblyFlags Flags
        {
            get { return _flags; }
        }

        /// <summary>
        /// Gets the public key idx.
        /// </summary>
        /// <value>The public key idx.</value>
        public TokenTypes PublicKeyIdx
        {
            get { return _publicKeyIdx; }
        }

        /// <summary>
        /// Gets the name idx.
        /// </summary>
        /// <value>The name idx.</value>
        public TokenTypes NameIdx
        {
            get { return _nameIdx; }
        }

        /// <summary>
        /// Gets the culture idx.
        /// </summary>
        /// <value>The culture idx.</value>
        public TokenTypes CultureIdx
        {
            get { return _cultureIdx; }
        }

        #endregion // Properties
	}
}
