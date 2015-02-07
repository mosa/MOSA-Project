/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	///
	/// </summary>
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

		public override FlowControl FlowControl { get { return FlowControl.Leave; } }

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

			ctx.SetCILBranch((int)decoder.Instruction.Operand);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Leave(context);
		}

		#endregion Methods
	}
}