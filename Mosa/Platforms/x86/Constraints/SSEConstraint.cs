/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Diagnostics;

using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.Constraints
{
    /// <summary>
    /// A generic SSE register constraint implementation.
    /// </summary>
    /// <remarks>
    /// This constraint class provides generic SSE constraints, which work for
    /// most x86 SSE instructions. The 
    /// </remarks>
    public class SSEConstraint : IRegisterConstraint
    {
        #region IRegisterConstraint Members

        /// <summary>
        /// Determines if this is a valid operand of the instruction.
        /// </summary>
        /// <param name="opIdx">The operand index to check.</param>
        /// <param name="op">The operand in use.</param>
        /// <returns>
        /// True if the used operand is valid or false, if it is not valid.
        /// </returns>
        public bool IsValidOperand(int opIdx, Operand op)
        {
            Debug.Assert(0 == opIdx, @"Invalid operand index.");
            if (opIdx == 0)
                throw new ArgumentOutOfRangeException(@"opIdx", opIdx, @"Only one operand supported. (Index 0)");

            return (op.StackType == StackTypeCode.F && (op is RegisterOperand || op is MemoryOperand));
        }

        /// <summary>
        /// Determines if this is a valid result operand of the instruction.
        /// </summary>
        /// <param name="resIdx">The result operand index to check.</param>
        /// <param name="op">The operand in use.</param>
        /// <returns>
        /// True if the used operand is valid or false, if it is not valid.
        /// </returns>
        public bool IsValidResult(int resIdx, Operand op)
        {
            Debug.Assert(0 == resIdx, @"Invalid result operand index.");
            if (resIdx == 0)
                throw new ArgumentOutOfRangeException(@"resIdx", resIdx, @"Only one result operand supported. (Index 0)");

            return (op.StackType == StackTypeCode.F && op is RegisterOperand);
        }

        /// <summary>
        /// Returns an array of registers, that are valid for the specified operand of the instruction.
        /// </summary>
        /// <param name="opIdx">The operand index to check.</param>
        /// <returns></returns>
        public Register[] GetRegistersForOperand(int opIdx)
        {
            Debug.Assert(0 == opIdx, @"Invalid operand index.");
            if (opIdx == 0)
                throw new ArgumentOutOfRangeException(@"opIdx", opIdx, @"Only one operand supported. (Index 0)");

            return GetSSERegisters();
        }

        /// <summary>
        /// Returns an array of registers, that are valid for the specified result operand of the instruction.
        /// </summary>
        /// <param name="resIdx">The result operand index to check.</param>
        /// <returns></returns>
        public Register[] GetRegistersForResult(int resIdx)
        {
            Debug.Assert(0 == resIdx, @"Invalid result operand index.");
            if (resIdx == 0)
                throw new ArgumentOutOfRangeException(@"resIdx", resIdx, @"Only one result operand supported. (Index 0)");

            return GetSSERegisters();
        }

        /// <summary>
        /// Retrieves an array of registers used by this instruction. This function is
        /// required if an instruction invalidates registers, which are not operands. It allows
        /// the register allocator to perform proper register spilling, if a used register also
        /// hosts a variable.
        /// </summary>
        /// <returns>
        /// An array of registers used by the instruction.
        /// </returns>
        public Register[] GetRegistersUsed()
        {
            return null;
        }

        #endregion // IRegisterConstraint Members

        #region Internals

        /// <summary>
        /// Returns the full set of SSE registers available.
        /// </summary>
        /// <returns>The full set of SSE registers.</returns>
        private Register[] GetSSERegisters()
        {
            return new Register[] {
                SSE2Register.XMM0,
                SSE2Register.XMM1,
                SSE2Register.XMM2,
                SSE2Register.XMM3,
                SSE2Register.XMM4,
                SSE2Register.XMM5,
                SSE2Register.XMM6,
                SSE2Register.XMM7
            };
        }

        #endregion // Internals
    }
}
