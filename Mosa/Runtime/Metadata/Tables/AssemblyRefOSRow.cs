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
	public struct AssemblyRefOSRow 
    {
		#region Data members

        private uint _platformId;
        private uint _majorVersion;
        private uint _minorVersion;
        private TokenTypes _assemblyRefIdx;

		#endregion // Data members

        #region Construction

        public AssemblyRefOSRow(uint platformId, uint majorVersion, uint minorVersion, TokenTypes assemblyRefIdx)
        {
            _platformId = platformId;
            _majorVersion = majorVersion;
            _minorVersion = minorVersion;
            _assemblyRefIdx = assemblyRefIdx;
        }

        #endregion // Construction

        #region Properties

        public uint PlatformId
        {
            get { return _platformId; }
        }

        public uint MajorVersion
        {
            get { return _majorVersion; }
        }

        public uint MinorVersion
        {
            get { return _minorVersion; }
        }

        public TokenTypes AssemblyRefIdx
        {
            get { return _assemblyRefIdx; }
        }

        #endregion // Properties
	}
}
