/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;

namespace Mosa.Platforms.x86.CPUx86
{
	/// <summary>
	/// Representations the x86 out instruction.
	/// </summary>
	public sealed class OutInstruction : ThreeOperandInstruction, IIntrinsicInstruction
	{
		#region Data Members

		private static readonly OpCode C_R_8 = new OpCode(new byte[] { 0xE6 });
		private static readonly OpCode R_R_8 = new OpCode(new byte[] { 0xEE });
		private static readonly OpCode C_R_32 = new OpCode(new byte[] { 0xE7 });
		private static readonly OpCode R_R_32 = new OpCode(new byte[] { 0xEF });

		#endregion // Data Members

		#region Methods
		/// <summary>
		/// Computes the opcode.
		/// </summary>
		/// <param name="empty">The empty.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		protected override OpCode ComputeOpCode(Operand empty, Operand destination, Operand source)
		{
			if (IsByte(source)) {
				if ((destination is ConstantOperand) && (source is RegisterOperand)) return C_R_8;
				if ((destination is RegisterOperand) && (source is RegisterOperand)) return R_R_8;
			}
			else {
				if ((destination is ConstantOperand) && (source is RegisterOperand)) return C_R_32;
				if ((destination is RegisterOperand) && (source is RegisterOperand)) return R_R_32;
			}
			throw new ArgumentException(@"No opcode for operand type.");
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="emitter"></param>
        protected override void Emit(Context context, MachineCodeEmitter emitter)
        {
            emitter.Emit(new OpCode(new byte[] { 0xEE }), null, null);
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Out(context);
		}

		/// <summary>
		/// Replaces the instrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		public void ReplaceIntrinsicCall(Context context)
		{
			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;

			RegisterOperand edx = new RegisterOperand(operand1.Type, GeneralPurposeRegister.EDX);
			RegisterOperand eax = new RegisterOperand(operand2.Type, GeneralPurposeRegister.EAX);

			context.SetInstruction(CPUx86.Instruction.MovInstruction, edx, operand1);
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, eax, operand2);
			context.AppendInstruction(CPUx86.Instruction.OutInstruction, null, edx, eax);
		}

		#endregion // Methods
	}
}
