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
	public class ParamRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ParamRow" /> struct.
		/// </summary>
		/// <param name="flags">The flags.</param>
		/// <param name="sequence">The sequence.</param>
		/// <param name="name">The name.</param>
		public ParamRow(ParameterAttributes flags, short sequence, HeapIndexToken name)
		{
			Name = name;
			Sequence = sequence;
			Flags = flags;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Returns the attributes of this parameter.
		/// </summary>
		/// <value>The flags.</value>
		public ParameterAttributes Flags { get; private set; }

		/// <summary>
		/// Retrieves the token of the parameter name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public HeapIndexToken Name { get; private set; }

		/// <summary>
		/// Retrieves the parameter sequence number.
		/// </summary>
		/// <value>The sequence.</value>
		public short Sequence { get; private set; }

		#endregion Properties
	}
}