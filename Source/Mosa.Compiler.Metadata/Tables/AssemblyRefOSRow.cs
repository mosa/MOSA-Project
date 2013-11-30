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
	public class AssemblyRefOSRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyRefOSRow"/> struct.
		/// </summary>
		/// <param name="platformId">The platform id.</param>
		/// <param name="majorVersion">The major version.</param>
		/// <param name="minorVersion">The minor version.</param>
		/// <param name="assemblyRef">The assembly ref.</param>
		public AssemblyRefOSRow(uint platformId, uint majorVersion, uint minorVersion, Token assemblyRef)
		{
			OSPlatformId = platformId;
			OSMajorVersion = majorVersion;
			OSMinorVersion = minorVersion;
			AssemblyRef = assemblyRef;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the platform id.
		/// </summary>
		/// <value>The platform id.</value>
		public uint OSPlatformId { get; private set; }

		/// <summary>
		/// Gets the major version.
		/// </summary>
		/// <value>The major version.</value>
		public uint OSMajorVersion { get; private set; }

		/// <summary>
		/// Gets the minor version.
		/// </summary>
		/// <value>The minor version.</value>
		public uint OSMinorVersion { get; private set; }

		/// <summary>
		/// Gets the assembly reference.
		/// </summary>
		/// <value>
		/// The assembly reference.
		/// </value>
		public Token AssemblyRef { get; private set; }

		#endregion Properties
	}
}