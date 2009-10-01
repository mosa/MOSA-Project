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
    /// Intermediate representation of the x86 move instruction.
    /// </summary>
	public sealed class MoveInstruction : TwoOperandInstruction
    {
        #region Data Members

        private static readonly OpCode R_C = new OpCode(new byte[] { 0xC7 }, 0); // Move imm32 to r/m32
        private static readonly OpCode M_C = new OpCode(new byte[] { 0xC7 }, 0);
        private static readonly OpCode R_R = new OpCode(new byte[] { 0x8B });
        private static readonly OpCode R_R_16 = new OpCode(new byte[] { 0x66, 0x8B });
        private static readonly OpCode R_R_U8 = new OpCode(new byte[] { 0x88 });
        private static readonly OpCode R_M = new OpCode(new byte[] { 0x8B }); // Move r/m32 to R32
        private static readonly OpCode R_M_16 = new OpCode(new byte[] { 0x66, 0x8B });
        private static readonly OpCode M_R = new OpCode(new byte[] { 0x89 });
        private static readonly OpCode M_R_16 = new OpCode(new byte[] { 0x66, 0x89 });
        private static readonly OpCode M_R_U8 = new OpCode(new byte[] { 0x88 }); // Move R8 to r/rm8
        private static readonly OpCode R_M_U8 = new OpCode(new byte[] { 0x8A }); // Move r/m8 to R8

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
            if ((dest is RegisterOperand) && (src is ConstantOperand))
            {
                return R_C;
            }

            if ((dest is MemoryOperand) && (src is ConstantOperand))
            {
                return M_C;
            }

            if ((dest is RegisterOperand) && (src is RegisterOperand))
            {
                if (IsByte(src) || IsByte(dest))
                    return R_R_U8;
                if (IsChar(src) || IsChar(dest) || IsShort(src) || IsShort(dest))
                    return R_R_16;
                return R_R;
            }

            if ((dest is RegisterOperand) && (src is MemoryOperand))
            {
                if (IsByte(dest))
                    return R_M_U8;
                if (IsChar(dest) || IsShort(dest))
                    return R_M_16;
                return R_M;
            }

            if ((dest is MemoryOperand) && (src is RegisterOperand))
            {
                if (IsByte(dest))
                    return M_R_U8;
                if (IsChar(dest) || IsShort(dest))
                    return M_R_16;
                return M_R;
            }

            throw new ArgumentException(@"No opcode for operand type. [" + dest.GetType() + ", " + src.GetType() + ")");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override string ToString(Context ctx)
        {
            return String.Format(@"x86.move {0}, {1} ; {0} <- {1}", ctx.Result, ctx.Operand1);
        }

        #endregion
    }
}
