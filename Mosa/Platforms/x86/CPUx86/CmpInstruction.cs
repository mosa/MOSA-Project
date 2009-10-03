/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
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
		/// <param name="dest">The destination.</param>
		/// <param name="src">The source.</param>
		/// <param name="third">The third.</param>
		/// <returns></returns>
        protected override OpCode ComputeOpCode(Operand dest, Operand src, Operand third)
        {
            if ((dest is MemoryOperand) && (src is RegisterOperand))
            {
                if (IsByte(dest) || IsByte(src))
                    return M_R_8;
                if (IsChar(dest) || IsChar(src))
                    return M_R_16;
                return M_R;
            }

            if ((dest is RegisterOperand) && (src is MemoryOperand))
            {
                if (IsByte(src) || IsByte(dest))
                    return R_M_8;
                if (IsChar(src) || IsShort(src))
                    return R_M_16;
                return R_M;
            }

            if ((dest is RegisterOperand) && (src is RegisterOperand)) return R_R;
            if ((dest is MemoryOperand) && (src is ConstantOperand)) return M_C;
            if ((dest is RegisterOperand) && (src is ConstantOperand))
            {
                if (IsByte(src) || IsByte(dest))
                    return R_C_8;
                return R_C;
            }
            throw new ArgumentException(@"No opcode for operand type.");
        }

		/// <summary>
		/// Returns a string representation of the instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>
		/// A string representation of the instruction in intermediate form.
		/// </returns>
        public override string ToString(Context context)
        {
            return String.Format(@"x86.cmp {0}, {1}", context.Operand1, context.Operand2);
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
