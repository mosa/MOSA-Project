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
	/// Intermediate representation of the div instruction.
	/// </summary>
	public sealed class DirectDivisionInstruction : OneOperandInstruction
    {
        #region Data Members
        private static readonly OpCode R = new OpCode(new byte[] { 0xF7 }, 6);
	    private static readonly OpCode M = new OpCode(new byte[] { 0xF7 }, 6);
        #endregion

        #region OneOperandInstruction Overrides

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <param name="thirdOperand"></param>
        /// <returns></returns>
        protected override OpCode ComputeOpCode(Operand dest, Operand src, Operand thirdOperand)
        {
            if (src is RegisterOperand) return R;
            if (src is MemoryOperand) return M;

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
			return String.Format(@"x86.div {0} ; edx:eax /= {0}", context.Operand1);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.DirectDivision(context);
		}

		#endregion // Methods
	}
}
