/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using Mosa.Runtime.CompilerFramework.Operands;

namespace Mosa.Runtime.CompilerFramework.IR
{
	/// <summary>
	/// Intermediate representation of a method return context.
	/// </summary>
	public sealed class ReturnInstruction : OneOperandInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="ReturnInstruction"/>.
		/// </summary>
		public ReturnInstruction()
		{
		}

		#endregion // Construction

		#region Overrides

		/// <summary>
		/// Determines flow behavior of this context.
		/// </summary>
		/// <value></value>
		/// <remarks>
		/// Knowledge of control flow is required for correct basic block
		/// building. Any instruction that alters the control flow must override
		/// this property and correctly identify its control flow modifications.
		/// </remarks>
		public override FlowControl FlowControl
		{
			get { return FlowControl.Branch; }
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.ReturnInstruction(context);
		}

		#endregion // OneOperandInstruction Overrides

	}
}
