/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */


namespace Mosa.Runtime.Metadata.Tables
{
	/// <summary>
	/// 
	/// </summary>
	public struct FieldLayoutRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private uint offset;

		/// <summary>
		/// 
		/// </summary>
		private Token field;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldLayoutRow"/> struct.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="field">The field.</param>
		public FieldLayoutRow(uint offset, Token field)
		{
			this.offset = offset;
			this.field = field;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the offset.
		/// </summary>
		/// <value>The offset.</value>
		public uint Offset
		{
			get { return offset; }
		}

		/// <summary>
		/// Gets the field.
		/// </summary>
		/// <value>The field.</value>
		public Token Field
		{
			get { return field; }
		}

		#endregion // Properties
	}
}
