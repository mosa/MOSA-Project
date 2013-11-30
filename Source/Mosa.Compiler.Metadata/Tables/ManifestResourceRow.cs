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
	public class ManifestResourceRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ManifestResourceRow" /> struct.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="flags">The flags.</param>
		/// <param name="nameString">Index of the name string.</param>
		/// <param name="implementation">The implementation.</param>
		public ManifestResourceRow(uint offset, ManifestResourceAttributes flags, HeapIndexToken nameString,
			Token implementation)
		{
			Offset = offset;
			Flags = flags;
			NameString = nameString;
			Implementation = implementation;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the offset.
		/// </summary>
		/// <value>The offset.</value>
		public uint Offset { get; private set; }

		/// <summary>
		/// Gets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public ManifestResourceAttributes Flags { get; private set; }

		/// <summary>
		/// Gets the name string.
		/// </summary>
		/// <value>
		/// The name string.
		/// </value>
		public HeapIndexToken NameString { get; private set; }

		/// <summary>
		/// Gets the implementation.
		/// </summary>
		/// <value>
		/// The implementation.
		/// </value>
		public Token Implementation { get; private set; }

		#endregion Properties
	}
}