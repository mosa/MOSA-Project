// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Intermediate representation of a method return context.
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseOneOperandInstruction" />
	public sealed class SetReturn : BaseOneOperandInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="SetReturn" />.
		/// </summary>
		public SetReturn()
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
		public override FlowControl FlowControl { get { return FlowControl.Return; } }

		#endregion Overrides
	}
}
