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
	public class PropertyRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="PropertyRow" /> struct.
		/// </summary>
		/// <param name="flags">The flags.</param>
		/// <param name="nameString">The name string.</param>
		/// <param name="typeBlob">The type BLOB.</param>
		public PropertyRow(PropertyAttributes flags, HeapIndexToken nameString, HeapIndexToken typeBlob)
		{
			Flags = flags;
			NameString = nameString;
			TypeBlob = typeBlob;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public PropertyAttributes Flags { get; private set; }

		/// <summary>
		/// Gets the name string.
		/// </summary>
		/// <value>
		/// The name string.
		/// </value>
		public HeapIndexToken NameString { get; private set; }

		/// <summary>
		/// Gets the type BLOB.
		/// </summary>
		/// <value>
		/// The type BLOB.
		/// </value>
		public HeapIndexToken TypeBlob { get; private set; }

		#endregion Properties
	}
}