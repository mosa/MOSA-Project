/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86.CPUx86
{
    /// <summary>
    /// Intermediate representation of the x86 xor instruction.
    /// </summary>
    public sealed class LogicalXorInstruction : TwoOperandInstruction
    {
        #region Construction
        private static readonly OpCode R_C = new OpCode(new byte[] { 0x81 }, 6);
        private static readonly OpCode R_M = new OpCode(new byte[] { 0x33 });
        private static readonly OpCode R_R = new OpCode(new byte[] { 0x33 });
        private static readonly OpCode M_R = new OpCode(new byte[] { 0x31 });
        #endregion // Construction

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        /// <param name="third"></param>
        /// <returns></returns>
        protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
        {
            if ((destination is RegisterOperand) && (source is ConstantOperand))
                return R_C;

            if ((destination is RegisterOperand) && (source is MemoryOperand))
                return R_M;

            if ((destination is RegisterOperand) && (source is RegisterOperand))
                return R_R;

            if ((destination is MemoryOperand) && (source is RegisterOperand))
                return M_R;

            throw new ArgumentException(@"No opcode for operand type.");
        }

        /// <summary>
        /// Returns a string representation of the instruction.
        /// </summary>
        /// <returns>
        /// A string representation of the instruction in intermediate form.
        /// </returns>
        public override string ToString(Context context)
        {
            return String.Format(@"x86.xor {0}, {1} ; {0} ^= {1}", context.Operand1, context.Operand2);
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Xor(context);
		}

        #endregion // Methods
    }
}
