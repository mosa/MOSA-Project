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
	public struct FieldRVARow
	{
		#region Data members

		/// <summary>
		///
		/// </summary>
		private uint rva;

		/// <summary>
		///
		/// </summary>
		private Token field;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldRVARow"/> struct.
		/// </summary>
		/// <param name="rva">The rva.</param>
		/// <param name="field">The field table idx.</param>
		public FieldRVARow(uint rva, Token field)
		{
			this.rva = rva;
			this.field = field;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the rva.
		/// </summary>
		/// <value>The rva.</value>
		public uint Rva
		{
			get { return rva; }
		}

		/// <summary>
		/// Gets the field.
		/// </summary>
		/// <value>The field.</value>
		public Token Field
		{
			get { return field; }
		}

		#endregion Properties
	}
}