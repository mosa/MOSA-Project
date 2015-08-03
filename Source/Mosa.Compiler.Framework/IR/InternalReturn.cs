// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Intermediate representation of a method return context.
	/// </summary>
	public sealed class InternalReturn : OneOperandInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Return"/>.
		/// </summary>
		public InternalReturn()
		{
		}

		#endregion Construction

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
		public override FlowControl FlowControl { get { return FlowControl.UnconditionalBranch; } }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.InternalReturn(context);
		}

		#endregion Overrides
	}
}