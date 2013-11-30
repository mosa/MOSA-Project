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
	public class FieldLayoutRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldLayoutRow"/> struct.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="field">The field.</param>
		public FieldLayoutRow(uint offset, Token field)
		{
			Offset = offset;
			Field = field;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the offset.
		/// </summary>
		/// <value>The offset.</value>
		public uint Offset { get; private set; }

		/// <summary>
		/// Gets the field.
		/// </summary>
		/// <value>The field.</value>
		public Token Field { get; private set; }

		#endregion Properties
	}
}