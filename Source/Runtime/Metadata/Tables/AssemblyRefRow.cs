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
	public struct AssemblyRefRow 
    {
		#region Data members

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
        private TokenTypes _publicKeyOrTokenIdx;

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _nameIdx;

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _cultureIdx;

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _hashValueIdx;

		#endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyRefRow"/> struct.
        /// </summary>
        /// <param name="majorVersion">The major version.</param>
        /// <param name="minorVersion">The minor version.</param>
        /// <param name="buildNumber">The build number.</param>
        /// <param name="revisionNumber">The revision number.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="publicKeyOrTokenIdx">The public key or token idx.</param>
        /// <param name="nameIdx">The name idx.</param>
        /// <param name="cultureIdx">The culture idx.</param>
        /// <param name="hashValueIdx">The hash value idx.</param>
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

        /// <summary>
        /// Gets the major version.
        /// </summary>
        /// <value>The major version.</value>
        public ushort MajorVersion
        {
            get { return _majorVersion; }
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
        /// Gets the public key or token idx.
        /// </summary>
        /// <value>The public key or token idx.</value>
        public TokenTypes PublicKeyOrTokenIdx
        {
            get { return _publicKeyOrTokenIdx; }
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

        /// <summary>
        /// Gets the hash value idx.
        /// </summary>
        /// <value>The hash value idx.</value>
        public TokenTypes HashValueIdx
        {
            get { return _hashValueIdx; }
        }

        #endregion // Properties
	}
}
