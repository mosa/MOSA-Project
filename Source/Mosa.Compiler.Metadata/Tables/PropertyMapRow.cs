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
	public class PropertyMapRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="PropertyMapRow" /> struct.
		/// </summary>
		/// <param name="parent">The parent.</param>
		/// <param name="property">The property.</param>
		public PropertyMapRow(Token parent, Token property)
		{
			Parent = parent;
			Property = property;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the parent.
		/// </summary>
		/// <value>
		/// The parent.
		/// </value>
		public Token Parent { get; private set; }

		/// <summary>
		/// Gets the property.
		/// </summary>
		/// <value>
		/// The property.
		/// </value>
		public Token Property { get; private set; }

		#endregion Properties
	}
}