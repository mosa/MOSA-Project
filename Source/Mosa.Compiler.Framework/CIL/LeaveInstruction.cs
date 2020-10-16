// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Leave Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.BranchInstruction" />
	public sealed class LeaveInstruction : BranchInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LeaveInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public LeaveInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Determines flow behavior of this instruction.
		/// </summary>
		/// <remarks>
		/// Knowledge of control flow is required for correct basic block
		/// building. Any instruction that alters the control flow must override
		/// this property and correctly identify its control flow modifications.
		/// </remarks>
		public override FlowControl FlowControl { get { return FlowControl.Leave; } }

		/// <summary>
		/// Gets a value indicating whether to [ignore instruction's basic block].
		/// </summary>
		/// <value>
		/// <c>true</c> if [ignore instruction basic block]; otherwise, <c>false</c>.
		/// </value>
		public override bool IgnoreInstructionBasicBlockTargets { get { return true; } }

		#endregion Properties

		#region Methods

		public override bool DecodeTargets(IInstructionDecoder decoder)
		{
			decoder.GetBlock((int)decoder.Instruction.Operand);
			return true;
		}

		#endregion Methods
	}
}
