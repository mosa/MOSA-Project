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
	public struct NestedClassRow
	{
		#region Data members

		/// <summary>
		///
		/// </summary>
		private Token nestedClass;

		/// <summary>
		///
		/// </summary>
		private Token enclosingClass;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="NestedClassRow"/> struct.
		/// </summary>
		/// <param name="nestedClassTableIdx">The nested class table idx.</param>
		/// <param name="enclosingClassTableIdx">The enclosing class table idx.</param>
		public NestedClassRow(Token nestedClass, Token enclosingClass)
		{
			this.nestedClass = nestedClass;
			this.enclosingClass = enclosingClass;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the nested class.
		/// </summary>
		/// <value>The nested class.</value>
		public Token NestedClass
		{
			get { return nestedClass; }
		}

		/// <summary>
		/// Gets the enclosing class.
		/// </summary>
		/// <value>The enclosing class .</value>
		public Token EnclosingClass
		{
			get { return enclosingClass; }
		}

		#endregion Properties
	}
}