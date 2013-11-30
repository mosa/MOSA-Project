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
	public class AssemblyRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyRow"/> struct.
		/// </summary>
		/// <param name="hashAlgId">The hash alg id.</param>
		/// <param name="majorVersion">The major version.</param>
		/// <param name="minorVersion">The minor version.</param>
		/// <param name="buildNumber">The build number.</param>
		/// <param name="revision">The revision.</param>
		/// <param name="flags">The flags.</param>
		/// <param name="publicKey">The public key.</param>
		/// <param name="name">The name.</param>
		/// <param name="culture">The culture.</param>
		public AssemblyRow(AssemblyHashAlgorithm hashAlgId,
							ushort majorVersion, ushort minorVersion, ushort buildNumber, ushort revision,
							AssemblyAttributes flags, HeapIndexToken publicKey, HeapIndexToken name, HeapIndexToken culture)
		{
			HashAlgId = hashAlgId;
			MajorVersion = majorVersion;
			MinorVersion = minorVersion;
			BuildNumber = buildNumber;
			Revision = revision;
			Flags = flags;
			PublicKey = publicKey;
			Name = name;
			Culture = culture;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets or sets the hash alg id.
		/// </summary>
		/// <value>The hash alg id.</value>
		public AssemblyHashAlgorithm HashAlgId { get; private set; }

		/// <summary>
		/// Gets or sets the major version.
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
		public ushort Revision { get; private set; }

		/// <summary>
		/// Gets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public AssemblyAttributes Flags { get; private set; }

		/// <summary>
		/// Gets the public key.
		/// </summary>
		/// <value>The public key.</value>
		public HeapIndexToken PublicKey { get; private set; }

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public HeapIndexToken Name { get; private set; }

		/// <summary>
		/// Gets the culture.
		/// </summary>
		/// <value>
		/// The culture.
		/// </value>
		public HeapIndexToken Culture { get; private set; }

		#endregion Properties
	}
}