/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// This interface is used to present register constraints by native
    /// instructions to the register allocator.
    /// </summary>
    public interface IRegisterConstraint
    {
        /// <summary>
        /// Returns an array of register constraints for the operands of the instruction.
        /// </summary>
        /// <returns>
        /// An array of register indices, which correspond to destination, first operand, 
        /// second operand, ..., last operand. Each integer represents either no constraint (0) 
        /// or the index of a required register.
        /// </returns>
        Register[] GetConstraints();

        /// <summary>
        /// Retrieves an array of registers used by this instruction. This function is
        /// required if an instruction invalidates registers, which are not operands. It allows
        /// the register allocator to perform proper register spilling, if a used register also
        /// hosts a variable.
        /// </summary>
        /// <returns>An array of register codes.</returns>
        Register[] GetRegistersUsed();
    }
}
