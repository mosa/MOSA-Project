/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Metadata.Signatures
{
	public class TypeSigType : SigType
	{
		/// <summary>
		/// Gets the token.
		/// </summary>
		/// <value>
		/// The token.
		/// </value>
		public Token Token { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeSigType"/> class.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <param name="type">The type.</param>
		public TypeSigType(Token token, CilElementType type) :
			base(type)
		{
			Token = token;
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			return base.ToString() + " " + Token.ToString();
		}
	}
}