using System;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Platforms.x86.CPUx86.Intrinsics
{
    /// <summary>
    /// Intermediate representation of the x86 CPUID instruction.
    /// </summary>
    public sealed class BochsDebug : BaseInstruction
    {
		/// <summary>
		/// Computes the op code.
		/// </summary>
		/// <param name="dest">The destination.</param>
		/// <param name="src">The source.</param>
		/// <param name="thirdOperand">The third operand.</param>
		/// <returns></returns>
        protected override OpCode ComputeOpCode(Operand dest, Operand src, Operand thirdOperand)
        {
            return new OpCode(new byte[] { 0x66, 0x87 });
        }

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="codeStream">The code stream.</param>
        public override void Emit(Context ctx, System.IO.Stream codeStream)
        {
            ctx.Result = new RegisterOperand(new SigType(Runtime.Metadata.CilElementType.I2), GeneralPurposeRegister.EBX);
            ctx.Operand1 = new RegisterOperand(new SigType(Runtime.Metadata.CilElementType.I2), GeneralPurposeRegister.EBX);
            base.Emit(ctx, codeStream);
        }
        /// <summary>
        /// Returns a string representation of the instruction.
        /// </summary>
        /// <returns>
        /// A string representation of the instruction in intermediate form.
        /// </returns>
        public override string ToString()
        {
            return String.Format(@"x86.xchg bx, bx");
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.BochsDebug(context);
		}

    }
}
