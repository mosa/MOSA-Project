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
    /// Intermediate representation of the x86 rotate right instruction.
    /// </summary>
    public sealed class RcrInstruction : TwoOperandInstruction
    {
        #region Codes
        private static readonly OpCode R = new OpCode(new byte[] { 0xD1 }, 3);
        private static readonly OpCode M = new OpCode(new byte[] { 0xD1 }, 3);
        #endregion

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
            if (destination is RegisterOperand) return R;
            if (destination is MemoryOperand) return M;
            throw new ArgumentException(@"No opcode for operand type.");
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Rcr(context);
		}

        #endregion // Methods
    }
}
