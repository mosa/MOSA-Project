// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Branch Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.BaseCILInstruction" />
	public class BranchInstruction : BaseCILInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BranchInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public BranchInstruction(OpCode opcode)
			: base(opcode, 0)
		{
		}

		#endregion Construction

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
		public override FlowControl FlowControl { get { return FlowControl.UnconditionalBranch; } }

		#endregion Properties

		#region Methods

		public override bool DecodeTargets(IInstructionDecoder decoder)
		{
			decoder.GetBlock((int)decoder.Instruction.Operand);
			return true;
		}

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="node">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(InstructionNode node, IInstructionDecoder decoder)
		{
			// Decode bases first
			base.Decode(node, decoder);

			var block = decoder.GetBlock((int)decoder.Instruction.Operand);

			node.AddBranchTarget(block);
		}

		#endregion Methods
	}
}
