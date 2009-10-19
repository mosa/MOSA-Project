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

namespace Mosa.Platforms.x86.CPUx86
{
    /// <summary>
    /// Intermediate representation for the x86 comisd instruction.
    /// </summary>
    public class ComisdInstruction : TwoOperandInstruction
    {
        #region Data Members
        private static readonly OpCode R_R = new OpCode(new byte[] { 0x66, 0x0F, 0x2F });
        private static readonly OpCode R_M = new OpCode(new byte[] { 0x66, 0x0F, 0x2F });
        private static readonly OpCode R_L = new OpCode(new byte[] { 0x66, 0x0F, 0x2F });
        private static readonly OpCode R_C = new OpCode(new byte[] { 0x66, 0x0F, 0x2F });
        #endregion

        #region Methods

		/// <summary>
		/// Computes the op code.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <param name="third">The third operand.</param>
		/// <returns></returns>
        protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
        {
			if ((destination is RegisterOperand) && (source is RegisterOperand)) return R_R;
			if ((destination is RegisterOperand) && (source is MemoryOperand)) return R_M;
			if ((destination is RegisterOperand) && (source is LabelOperand)) return R_L;
			if ((destination is RegisterOperand) && (source is ConstantOperand)) return R_C;
            throw new ArgumentException(@"No opcode for operand type.");
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
