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
	public class MethodSemanticsRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="MethodSemanticsRow"/> struct.
		/// </summary>
		/// <param name="semantics">The semantics.</param>
		/// <param name="method">The method.</param>
		/// <param name="association">The association.</param>
		public MethodSemanticsRow(MethodSemanticsAttributes semantics, Token method,
									Token association)
		{
			Semantics = semantics;
			Method = method;
			Association = association;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the semantics.
		/// </summary>
		/// <value>The semantics.</value>
		public MethodSemanticsAttributes Semantics { get; private set; }

		/// <summary>
		/// Gets the method.
		/// </summary>
		/// <value>The method.</value>
		public Token Method { get; private set; }

		/// <summary>
		/// Gets the association.
		/// </summary>
		/// <value>The association.</value>
		public Token Association { get; private set; }

		#endregion Properties
	}
}