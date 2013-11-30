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
	public class MethodImplRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="MethodImplRow"/> struct.
		/// </summary>
		/// <param name="class">The @class.</param>
		/// <param name="methodBody">The method body.</param>
		/// <param name="methodDeclaration">The method declaration.</param>
		public MethodImplRow(Token @class, Token methodBody, Token methodDeclaration)
		{
			Class = @class;
			MethodBody = methodBody;
			MethodDeclaration = methodDeclaration;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the class.
		/// </summary>
		/// <value>
		/// The class.
		/// </value>
		public Token Class { get; private set; }

		/// <summary>
		/// Gets the method body .
		/// </summary>
		/// <value>The method body.</value>
		public Token MethodBody { get; private set; }

		/// <summary>
		/// Gets the method declaration.
		/// </summary>
		/// <value>The method declaration.</value>
		public Token MethodDeclaration { get; private set; }

		#endregion Properties
	}
}