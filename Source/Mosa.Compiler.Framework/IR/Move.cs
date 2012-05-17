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
	/// Intermediate representation of an arbitrary move context.
	/// </summary>
	public sealed class Move : TwoOperandInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Move"/>.
		/// </summary>
		public Move()
		{
		}

		#endregion // Construction

		#region TwoOperandInstruction Overrides

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.Move(context);
		}

		#endregion // TwoOperandInstruction Overrides
	}
}
