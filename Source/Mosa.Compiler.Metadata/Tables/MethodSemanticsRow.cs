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
	public struct MethodSemanticsRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private MethodSemanticsAttributes _semantics;

		/// <summary>
		/// 
		/// </summary>
		private Token _method;

		/// <summary>
		/// 
		/// </summary>
		private Token _association;

		#endregion // Data members

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
			_semantics = semantics;
			_method = method;
			_association = association;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the semantics.
		/// </summary>
		/// <value>The semantics.</value>
		public MethodSemanticsAttributes Semantics
		{
			get { return _semantics; }
		}

		/// <summary>
		/// Gets the method.
		/// </summary>
		/// <value>The method.</value>
		public Token Method
		{
			get { return _method; }
		}

		/// <summary>
		/// Gets the association.
		/// </summary>
		/// <value>The association.</value>
		public Token Association
		{
			get { return _association; }
		}

		#endregion // Properties
	}
}
