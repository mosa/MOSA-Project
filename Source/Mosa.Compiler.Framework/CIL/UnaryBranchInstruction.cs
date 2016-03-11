// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Represents a unary branch instruction in internal representation.
	/// </summary>
	/// <remarks>
	/// This instruction is used to represent brfalse[.s] and brtrue[.s].
	/// </remarks>
	public class UnaryBranchInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="UnaryBranchInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public UnaryBranchInstruction(OpCode opcode)
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
		public override FlowControl FlowControl { get { return FlowControl.ConditionalBranch; } }

		#endregion Properties

		#region Methods

		public override bool DecodeTargets(IInstructionDecoder decoder)
		{
			if (opcode == OpCode.Brfalse_s || opcode == OpCode.Brtrue_s ||
				opcode == OpCode.Brfalse || opcode == OpCode.Brtrue)
			{
				decoder.GetBlock((int)decoder.Instruction.Operand);
				return true;
			}
			else if (opcode == OpCode.Switch)
			{
				return base.DecodeTargets(decoder);
			}

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

			// Read the branch target
			// Is this a short branch target?
			if (opcode == OpCode.Brfalse_s || opcode == OpCode.Brtrue_s ||
				opcode == OpCode.Brfalse || opcode == OpCode.Brtrue)
			{
				var block = decoder.GetBlock((int)decoder.Instruction.Operand);

				ctx.AddBranchTarget(block);
			}
			else if (opcode == OpCode.Switch)
			{
				// Don't do anything, the derived class will do everything
			}
			else
			{
				throw new NotSupportedException(@"Invalid opcode " + opcode.ToString() + " specified for UnaryBranchInstruction.");
			}
		}

		/// <summary>
		/// Gets the instruction modifier.
		/// </summary>
		/// <param name="node">The context.</param>
		/// <returns></returns>
		protected override string GetModifier(InstructionNode node)
		{
			OpCode opCode = ((node.Instruction) as CIL.BaseCILInstruction).OpCode;
			switch (opCode)
			{
				case OpCode.Brtrue: return @"true";
				case OpCode.Brtrue_s: return @"true";
				case OpCode.Brfalse: return @"false";
				case OpCode.Brfalse_s: return @"false";
				case OpCode.Switch: return @"switch";
				default: throw new InvalidOperationException(@"Opcode not set.");
			}
		}

		#endregion Methods
	}
}
