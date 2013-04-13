/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Compiler.Metadata.Tables
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
		private ushort majorVersion;

		/// <summary>
		///
		/// </summary>
		private ushort minorVersion;

		/// <summary>
		///
		/// </summary>
		private ushort buildNumber;

		/// <summary>
		///
		/// </summary>
		private ushort revisionNumber;

		/// <summary>
		///
		/// </summary>
		private AssemblyAttributes flags;

		/// <summary>
		///
		/// </summary>
		private HeapIndexToken publicKeyOrToken;

		/// <summary>
		///
		/// </summary>
		private HeapIndexToken name;

		/// <summary>
		///
		/// </summary>
		private HeapIndexToken culture;

		/// <summary>
		///
		/// </summary>
		private HeapIndexToken hashValue;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyRefRow"/> struct.
		/// </summary>
		/// <param name="majorVersion">The major version.</param>
		/// <param name="minorVersion">The minor version.</param>
		/// <param name="buildNumber">The build number.</param>
		/// <param name="revisionNumber">The revision number.</param>
		/// <param name="flags">The flags.</param>
		/// <param name="publicKeyOrToken">The public key or token.</param>
		/// <param name="name">The name.</param>
		/// <param name="culture">The culture.</param>
		/// <param name="hashValue">The hash value.</param>
		public AssemblyRefRow(ushort majorVersion, ushort minorVersion, ushort buildNumber, ushort revisionNumber,
								AssemblyAttributes flags, HeapIndexToken publicKeyOrToken, HeapIndexToken name,
								HeapIndexToken culture, HeapIndexToken hashValue)
		{
			this.majorVersion = majorVersion;
			this.minorVersion = minorVersion;
			this.buildNumber = buildNumber;
			this.revisionNumber = revisionNumber;
			this.flags = flags;
			this.publicKeyOrToken = publicKeyOrToken;
			this.name = name;
			this.culture = culture;
			this.hashValue = hashValue;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the major version.
		/// </summary>
		/// <value>The major version.</value>
		public ushort MajorVersion
		{
			get { return majorVersion; }
		}

		/// <summary>
		/// Gets the minor version.
		/// </summary>
		/// <value>The minor version.</value>
		public ushort MinorVersion
		{
			get { return minorVersion; }
		}

		/// <summary>
		/// Gets the build number.
		/// </summary>
		/// <value>The build number.</value>
		public ushort BuildNumber
		{
			get { return buildNumber; }
		}

		/// <summary>
		/// Gets the revision.
		/// </summary>
		/// <value>The revision.</value>
		public ushort RevisionNumber
		{
			get { return revisionNumber; }
		}

		/// <summary>
		/// Gets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public AssemblyAttributes Flags
		{
			get { return flags; }
		}

		/// <summary>
		/// Gets the public key or token.
		/// </summary>
		/// <value>The public key or token.</value>
		public HeapIndexToken PublicKeyOrToken
		{
			get { return publicKeyOrToken; }
		}

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public HeapIndexToken Name
		{
			get { return name; }
		}

		/// <summary>
		/// Gets the culture.
		/// </summary>
		/// <value>The culture.</value>
		public HeapIndexToken Culture
		{
			get { return culture; }
		}

		/// <summary>
		/// Gets the hash value.
		/// </summary>
		/// <value>The hash value.</value>
		public HeapIndexToken HashValue
		{
			get { return hashValue; }
		}

		#endregion Properties
	}
}