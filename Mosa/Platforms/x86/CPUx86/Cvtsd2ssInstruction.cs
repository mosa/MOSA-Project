/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *
 */

using System;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;

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
		/// Computes the opcode.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <param name="third">The third operand.</param>
		/// <returns></returns>
        protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
        {
			if ((destination is RegisterOperand) && (source is RegisterOperand)) return R_R;
			if ((destination is RegisterOperand) && (source is MemoryOperand)) return R_M;
            throw new ArgumentException(@"No opcode for operand type.");
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
