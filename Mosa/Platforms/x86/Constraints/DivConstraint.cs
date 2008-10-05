/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */

using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.Constraints
{
    /// <summary>
    /// Provides register constraints for the x86 idiv and div instructions.
    /// </summary>
    public sealed class DivConstraint : GPRConstraint
    {
        /// <summary>
        /// Determines if this is a valid operand of the instruction.
        /// </summary>
        /// <param name="opIdx">The operand index to check.</param>
        /// <param name="op">The operand in use.</param>
        /// <returns>True if the used operand is valid or false, if it is not valid.</returns>
        public override bool IsValidOperand(int opIdx, Operand op)
        {
            return (true == base.IsValidOperand(opIdx, op) && !(op is ConstantOperand));
        }

        /// <summary>
        /// Determines if this is a valid result operand of the instruction.
        /// </summary>
        /// <param name="resIdx">The result operand index to check.</param>
        /// <param name="op">The operand in use.</param>
        /// <returns>True if the used operand is valid or false, if it is not valid.</returns>
        public override bool IsValidResult(int resIdx, Operand op)
        {
            RegisterOperand rop = op as RegisterOperand;
            return (true == base.IsValidResult(resIdx, op) && null != rop && rop.Register == GeneralPurposeRegister.EAX);
        }

        /// <summary>
        /// Returns an array of registers, that are valid for the specified result operand of the instruction.
        /// </summary>
        /// <param name="resIdx">The result operand index to check.</param>
        public override Register[] GetRegistersForResult(int resIdx)
        {
            return new Register[] { x86.GeneralPurposeRegister.EAX };
        }

        /// <summary>
        /// Retrieves an array of registers used by this instruction. This function is
        /// required if an instruction invalidates registers, which are not operands. It allows
        /// the register allocator to perform proper register spilling, if a used register also
        /// hosts a variable.
        /// </summary>
        /// <returns>An array of registers used by the instruction.</returns>
        public override Register[] GetRegistersUsed()
        {
            return new Register[] { x86.GeneralPurposeRegister.EDX };
        }
    }
}
