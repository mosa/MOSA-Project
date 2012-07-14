/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;


namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Represents a unary branch instruction in internal representation.
	/// </summary>
	/// <remarks>
	/// This instruction is used to represent brfalse[.s] and brtrue[.s].
	/// </remarks>
	public class UnaryBranchInstruction : UnaryInstruction, IBranchInstruction
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

		#endregion // Construction

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

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(Context ctx, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ctx, decoder);

			// Read the branch target
			// Is this a short branch target?
			if (opcode == OpCode.Brfalse_s || opcode == OpCode.Brtrue_s)
			{
				sbyte target = decoder.DecodeSByte();
				ctx.SetBranch(target);
			}
			else if (opcode == OpCode.Brfalse || opcode == OpCode.Brtrue)
			{
				int target = decoder.DecodeInt();
				ctx.SetBranch(target);
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
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.UnaryBranch(context);
		}

		/// <summary>
		/// Gets the instruction modifier.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		protected override string GetModifier(Context context)
		{
			OpCode opCode = ((context.Instruction) as CIL.BaseCILInstruction).OpCode;
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

		/// <summary>
		/// Determines if the branch is conditional.
		/// </summary>
		/// <value></value>
		public bool IsConditional { get { return true; } }

	}
}
