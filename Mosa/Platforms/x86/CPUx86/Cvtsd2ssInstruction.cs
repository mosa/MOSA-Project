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
    /// Intermediate representation for the x86 cvtsd2ss instruction.
    /// </summary>
    public class Cvtsd2ssInstruction : TwoOperandInstruction
    {
        #region Data Members
        private static readonly OpCode R_R = new OpCode(new byte[] { 0xF2, 0x0F, 0x5A });
        private static readonly OpCode R_M = new OpCode(new byte[] { 0xF2, 0x0F, 0x5A });
        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <param name="thirdOperand"></param>
        /// <returns></returns>
        protected override OpCode ComputeOpCode(Operand dest, Operand src, Operand thirdOperand)
        {
            if ((dest is RegisterOperand) && (src is RegisterOperand)) return R_R;
            if ((dest is RegisterOperand) && (src is MemoryOperand)) return R_M;
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
            return String.Format("x86.cvtsd2ss {0}, {1} ; {0} = (float){1}", context.Operand1, context.Operand2);
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Cvtsd2ss(context);
		}

        #endregion // Methods
    }
}
