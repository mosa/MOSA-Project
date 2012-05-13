/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

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
	public sealed class FastDominance : IDominanceProvider
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private List<BasicBlock> basicBlocks;

		/// <summary>
		/// 
		/// </summary>
		private BasicBlock entryBlock;
		/// <summary>
		/// 
		/// </summary>
		private BasicBlock exitBlock;

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
		public FastDominance(BasicBlocks basicBlocks, BasicBlock entryBlock)
		{
			this.basicBlocks = basicBlocks.GetConnectedBlocksStartingAtHead(entryBlock);
			this.entryBlock = entryBlock;
			this.exitBlock = basicBlocks.GetExitBlock(entryBlock);

			//entryBlock.NextBlocks.Add(exitBlock);
			//exitBlock.PreviousBlocks.Add(entryBlock);

			CalculateDominance();
			CalculateChildren();
			CalculateDominanceFrontier();

			//entryBlock.NextBlocks.Remove(exitBlock);
			//exitBlock.PreviousBlocks.Remove(entryBlock);
		}

		/// <summary>
		/// Calculates the immediate dominance of all Blocks in the block provider.
		/// </summary>
		private void CalculateDominance()
		{
			// Changed flag
			bool changed = true;

			// Blocks in reverse post order topology
			var revPostOrder = BasicBlocks.ReversePostorder(entryBlock);

			//doms.Add(entryBlock, entryBlock);

			// Calculate the dominance
			while (changed)
			{
				changed = false;
				foreach (BasicBlock b in revPostOrder)
				{
					BasicBlock idom = b.PreviousBlocks[0];

					for (int idx = 1; idx < b.PreviousBlocks.Count; idx++)
					{
						BasicBlock p = b.PreviousBlocks[idx];

						if (doms.ContainsKey(p))
						{
							idom = Intersect(p, idom);
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

		private void CalculateChildren()
		{
			foreach (var x in basicBlocks)
			{
				var immediateDominator = (this as IDominanceProvider).GetImmediateDominator(x);

				if (immediateDominator == null)
					continue;

				List<BasicBlock> child = null;

				if (!children.TryGetValue(immediateDominator, out child))
				{
					child = new List<BasicBlock>();
					children.Add(immediateDominator, child);
				}

				if (!child.Contains(x) && x != immediateDominator)
					child.Add(x);
			}
		}

		/// <summary>
		/// Calculates the dominance frontier of all Blocks in the block list.
		/// </summary>
		private void CalculateDominanceFrontier()
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

							if (!domFrontier.Contains(b))
							{
								domFrontier.Add(b);
							}
							if (!runnerFrontier.Contains(b))
							{
								runnerFrontier.Add(b);
							}

							runner = doms[runner];
						}

					}
				}
			}

		}

		/// <summary>
		/// Retrieves blocks of the iterated dominance frontier.
		/// </summary>
		/// <param name="block">The block.</param>
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
				var dominanceFrontier = (this as IDominanceProvider).GetDominanceFrontierOfBlock(n);
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

			while(true)
			{
				result.Add(b);

				if (!doms.TryGetValue(b, out b))
				{
					return result;
				}
			}

		}

		List<BasicBlock> IDominanceProvider.GetDominanceFrontier()
		{
			return domFrontier;
		}

		List<BasicBlock> IDominanceProvider.GetDominanceFrontierOfBlock(BasicBlock block)
		{
			if (block == null)
				throw new ArgumentNullException(@"block");

			return domFrontierOfBlock[block];
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
			BasicBlock f1 = b1, f2 = b2;

			while (f2 != null && f1 != null && f1 != f2)
			{
				while (f1 != null && f1.Sequence > f2.Sequence)
				{
					//f1 = doms[f1];
					BasicBlock f = null;
					doms.TryGetValue(f1, out f);
					f1 = f;
				}

				while (f2 != null && f1 != null && f2.Sequence > f1.Sequence)
				{
					//f2 = doms[f2];
					BasicBlock f = null;
					doms.TryGetValue(f2, out f);
					f2 = f;
				}
			}

			return f1;
		}

		#endregion // Internals

	}
}
