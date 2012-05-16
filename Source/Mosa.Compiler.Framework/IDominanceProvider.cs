/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System.Collections.Generic;

namespace Mosa.Compiler.Framework
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
		List<BasicBlock> GetDominators(BasicBlock block);

		/// <summary>
		/// Retrieves the blocks which are in the dominance frontier of any other block.
		/// </summary>
		/// <returns>All blocks which are in a dominance frontier of another block.</returns>
		List<BasicBlock> GetDominanceFrontier();

		/// <summary>
		/// Retrieves the dominance frontier of the given block.
		/// </summary>
		/// <param name="block">The block to return the dominance frontier of.</param>
		/// <returns>An array of Blocks, which represent the dominance frontier.</returns>
		List<BasicBlock> GetDominanceFrontier(BasicBlock block);

		/// <summary>
		/// Gets the children.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <returns></returns>
		List<BasicBlock> GetChildren(BasicBlock block);

		/// <summary>
		/// Retrieves blocks of the iterated dominance frontier.
		/// </summary>
		/// <param name="blocks">The blocks.</param>
		/// <returns></returns>
		List<BasicBlock> IteratedDominanceFrontier(List<BasicBlock> blocks);
	}
}
