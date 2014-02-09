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
	/// Specifies a function pointer signature type.
	/// </summary>
	public sealed class FnptrSigType : SigType
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="FnptrSigType"/> class.
		/// </summary>
		/// <param name="token">The token.</param>
		public FnptrSigType(HeapIndexToken token) :
			base(CilElementType.FunctionPtr)
		{
			Token = token;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the method definition, reference or specification token.
		/// </summary>
		/// <value>The token.</value>
		public HeapIndexToken Token { get; private set; }

		#endregion Properties
	}
}