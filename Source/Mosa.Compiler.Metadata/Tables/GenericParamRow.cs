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
	public class GenericParamRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericParamRow" /> struct.
		/// </summary>
		/// <param name="number">The number.</param>
		/// <param name="flags">The flags.</param>
		/// <param name="owner">The owner.</param>
		/// <param name="nameString">The name string.</param>
		public GenericParamRow(ushort number, GenericParameterAttributes flags, Token owner, HeapIndexToken nameString)
		{
			Number = number;
			Flags = flags;
			Owner = owner;
			NameString = nameString;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the number.
		/// </summary>
		/// <value>The number.</value>
		public ushort Number { get; private set; }

		/// <summary>
		/// Gets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public GenericParameterAttributes Flags { get; private set; }

		/// <summary>
		/// Gets the owner.
		/// </summary>
		/// <value>
		/// The owner.
		/// </value>
		public Token Owner { get; private set; }

		/// <summary>
		/// Gets the name string.
		/// </summary>
		/// <value>
		/// The name string.
		/// </value>
		public HeapIndexToken NameString { get; private set; }

		#endregion Properties
	}
}