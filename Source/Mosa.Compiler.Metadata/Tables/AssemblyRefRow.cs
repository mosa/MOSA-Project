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
	public class AssemblyRefRow
	{
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
			MajorVersion = majorVersion;
			MinorVersion = minorVersion;
			BuildNumber = buildNumber;
			RevisionNumber = revisionNumber;
			Flags = flags;
			PublicKeyOrToken = publicKeyOrToken;
			Name = name;
			Culture = culture;
			HashValue = hashValue;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the major version.
		/// </summary>
		/// <value>The major version.</value>
		public ushort MajorVersion { get; private set; }

		/// <summary>
		/// Gets the minor version.
		/// </summary>
		/// <value>The minor version.</value>
		public ushort MinorVersion { get; private set; }

		/// <summary>
		/// Gets the build number.
		/// </summary>
		/// <value>The build number.</value>
		public ushort BuildNumber { get; private set; }

		/// <summary>
		/// Gets the revision.
		/// </summary>
		/// <value>The revision.</value>
		public ushort RevisionNumber { get; private set; }

		/// <summary>
		/// Gets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public AssemblyAttributes Flags { get; private set; }

		/// <summary>
		/// Gets the public key or token.
		/// </summary>
		/// <value>The public key or token.</value>
		public HeapIndexToken PublicKeyOrToken { get; private set; }

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public HeapIndexToken Name { get; private set; }

		/// <summary>
		/// Gets the culture.
		/// </summary>
		/// <value>The culture.</value>
		public HeapIndexToken Culture { get; private set; }

		/// <summary>
		/// Gets the hash value.
		/// </summary>
		/// <value>The hash value.</value>
		public HeapIndexToken HashValue { get; private set; }

		#endregion Properties
	}
}