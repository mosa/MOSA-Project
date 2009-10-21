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

namespace Mosa.Platforms.x86.CPUx86
{
    /// <summary>
    /// Representations the x86 out instruction.
    /// </summary>
    public sealed class OutInstruction : ThreeOperandInstruction
    {
        #region Codes
        private static OpCode C_R_8 = new OpCode(new byte[] { 0xE6 });
        private static OpCode R_R_8 = new OpCode(new byte[] { 0xEE });
        private static OpCode C_R_32 = new OpCode(new byte[] { 0xE7 });
        private static OpCode R_R_32 = new OpCode(new byte[] { 0xEF });
        #endregion 

        #region Methods
		/// <summary>
		/// Computes the op code.
		/// </summary>
		/// <param name="empty">The empty.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="source">The source.</param>
		/// <returns></returns>
        protected override OpCode ComputeOpCode(Operand empty, Operand destination, Operand source)
        {
            if (IsByte(source))
            {
                if ((destination is ConstantOperand) && (source is RegisterOperand)) return C_R_8;
                if ((destination is RegisterOperand) && (source is RegisterOperand)) return R_R_8;
            }
            else
            {
                if ((destination is ConstantOperand) && (source is RegisterOperand)) return C_R_32;
                if ((destination is RegisterOperand) && (source is RegisterOperand)) return R_R_32;
            }
            throw new ArgumentException(@"No opcode for operand type.");
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

        #endregion // Methods
    }
}
