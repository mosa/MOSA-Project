// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Return Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.UnaryInstruction" />
	public sealed class ReturnInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ReturnInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public ReturnInstruction(OpCode opcode)
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
		public override FlowControl FlowControl { get { return FlowControl.Return; } }

		#endregion Properties

		#region Methods

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="node">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		/// <exception cref="ArgumentException">Invalid opcode. - codeReader</exception>
		public override void Decode(InstructionNode node, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(node, decoder);

			if (OpCode.Ret != opcode)
				throw new ArgumentException("Invalid opcode.", "codeReader");

			if (decoder.Method.Signature.ReturnType.IsVoid)
				node.OperandCount = 0;
			else
				node.OperandCount = 1;

			var block = decoder.GetBlock(BasicBlock.EpilogueLabel);

			node.AddBranchTarget(block);
		}

		#endregion Methods
	}
}
