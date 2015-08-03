// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	///
	/// </summary>
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
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(InstructionNode ctx, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ctx, decoder);

			if (OpCode.Ret != opcode)
				throw new ArgumentException(@"Invalid opcode.", @"codeReader");

			if (decoder.Method.Signature.ReturnType.IsVoid)
				ctx.OperandCount = 0;
			else
				ctx.OperandCount = 1;

			var block = decoder.GetBlock(BasicBlock.EpilogueLabel);

			ctx.AddBranchTarget(block);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Ret(context);
		}

		#endregion Methods
	}
}