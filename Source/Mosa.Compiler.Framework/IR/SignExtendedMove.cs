/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */


namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Intermediate representation of a signed conversion context.
	/// </summary>
	/// <remarks>
	/// This instruction takes the source operand and converts to the request size maintaining its sign.
	/// </remarks>
	public sealed class SignExtendedMove : TwoOperandInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="SignExtendedMove"/>.
		/// </summary>
		public SignExtendedMove()
		{
		}

		#endregion // Construction

		#region TwoOperandInstruction Overrides

		/// <summary>
		/// Implementation of the visitor pattern.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.SignExtendedMove(context);
		}

		#endregion // TwoOperandInstruction Overrides
	}
}
