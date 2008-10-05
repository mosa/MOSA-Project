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
    /// <summary>
    /// 
    /// </summary>
	public struct AssemblyOSRow
    {
		#region Data members

        /// <summary>
        /// 
        /// </summary>
        private int _platformId;

        /// <summary>
        /// 
        /// </summary>
        private int _majorVersion;

        /// <summary>
        /// 
        /// </summary>
        private int _minorVersion;

		#endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyOSRow"/> struct.
        /// </summary>
        /// <param name="platformId">The platform id.</param>
        /// <param name="majorVersion">The major version.</param>
        /// <param name="minorVersion">The minor version.</param>
        public AssemblyOSRow(int platformId, int majorVersion, int minorVersion)
        {
            _platformId = platformId;
            _majorVersion = majorVersion;
            _minorVersion = minorVersion;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the platform id.
        /// </summary>
        /// <value>The platform id.</value>
        public int PlatformId
        {
            get { return _platformId; }
        }

        /// <summary>
        /// Gets the major version.
        /// </summary>
        /// <value>The major version.</value>
        public int MajorVersion
        {
            get { return _majorVersion; }
        }

        /// <summary>
        /// Gets the minor version.
        /// </summary>
        /// <value>The minor version.</value>
        public int MinorVersion
        {
            get { return _minorVersion; }
        }

        #endregion // Properties
	}
}
