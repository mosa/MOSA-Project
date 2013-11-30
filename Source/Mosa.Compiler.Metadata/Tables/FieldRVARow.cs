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
	public class FieldRVARow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldRVARow" /> struct.
		/// </summary>
		/// <param name="rva">The rva.</param>
		/// <param name="field">The field.</param>
		public FieldRVARow(uint rva, Token field)
		{
			Rva = rva;
			Field = field;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the rva.
		/// </summary>
		/// <value>The rva.</value>
		public uint Rva { get; private set; }

		/// <summary>
		/// Gets the field.
		/// </summary>
		/// <value>The field.</value>
		public Token Field { get; private set; }

		#endregion Properties
	}
}