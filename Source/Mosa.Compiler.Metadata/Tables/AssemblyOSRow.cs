/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Metadata.Tables
{
	/// <summary>
	///
	/// </summary>
	public class AssemblyOSRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyOSRow"/> struct.
		/// </summary>
		/// <param name="platformId">The platform id.</param>
		/// <param name="majorVersion">The major version.</param>
		/// <param name="minorVersion">The minor version.</param>
		public AssemblyOSRow(int platformId, int majorVersion, int minorVersion)
		{
			OSPlatformID = platformId;
			OSMajorVersion = majorVersion;
			OSMinorVersion = minorVersion;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the platform id.
		/// </summary>
		/// <value>The platform id.</value>
		public int OSPlatformID { get; private set; }

		/// <summary>
		/// Gets the major version.
		/// </summary>
		/// <value>The major version.</value>
		public int OSMajorVersion { get; private set; }

		/// <summary>
		/// Gets the minor version.
		/// </summary>
		/// <value>The minor version.</value>
		public int OSMinorVersion { get; private set; }

		#endregion Properties
	}
}