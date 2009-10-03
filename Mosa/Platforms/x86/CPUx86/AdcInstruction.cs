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

		/// <summary>
		/// Computes the op code.
		/// </summary>
		/// <param name="dest">The destination.</param>
		/// <param name="src">The source.</param>
		/// <param name="third">The third.</param>
		/// <returns></returns>
        protected override OpCode ComputeOpCode(Operand dest, Operand src, Operand third)
        {
            if ((dest is RegisterOperand) && (src is ConstantOperand)) return R_C;
            if ((dest is RegisterOperand) && (src is RegisterOperand)) return R_R;
            if ((dest is RegisterOperand) && (src is MemoryOperand)) return R_M;
            if ((dest is MemoryOperand) && (src is RegisterOperand)) return M_R;
            throw new ArgumentException(@"No opcode for operand type.");
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
