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
	public class ConstantRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ConstantRow" /> struct.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="parent">The parent.</param>
		/// <param name="value">The value.</param>
		public ConstantRow(CilElementType type, Token parent, HeapIndexToken value)
		{
			Type = type;
			Parent = parent;
			Value = value;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the type.
		/// </summary>
		/// <value>The type.</value>
		public CilElementType Type { get; private set; }

		/// <summary>
		/// Gets the parent.
		/// </summary>
		/// <value>The parent.</value>
		public Token Parent { get; private set; }

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		public HeapIndexToken Value { get; private set; }

		#endregion Properties
	}
}