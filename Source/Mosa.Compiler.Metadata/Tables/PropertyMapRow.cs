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
	public struct PropertyMapRow
	{
		#region Data members

		/// <summary>
		///
		/// </summary>
		private Token _parent;

		/// <summary>
		///
		/// </summary>
		private Token _property;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="PropertyMapRow"/> struct.
		/// </summary>
		/// <param name="parentTableIdx">The parent table idx.</param>
		/// <param name="propertyTableIdx">The property table idx.</param>
		public PropertyMapRow(Token parent, Token property)
		{
			_parent = parent;
			_property = property;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the parent table idx.
		/// </summary>
		/// <value>The parent table idx.</value>
		public Token ParentTableIdx
		{
			get { return _parent; }
		}

		/// <summary>
		/// Gets the property table idx.
		/// </summary>
		/// <value>The property table idx.</value>
		public Token PropertyTableIdx
		{
			get { return _property; }
		}

		#endregion Properties
	}
}