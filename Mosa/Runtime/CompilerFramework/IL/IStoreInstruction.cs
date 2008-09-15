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

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Interface used to tag instructions, which store a value.
    /// </summary>
    /// <remarks>
    /// This interface is used by <see cref="CilToIrTransformationStage"/> to drop 
    /// store instructions from the instruction stream. It uses the interface to determine
    /// the appropriate operands to replace/remove.
    /// </remarks>
    public interface IStoreInstruction
    {
        /// <summary>
        /// Gets the operand to store.
        /// </summary>
        Operand Source { get; }

        /// <summary>
        /// Gets the store destination operand.
        /// </summary>
        Operand Destination { get; }
    }
}
