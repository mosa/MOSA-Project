/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Compiler.Metadata.Signatures
{
	/// <summary>
	/// Represents a value type in a signature.
	/// </summary>
	public sealed class ValueTypeSigType : TypeSigType
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueTypeSigType"/> class.
		/// </summary>
		/// <param name="token">The type definition, reference or specification token of the value type.</param>
		public ValueTypeSigType(Token token)
			: base(token, CilElementType.ValueType)
		{
		}

		#endregion Construction
	}
}