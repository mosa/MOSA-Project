/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 */

using System;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.CPUx86
{
	/// <summary>
	/// Intermediate representation of the x86 adc instruction.
	/// </summary>
	public sealed class AdcInstruction : TwoOperandInstruction
    {
        #region Data members
        private static readonly OpCode R_C = new OpCode(new byte[] { 0x81 });
        private static readonly OpCode R_R = new OpCode(new byte[] { 0x11 });
        private static readonly OpCode R_M = new OpCode(new byte[] { 0x13 });
        private static readonly OpCode M_R = new OpCode(new byte[] { 0x11 });
        #endregion

        #region Properties

        /// <summary>
		/// Gets the instruction latency.
		/// </summary>
		/// <value>The latency.</value>
		public override int Latency { get { return 1; } }

		#endregion // Properties

		#region Methods

        private static OpCode Adc(Operand dest, Operand src)
        {
            if ((dest is RegisterOperand) && (src is ConstantOperand)) return R_C;
            if ((dest is RegisterOperand) && (src is RegisterOperand)) return R_R;
            if ((dest is RegisterOperand) && (src is MemoryOperand)) return R_M;
            if ((dest is MemoryOperand) && (src is RegisterOperand)) return M_R;
            throw new ArgumentException(@"No opcode for operand type.");
        }

        /// <summary>
        /// Emits the specified platform instruction.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="codeStream">The code stream.</param>
        public override void Emit(Context ctx, System.IO.Stream codeStream)
        {
            OpCode opcode = Adc(ctx.Result, ctx.Operand1);
            MachineCodeEmitter.Emit(codeStream, opcode, ctx.Result, ctx.Operand1);
        }

		/// <summary>
		/// Returns a string representation of the instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString(Context context)
		{
			return String.Format("x86.adc {0}, {1} ; {0} = {0} + {1} + carry-flag", context.Operand1, context.Operand2);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Adc(context);
		}

		#endregion // Methods
	}
}
