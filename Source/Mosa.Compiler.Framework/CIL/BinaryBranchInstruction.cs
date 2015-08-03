// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	///
	/// </summary>
	public sealed class BinaryBranchInstruction : BinaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryBranchInstruction"/> class.
		/// </summary>
		/// <param name="opCode">The opcode.</param>
		public BinaryBranchInstruction(OpCode opCode)
			: base(opCode)
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
		public override FlowControl FlowControl { get { return FlowControl.ConditionalBranch; } }

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
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(InstructionNode ctx, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ctx, decoder);

			var block = decoder.GetBlock((int)decoder.Instruction.Operand);

			ctx.AddBranchTarget(block);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.BinaryBranch(context);
		}

		/// <summary>
		/// Gets the instruction modifier.
		/// </summary>
		/// <param name="node">The context.</param>
		/// <returns></returns>
		protected override string GetModifier(InstructionNode node)
		{
			switch (((node.Instruction) as CIL.BaseCILInstruction).OpCode)
			{
				case OpCode.Beq_s: return @"==";
				case OpCode.Beq: return @"==";
				case OpCode.Bge_s: return @">=";
				case OpCode.Bge: return @">=";
				case OpCode.Bge_un_s: return @">= unordered";
				case OpCode.Bge_un: return @">= unordered";
				case OpCode.Bgt_s: return @">";
				case OpCode.Bgt: return @">";
				case OpCode.Bgt_un_s: return @"> unordered";
				case OpCode.Bgt_un: return @"> unordered";
				case OpCode.Ble_s: return @"<=";
				case OpCode.Ble: return @"<=";
				case OpCode.Ble_un_s: return @"<= unordered";
				case OpCode.Ble_un: return @"<= unordered";
				case OpCode.Blt_s: return @"<";
				case OpCode.Blt: return @"<";
				case OpCode.Blt_un_s: return @"< unordered";
				case OpCode.Blt_un: return @"< unordered";
				case OpCode.Bne_un_s: return @"!= unordered";
				case OpCode.Bne_un: return @"!= unordered";
				default: throw new InvalidOperationException(@"Opcode not set.");
			}
		}

		#endregion Methods
	}
}
