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
	public class GenericParamConstraintRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericParamConstraintRow" /> struct.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="constraint">The constraint.</param>
		public GenericParamConstraintRow(Token owner, Token constraint)
		{
			Owner = owner;
			Constraint = constraint;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the owner.
		/// </summary>
		/// <value>
		/// The owner.
		/// </value>
		public Token Owner { get; private set; }

		/// <summary>
		/// Gets the constraint.
		/// </summary>
		/// <value>
		/// The constraint.
		/// </value>
		public Token Constraint { get; private set; }

		#endregion Properties
	}
}