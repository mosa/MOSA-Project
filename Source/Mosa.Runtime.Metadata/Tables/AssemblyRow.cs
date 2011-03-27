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

using Mosa.Runtime.Metadata;

namespace Mosa.Runtime.Metadata.Tables
{
	/// <summary>
	/// 
	/// </summary>
	public struct AssemblyRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private AssemblyHashAlgorithm hashAlgId;
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
		private HeapIndexToken publicKey;
		/// <summary>
		/// 
		/// </summary>
		private HeapIndexToken name;
		/// <summary>
		/// 
		/// </summary>
		private HeapIndexToken culture;

		#endregion // Data members

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
			this.hashAlgId = hashAlgId;
			this.majorVersion = majorVersion;
			this.minorVersion = minorVersion;
			this.buildNumber = buildNumber;
			this.revisionNumber = revision;
			this.flags = flags;
			this.publicKey = publicKey;
			this.name = name;
			this.culture = culture;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets or sets the hash alg id.
		/// </summary>
		/// <value>The hash alg id.</value>
		public AssemblyHashAlgorithm HashAlgId
		{
			get { return hashAlgId; }
			set { hashAlgId = value; }
		}

		/// <summary>
		/// Gets or sets the major version.
		/// </summary>
		/// <value>The major version.</value>
		public ushort MajorVersion
		{
			get { return majorVersion; }
			set { majorVersion = value; }
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
		public ushort Revision
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
		/// Gets the public key.
		/// </summary>
		/// <value>The public key.</value>
		public HeapIndexToken PublicKey
		{
			get { return publicKey; }
		}

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public HeapIndexToken Name
		{
			get { return name; }
		}

		public HeapIndexToken Culture
		{
			get { return culture; }
		}

		#endregion // Properties
	}
}
