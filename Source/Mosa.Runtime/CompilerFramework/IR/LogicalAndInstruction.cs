/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */


namespace Mosa.Runtime.CompilerFramework.IR
{
	/// <summary>
	/// Intermediate representation of the and instruction.
	/// </summary>
	public sealed class LogicalAndInstruction : ThreeOperandInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LogicalAndInstruction"/> class.
		/// </summary>
		public LogicalAndInstruction()
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
			visitor.LogicalAndInstruction(context);
		}

		#endregion // Instruction Overrides
	}
}
