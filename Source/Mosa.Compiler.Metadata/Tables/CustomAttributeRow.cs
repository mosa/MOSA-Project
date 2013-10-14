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
	public struct CustomAttributeRow
	{
		#region Data members

		/// <summary>
		///
		/// </summary>
		private Token parent;

		/// <summary>
		///
		/// </summary>
		private Token type;

		/// <summary>
		///
		/// </summary>
		private HeapIndexToken value;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CustomAttributeRow"/> struct.
		/// </summary>
		/// <param name="parent">The parent.</param>
		/// <param name="type">The type.</param>
		/// <param name="value">The value.</param>
		public CustomAttributeRow(Token parent, Token type, HeapIndexToken value)
		{
			this.parent = parent;
			this.type = type;
			this.value = value;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the parent.
		/// </summary>
		/// <value>The parent.</value>
		public Token Parent
		{
			get { return parent; }
		}

		/// <summary>
		/// Gets the type.
		/// </summary>
		/// <value>The type.</value>
		public Token Type
		{
			get { return type; }
		}

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <value>The value.</value>
		public HeapIndexToken Value
		{
			get { return value; }
		}

		#endregion Properties
	}
}