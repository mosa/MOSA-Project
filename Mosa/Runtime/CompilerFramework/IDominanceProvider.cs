/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// A dominance provider allows other compilation stages to retrieve dominance relationships.
    /// </summary>
    public interface IDominanceProvider
    {
        /// <summary>
        /// Retrieves the immediate dominator of a block.
        /// </summary>
        /// <param name="block">The block to retrieve the immediate dominator for.</param>
        /// <returns>The immediate dominator of the block.</returns>
        BasicBlock GetImmediateDominator(BasicBlock block);

        /// <summary>
        /// Retrieves the dominators of the given block.
        /// </summary>
        /// <param name="block">The block to retrieve dominators for.</param>
        /// <returns>An array of dominators of a block. The first element (at index zero) is the block itself.</returns>
        BasicBlock[] GetDominators(BasicBlock block);

        /// <summary>
        /// Retrieves the Blocks which are in the dominance frontier of any other block.
        /// </summary>
        /// <returns>All Blocks which are in a dominance frontier of another block.</returns>
        BasicBlock[] GetDominanceFrontier();

        /// <summary>
        /// Retrieves the dominance frontier of the given block.
        /// </summary>
        /// <param name="block">The block to return the dominance frontier of.</param>
        /// <returns>An array of Blocks, which represent the dominance frontier.</returns>
        BasicBlock[] GetDominanceFrontierOfBlock(BasicBlock block);
    }
}
