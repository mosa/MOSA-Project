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
    /// Allows downlevel compilation stages to access basic blocks and the raw instruction stream
    /// determined by a previous compilation stage.
    /// </summary>
    public interface IBasicBlockProvider : IEnumerable<BasicBlock>
    {
        /// <summary>
        /// Returns the number of basic blocks found.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Retrieves a specific basic block.
        /// </summary>
        /// <param name="index">The index of the basic block to retrieve.</param>
        /// <returns>The basic block to retrieve.</returns>
        BasicBlock this[int index] { get; }

        /// <summary>
        /// Retrieves a basic block from its label.
        /// </summary>
        /// <param name="label">The label of the basic block.</param>
        /// <returns>The basic block with the given label or null.</returns>
        BasicBlock FromLabel(int label);
    }
}
