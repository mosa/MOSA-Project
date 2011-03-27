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
	public struct AssemblyRefOSRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private uint _platformId;
		/// <summary>
		/// 
		/// </summary>
		private uint _majorVersion;
		/// <summary>
		/// 
		/// </summary>
		private uint _minorVersion;
		/// <summary>
		/// 
		/// </summary>
		private MetadataToken _assemblyRefIdx;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyRefOSRow"/> struct.
		/// </summary>
		/// <param name="platformId">The platform id.</param>
		/// <param name="majorVersion">The major version.</param>
		/// <param name="minorVersion">The minor version.</param>
		/// <param name="assemblyRefIdx">The assembly ref idx.</param>
		public AssemblyRefOSRow(uint platformId, uint majorVersion, uint minorVersion, MetadataToken assemblyRefIdx)
		{
			_platformId = platformId;
			_majorVersion = majorVersion;
			_minorVersion = minorVersion;
			_assemblyRefIdx = assemblyRefIdx;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the platform id.
		/// </summary>
		/// <value>The platform id.</value>
		public uint PlatformId
		{
			get { return _platformId; }
		}

		/// <summary>
		/// Gets the major version.
		/// </summary>
		/// <value>The major version.</value>
		public uint MajorVersion
		{
			get { return _majorVersion; }
		}

		/// <summary>
		/// Gets the minor version.
		/// </summary>
		/// <value>The minor version.</value>
		public uint MinorVersion
		{
			get { return _minorVersion; }
		}

		/// <summary>
		/// Gets the assembly ref idx.
		/// </summary>
		/// <value>The assembly ref idx.</value>
		public MetadataToken AssemblyRefIdx
		{
			get { return _assemblyRefIdx; }
		}

		#endregion // Properties
	}
}
