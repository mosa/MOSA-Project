/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;

namespace Mosa.Platforms.x86.CPUx86
{
    /// <summary>
    /// Representations the x86 push instruction.
    /// </summary>
	public sealed class SetRC0Instruction : OneOperandInstruction, IIntrinsicInstruction
    {
		#region Data Members

		// TODO
		private static readonly OpCode opcode = new OpCode(new byte[] { 0x0F, 0x01 }, 7);

		#endregion // Data Members

        #region Methods
		
		/// <summary>
		/// Computes the opcode.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <param name="third">The third operand.</param>
		/// <returns></returns>
        protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
        {
			return opcode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="emitter"></param>
        protected override void Emit(Context ctx, MachineCodeEmitter emitter)
        {
            OpCode code = ComputeOpCode(ctx.Result, ctx.Operand1, ctx.Operand2);
            emitter.Emit(code, ctx.Operand1, null);
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			//visitor.Invlpg(context);
		}

		/// <summary>
		/// Replaces the instrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		public void ReplaceIntrinsicCall(Context context)
		{
			// TODO
			//context.SetInstruction(CPUx86.Instruction.InvlpgInstruction, null, context.Operand1);
		}

        #endregion // Methods
    }
}
