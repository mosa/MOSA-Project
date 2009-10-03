/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *
 */

using System;

using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.CPUx86
{
    /// <summary>
    /// Intermediate representation for the x86 comisd instruction.
    /// </summary>
    public class ComissInstruction : TwoOperandInstruction
    {
        #region Data Members
        private static readonly OpCode R_R = new OpCode(new byte[] { 0x0F, 0x2F });
        private static readonly OpCode R_M = new OpCode(new byte[] { 0x0F, 0x2F });
        private static readonly OpCode R_L = new OpCode(new byte[] { 0x0F, 0x2F });
        private static readonly OpCode R_C = new OpCode(new byte[] { 0x0F, 0x2F });
        #endregion

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
            if ((dest is RegisterOperand) && (src is RegisterOperand)) return R_R;
            if ((dest is RegisterOperand) && (src is MemoryOperand)) return R_M;
            if ((dest is RegisterOperand) && (src is LabelOperand)) return R_L;
            if ((dest is RegisterOperand) && (src is ConstantOperand)) return R_C;
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
            return String.Format("x86.comiss {0}, {1} ; {0} == {1}", context.Operand1, context.Operand2);
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="context">The context.</param>
        public override void Visit(IX86Visitor visitor, Context context)
        {
            visitor.Comisd(context);
        }

        #endregion // Methods
    }
}
