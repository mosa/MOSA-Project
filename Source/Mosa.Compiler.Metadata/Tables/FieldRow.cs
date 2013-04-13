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
	public struct FieldRow
	{
		#region Data members

		/// <summary>
		///
		/// </summary>
		private readonly FieldAttributes flags;

		/// <summary>
		///
		/// </summary>
		private readonly HeapIndexToken name;

		/// <summary>
		///
		/// </summary>
		private readonly HeapIndexToken signature;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldRow"/> struct.
		/// </summary>
		/// <param name="flags">The flags.</param>
		/// <param name="name">The name string idx.</param>
		/// <param name="signature">The signature BLOB idx.</param>
		public FieldRow(FieldAttributes flags, HeapIndexToken name, HeapIndexToken signature)
		{
			this.flags = flags;
			this.name = name;
			this.signature = signature;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public FieldAttributes Flags
		{
			get { return flags; }
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
		/// Gets the signature.
		/// </summary>
		/// <value>The signature.</value>
		public HeapIndexToken Signature
		{
			get { return signature; }
		}

		#endregion Properties
	}
}