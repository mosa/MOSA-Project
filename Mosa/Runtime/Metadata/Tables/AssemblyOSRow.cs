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
	public struct AssemblyOSRow
    {
		#region Data members

        private int _platformId;

        private int _majorVersion;

        private int _minorVersion;

		#endregion // Data members

        #region Construction

        public AssemblyOSRow(int platformId, int majorVersion, int minorVersion)
        {
            _platformId = platformId;
            _majorVersion = majorVersion;
            _minorVersion = minorVersion;
        }

        #endregion // Construction

        #region Properties

        public int PlatformId
        {
            get { return _platformId; }
        }

        public int MajorVersion
        {
            get { return _majorVersion; }
        }

        public int MinorVersion
        {
            get { return _minorVersion; }
        }

        #endregion // Properties
	}
}
