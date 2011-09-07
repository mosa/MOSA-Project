/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */


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
		private uint platformId;
		/// <summary>
		/// 
		/// </summary>
		private uint majorVersion;
		/// <summary>
		/// 
		/// </summary>
		private uint minorVersion;
		/// <summary>
		/// 
		/// </summary>
		private Token assemblyRef;

		#endregion // Data members

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
			this.platformId = platformId;
			this.majorVersion = majorVersion;
			this.minorVersion = minorVersion;
			this.assemblyRef = assemblyRef;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the platform id.
		/// </summary>
		/// <value>The platform id.</value>
		public uint OSPlatformId
		{
			get { return platformId; }
		}

		/// <summary>
		/// Gets the major version.
		/// </summary>
		/// <value>The major version.</value>
		public uint OSMajorVersion
		{
			get { return majorVersion; }
		}

		/// <summary>
		/// Gets the minor version.
		/// </summary>
		/// <value>The minor version.</value>
		public uint OSMinorVersion
		{
			get { return minorVersion; }
		}

		/// <summary>
		/// Gets the assembly ref idx.
		/// </summary>
		/// <value>The assembly ref idx.</value>
		public Token AssemblyRef
		{
			get { return assemblyRef; }
		}

		#endregion // Properties
	}
}
