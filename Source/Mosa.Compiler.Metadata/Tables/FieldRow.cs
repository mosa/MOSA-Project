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
	public class FieldRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldRow" /> struct.
		/// </summary>
		/// <param name="flags">The flags.</param>
		/// <param name="name">The name.</param>
		/// <param name="signature">The signature.</param>
		public FieldRow(FieldAttributes flags, HeapIndexToken name, HeapIndexToken signature)
		{
			Flags = flags;
			Name = name;
			Signature = signature;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public FieldAttributes Flags { get; private set; }

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public HeapIndexToken Name { get; private set; }

		/// <summary>
		/// Gets the signature.
		/// </summary>
		/// <value>The signature.</value>
		public HeapIndexToken Signature { get; private set; }

		#endregion Properties
	}
}