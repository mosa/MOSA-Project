// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	public class Switch : BaseIRInstruction
	{
		#region Properties

		/// <summary>
		/// Determines flow behavior of this instruction.
		/// </summary>
		/// <value></value>
		/// <remarks>
		/// Knowledge of control flow is required for correct basic block
		/// building. Any instruction that alters the control flow must override
		/// this property and correctly identify its control flow modifications.
		/// </remarks>
		public override FlowControl FlowControl { get { return FlowControl.Switch; } }

		#endregion Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Switch" /> class.
		/// </summary>
		public Switch()
			: base(0, 0)
		{
		}

		#endregion Construction
	}
}
