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
	public struct MethodImplRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private Token @class;

		/// <summary>
		/// 
		/// </summary>
		private Token methodBody;

		/// <summary>
		/// 
		/// </summary>
		private Token methodDeclaration;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="MethodImplRow"/> struct.
		/// </summary>
		/// <param name="class">The @class.</param>
		/// <param name="methodBody">The method body.</param>
		/// <param name="methodDeclaration">The method declaration.</param>
		public MethodImplRow(Token @class, Token methodBody, Token methodDeclaration)
		{
			this.@class = @class;
			this.methodBody = methodBody;
			this.methodDeclaration = methodDeclaration;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the class.
		/// </summary>
		/// <value>The class table idx.</value>
		public Token @Class
		{
			get { return @class; }
		}

		/// <summary>
		/// Gets the method body .
		/// </summary>
		/// <value>The method body.</value>
		public Token MethodBody
		{
			get { return methodBody; }
		}

		/// <summary>
		/// Gets the method declaration.
		/// </summary>
		/// <value>The method declaration.</value>
		public Token MethodDeclaration
		{
			get { return methodDeclaration; }
		}

		#endregion // Properties
	}
}
