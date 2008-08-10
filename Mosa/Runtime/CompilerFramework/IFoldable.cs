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

namespace Mosa.Runtime.CompilerFramework.Ir
{
    /// <summary>
    /// Implemented by instructions, which can be removed by constant folding.
    /// </summary>
    public interface IFoldable
    {
        /// <summary>
        /// Determines if the instruction can be folded. This is ussually only
        /// possible if all operands are constant.
        /// </summary>
        bool IsFoldable { get; }

        /// <summary>
        /// Folds the operation and returns a new operand, which represents the result.
        /// </summary>
        /// <returns>The operand, which represents the folded result.</returns>
        Operand Fold();
    }
}
