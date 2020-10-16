// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Intermediate representation for the IL switch instruction.
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.UnaryBranchInstruction" />
	public sealed class SwitchInstruction : UnaryBranchInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="SwitchInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public SwitchInstruction(OpCode opcode)
			: base(opcode)
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
		public override FlowControl FlowControl { get { return FlowControl.Switch; } }

		#endregion Properties

		#region Methods Overrides

		public override bool DecodeTargets(IInstructionDecoder decoder)
		{
			foreach (var target in (int[])decoder.Instruction.Operand)
			{
				var block = decoder.GetBlock(target);
			}

			decoder.GetBlock(decoder.Instruction.Next.Value);
			return true;
		}

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="node">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(InstructionNode node, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(node, decoder);

			foreach (var target in (int[])decoder.Instruction.Operand)
			{
				var block = decoder.GetBlock(target);

				node.AddBranchTarget(block);
			}

			node.AddBranchTarget(decoder.GetBlock(decoder.Instruction.Next.Value));
		}

		#endregion Methods Overrides
	}
}
