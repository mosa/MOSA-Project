/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System.Collections.Generic;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Allows downlevel compilation stages to access basic Blocks and the raw instruction stream
    /// determined by a previous compilation stage.
    /// </summary>
    public interface IBasicBlockProvider : IEnumerable<BasicBlock>
    {
        /// <summary>
        /// Gets the basic Blocks.
        /// </summary>
        /// <value>The basic Blocks.</value>
        List<BasicBlock> Blocks { get; }

        /// <summary>
        /// Retrieves a basic block from its label.
        /// </summary>
        /// <param name="label">The label of the basic block.</param>
        /// <returns>The basic block with the given label or null.</returns>
        BasicBlock FromLabel(int label);
    }
}
