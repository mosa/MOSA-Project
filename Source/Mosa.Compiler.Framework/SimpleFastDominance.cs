/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Performs dominance calculations on basic blocks.
	/// </summary>
	/// <remarks>
	/// The stage exposes the IDominanceProvider interface for other compilation stages to allow
	/// them to use the calculated dominance properties.
	/// <para/>
	/// The implementation is based on "A Simple, Fast Dominance Algorithm" by Keith D. Cooper, 
	/// Timothy J. Harvey, and Ken Kennedy, Rice University in Houston, Texas, USA.
	/// </remarks>
	public sealed class SimpleFastDominance : IDominanceProvider
	{
		#region Data members

		/// <summary>
		/// Holds the dominance information of a block.
		/// </summary>
		private Dictionary<BasicBlock, BasicBlock> doms = new Dictionary<BasicBlock, BasicBlock>();

		/// <summary>
		/// Holds the dominance frontier blocks.
		/// </summary>
		private List<BasicBlock> domFrontier = new List<BasicBlock>();

		/// <summary>
		/// Holds the dominance frontier of individual blocks.
		/// </summary>
		Dictionary<BasicBlock, List<BasicBlock>> domFrontierOfBlock = new Dictionary<BasicBlock, List<BasicBlock>>();

		/// <summary>
		/// 
		/// </summary>
		private Dictionary<BasicBlock, List<BasicBlock>> children = new Dictionary<BasicBlock, List<BasicBlock>>();

		#endregion // Data members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public SimpleFastDominance(BasicBlocks basicBlocks, BasicBlock entryBlock)
		{
			// Blocks in reverse post order topology
			List<BasicBlock> blocks = BasicBlocks.ReversePostorder(entryBlock); //basicBlocks.GetConnectedBlocksStartingAtHead(entryBlock);

			CalculateDominance(blocks);
			CalculateChildren(blocks);
			CalculateDominanceFrontier(blocks);
		}

		/// <summary>
		/// Calculates the immediate dominance of all Blocks in the block provider.
		/// </summary>
		/// <param name="reversePostOrder">The rev post order.</param>
		private void CalculateDominance(List<BasicBlock> reversePostOrder)
		{
			// Changed flag
			bool changed = true;

			doms.Add(reversePostOrder[0], reversePostOrder[0]);

			// Calculate the dominance
			while (changed)
			{
				changed = false;
				foreach (BasicBlock b in reversePostOrder)
				{
					BasicBlock idom = null;

					foreach (var previous in b.PreviousBlocks)
					{
						if (idom == null)
						{
							idom = previous;
						}
						else
						{
							if (doms.ContainsKey(previous))
							{
								idom = Intersect(previous, idom);
							}
						}
					}

					BasicBlock dom = null;
					if (!doms.TryGetValue(b, out dom))
					{
						doms.Add(b, idom);
						changed = true;
					}
					else
					{
						if (!ReferenceEquals(dom, idom))
						{
							doms[b] = idom;
							changed = true;
						}
					}
				}
			}
		}

		/// <summary>
		/// Calculates the children.
		/// </summary>
		/// <param name="basicBlocks">The basic blocks.</param>
		private void CalculateChildren(List<BasicBlock> basicBlocks)
		{
			foreach (var block in basicBlocks)
			{
				var immediateDominator = (this as IDominanceProvider).GetImmediateDominator(block);

				if (immediateDominator == null)
					continue;

				List<BasicBlock> child = null;

				if (!children.TryGetValue(immediateDominator, out child))
				{
					child = new List<BasicBlock>();
					children.Add(immediateDominator, child);
				}

				if (block != immediateDominator)
				{
					child.AddIfNew(block);
				}
			}
		}

		/// <summary>
		/// Calculates the dominance frontier of all blocks.
		/// </summary>
		private void CalculateDominanceFrontier(List<BasicBlock> basicBlocks)
		{
			foreach (BasicBlock b in basicBlocks)
			{
				if (b.PreviousBlocks.Count > 1)
				{
					foreach (BasicBlock p in b.PreviousBlocks)
					{
						BasicBlock runner = p;

						while (runner != null && !ReferenceEquals(runner, doms[b]))
						{
							List<BasicBlock> runnerFrontier = null;

							if (!domFrontierOfBlock.TryGetValue(runner, out runnerFrontier))
							{
								runnerFrontier = new List<BasicBlock>();
								domFrontierOfBlock.Add(runner, runnerFrontier);
							}

							domFrontier.AddIfNew(b);
							runnerFrontier.AddIfNew(b);

							BasicBlock newrunner = null;
							doms.TryGetValue(runner, out newrunner);
							runner = newrunner;
						}

					}
				}
			}

		}

		/// <summary>
		/// Retrieves blocks of the iterated dominance frontier.
		/// </summary>
		/// <param name="blocks">The blocks.</param>
		/// <returns></returns>
		List<BasicBlock> IDominanceProvider.IteratedDominanceFrontier(List<BasicBlock> blocks)
		{
			var result = new List<BasicBlock>();
			var workList = new Queue<BasicBlock>();

			foreach (var block in blocks)
				workList.Enqueue(block);

			while (workList.Count > 0)
			{
				var n = workList.Dequeue();
				var dominanceFrontier = (this as IDominanceProvider).GetDominanceFrontier(n);
				if (dominanceFrontier == null)
					continue;

				foreach (var c in dominanceFrontier)
				{
					if (!result.Contains(c))
					{
						result.Add(c);
						workList.Enqueue(c);
					}
				}
			}
			return result;
		}

		#region IDominanceProvider Members

		BasicBlock IDominanceProvider.GetImmediateDominator(BasicBlock block)
		{
			if (block == null)
				throw new ArgumentNullException(@"block");

			BasicBlock idom = null;

			doms.TryGetValue(block, out idom);

			return idom;
		}

		List<BasicBlock> IDominanceProvider.GetDominators(BasicBlock block)
		{
			if (block == null)
				throw new ArgumentNullException(@"block");

			// Return value
			var result = new List<BasicBlock>();

			BasicBlock b = block;

			while (b != null)
			{
				result.Add(b);

				if (!doms.TryGetValue(b, out b))
				{
					return result;
				}
			}

			return result;
		}

		List<BasicBlock> IDominanceProvider.GetDominanceFrontier()
		{
			return domFrontier;
		}

		List<BasicBlock> IDominanceProvider.GetDominanceFrontier(BasicBlock block)
		{
			if (block == null)
				throw new ArgumentNullException(@"block");

			List<BasicBlock> domofBlock;

			if (domFrontierOfBlock.TryGetValue(block, out domofBlock))
				return domofBlock;
			else
				return new List<BasicBlock>(); // Empty List
		}

		List<BasicBlock> IDominanceProvider.GetChildren(BasicBlock block)
		{
			List<BasicBlock> child;

			if (children.TryGetValue(block, out child))
				return child;
			else
				return new List<BasicBlock>(); // Empty List
		}

		#endregion // IDominanceProvider Members

		#region Internals

		/// <summary>
		/// Retrieves the highest common immediate dominator of the two given blocks.
		/// </summary>
		/// <param name="b1">The first basic block.</param>
		/// <param name="b2">The second basic block.</param>
		/// <returns>The highest common dominator.</returns>
		private BasicBlock Intersect(BasicBlock b1, BasicBlock b2)
		{
			BasicBlock finger1 = b1, finger2 = b2;

			while (finger2 != null && finger1 != null && finger1 != finger2)
			{
				while (finger1 != null && finger1.Sequence > finger2.Sequence)
				{
					BasicBlock f = null;
					doms.TryGetValue(finger1, out f);
					finger1 = f;
				}

				while (finger2 != null && finger1 != null && finger2.Sequence > finger1.Sequence)
				{
					BasicBlock f = null;
					doms.TryGetValue(finger2, out f);
					finger2 = f;
				}
			}

			return finger1;
		}

		#endregion // Internals

	}
}
