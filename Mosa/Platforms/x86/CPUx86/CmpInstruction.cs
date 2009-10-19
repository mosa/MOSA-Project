/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.CPUx86
{
    /// <summary>
    /// Intermediate representation of the x86 cmp instruction.
    /// </summary>
    public sealed class CmpInstruction : TwoOperandInstruction
    {
        #region Data Member
        private static readonly OpCode M_R = new OpCode(new byte[] { 0x39 });
        private static readonly OpCode R_M = new OpCode(new byte[] { 0x3B });
        private static readonly OpCode R_R = new OpCode(new byte[] { 0x3B });
        private static readonly OpCode M_C = new OpCode(new byte[] { 0x81 }, 7);
        private static readonly OpCode R_C = new OpCode(new byte[] { 0x81 }, 7);
        private static readonly OpCode R_C_8 = new OpCode(new byte[] { 0x80 }, 7);
        private static readonly OpCode M_R_8 = new OpCode(new byte[] { 0x38 });
        private static readonly OpCode R_M_8 = new OpCode(new byte[] { 0x3A });
        private static readonly OpCode M_R_16 = new OpCode(new byte[] { 0x66, 0x39 });
        private static readonly OpCode R_M_16 = new OpCode(new byte[] { 0x66, 0x3B });
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
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <param name="third">The third operand.</param>
		/// <returns></returns>
        protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
        {
			if ((destination is MemoryOperand) && (source is RegisterOperand))
            {
				if (IsByte(destination) || IsByte(source))
                    return M_R_8;
				if (IsChar(destination) || IsChar(source))
                    return M_R_16;
                return M_R;
            }

			if ((destination is RegisterOperand) && (source is MemoryOperand))
            {
				if (IsByte(source) || IsByte(destination))
                    return R_M_8;
				if (IsChar(source) || IsShort(source))
                    return R_M_16;
                return R_M;
            }

			if ((destination is RegisterOperand) && (source is RegisterOperand)) return R_R;
			if ((destination is MemoryOperand) && (source is ConstantOperand)) return M_C;
			if ((destination is RegisterOperand) && (source is ConstantOperand))
            {
				if (IsByte(source) || IsByte(destination))
                    return R_C_8;
                return R_C;
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
			visitor.Cmp(context);
		}

        #endregion // Methods
    }
}
