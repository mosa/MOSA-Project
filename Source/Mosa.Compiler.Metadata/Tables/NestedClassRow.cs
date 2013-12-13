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
	public class NestedClassRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="NestedClassRow" /> struct.
		/// </summary>
		/// <param name="nestedClass">The nested class.</param>
		/// <param name="enclosingClass">The enclosing class.</param>
		public NestedClassRow(Token nestedClass, Token enclosingClass)
		{
			NestedClass = nestedClass;
			EnclosingClass = enclosingClass;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the nested class.
		/// </summary>
		/// <value>The nested class.</value>
		public Token NestedClass { get; private set; }

		/// <summary>
		/// Gets the enclosing class.
		/// </summary>
		/// <value>The enclosing class .</value>
		public Token EnclosingClass { get; private set; }

		#endregion Properties
	}
}