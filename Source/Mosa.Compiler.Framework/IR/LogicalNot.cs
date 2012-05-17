/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */


namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Intermediate representation of the not instruction.
	/// </summary>
	public sealed class LogicalNot : TwoOperandInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="LogicalNot"/>.
		/// </summary>
		public LogicalNot()
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
			visitor.LogicalNot(context);
		}

		#endregion // Instruction Overrides
	}
}
