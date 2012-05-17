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
	/// Intermediate representation of the and instruction.
	/// </summary>
	public sealed class LogicalAnd : ThreeOperandInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LogicalAnd"/> class.
		/// </summary>
		public LogicalAnd()
		{
		}

		#endregion // Construction

		#region Instruction Overrides

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.LogicalAnd(context);
		}

		#endregion // Instruction Overrides
	}
}
