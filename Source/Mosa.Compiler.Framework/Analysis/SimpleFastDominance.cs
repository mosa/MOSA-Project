// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Analysis
{
	/// <summary>
	/// Performs dominance calculations on basic blocks.
	/// </summary>
	/// <remarks>
	/// The implementation is based on "A Simple, Fast Dominance Algorithm" by Keith D. Cooper,
	/// Timothy J. Harvey, and Ken Kennedy, Rice University in Houston, Texas, USA.
	/// </remarks>
	public sealed class SimpleFastDominance
	{
		#region Data Members

		/// <summary>
		/// Holds the dominance information of a block.
		/// </summary>
		private readonly Dictionary<BasicBlock, BasicBlock> doms = new Dictionary<BasicBlock, BasicBlock>();

		/// <summary>
		/// Holds the dominance frontier blocks.
		/// </summary>
		private readonly List<BasicBlock> domFrontier = new List<BasicBlock>();

		/// <summary>
		/// Holds the dominance frontier of individual blocks.
		/// </summary>
		private readonly List<BasicBlock>[] domFrontierOfBlock;

		/// <summary>
		/// The children
		/// </summary>
		private readonly List<BasicBlock>[] children;

		/// <summary>
		/// The post order
		/// </summary>
		private List<BasicBlock> reversePostOrder;

		#endregion Data Members

		/// <summary>
		/// Initializes a new instance of the <see cref="SimpleFastDominance" /> class.
		/// </summary>
		/// <param name="basicBlocks">The basic blocks.</param>
		/// <param name="entryBlock">The entry block.</param>
		public SimpleFastDominance(BasicBlocks basicBlocks, BasicBlock entryBlock)
		{
			domFrontierOfBlock = new List<BasicBlock>[basicBlocks.Count];
			children = new List<BasicBlock>[basicBlocks.Count];

			// Blocks in reverse post order topology
			reversePostOrder = BasicBlocks.ReversePostOrder(entryBlock);

			CalculateDominance();
			CalculateChildren();
			CalculateDominanceFrontier();
		}

		/// <summary>
		/// Calculates the immediate dominance of all Blocks in the block provider.
		/// </summary>
		private void CalculateDominance()
		{
			// Changed flag
			bool changed = true;

			doms.Add(reversePostOrder[0], reversePostOrder[0]);

			// Calculate the dominance
			while (changed)
			{
				changed = false;
				foreach (var b in reversePostOrder)
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

					if (!doms.TryGetValue(b, out BasicBlock dom))
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
		private void CalculateChildren()
		{
			foreach (var block in reversePostOrder)
			{
				var immediateDominator = GetImmediateDominator(block);

				if (immediateDominator == null)
					continue;

				if (block == immediateDominator)
					continue;

				var list = children[immediateDominator.Sequence];

				if (list == null)
				{
					list = new List<BasicBlock>();
					children[immediateDominator.Sequence] = list;
				}

				list.AddIfNew(block);
			}
		}

		/// <summary>
		/// Calculates the dominance frontier of all blocks.
		/// </summary>
		private void CalculateDominanceFrontier()
		{
			foreach (var b in reversePostOrder)
			{
				if (b.PreviousBlocks.Count > 1)
				{
					foreach (var p in b.PreviousBlocks)
					{
						var runner = p;

						while (runner != null && !ReferenceEquals(runner, doms[b]))
						{
							var runnerFrontier = domFrontierOfBlock[runner.Sequence];

							if (runnerFrontier == null)
							{
								runnerFrontier = new List<BasicBlock>();
								domFrontierOfBlock[runner.Sequence] = runnerFrontier;
							}

							domFrontier.AddIfNew(b);
							runnerFrontier.AddIfNew(b);

							doms.TryGetValue(runner, out BasicBlock newrunner);
							runner = newrunner;
						}
					}
				}
			}
		}

		/// <summary>
		/// Retrieves blocks of the iterated dominance frontier.
		/// </summary>
		/// <returns></returns>
		public List<BasicBlock> IteratedDominanceFrontier(List<BasicBlock> blocks)
		{
			var result = new List<BasicBlock>();
			var workList = new Queue<BasicBlock>();

			foreach (var block in blocks)
				workList.Enqueue(block);

			while (workList.Count > 0)
			{
				var n = workList.Dequeue();

				var dominanceFrontier = GetDominanceFrontier(n);

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

		#region BaseDominanceAnalysis Members

		public BasicBlock GetImmediateDominator(BasicBlock block)
		{
			if (block == null)
				throw new ArgumentNullException(nameof(block));

			doms.TryGetValue(block, out BasicBlock idom);

			return idom;
		}

		public List<BasicBlock> GetDominators(BasicBlock block)
		{
			if (block == null)
				throw new ArgumentNullException(nameof(block));

			var result = new List<BasicBlock>();
			var b = block;

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

		public List<BasicBlock> GetDominanceFrontier()
		{
			return domFrontier;
		}

		public List<BasicBlock> GetDominanceFrontier(BasicBlock block)
		{
			return domFrontierOfBlock[block.Sequence];
		}

		public List<BasicBlock> GetChildren(BasicBlock block)
		{
			if (children[block.Sequence] == null)
				return new List<BasicBlock>();

			return children[block.Sequence];
		}

		/// <summary>
		/// Determines whether the specified block is dominated by another specific block
		/// </summary>
		/// <param name="dom">The DOM.</param>
		/// <param name="block">The block.</param>
		/// <returns>
		///   <c>true</c> if the specified DOM is dominator; otherwise, <c>false</c>.
		/// </returns>
		public bool IsDominator(BasicBlock dom, BasicBlock block)
		{
			var b = block;

			while (b != null)
			{
				if (!doms.TryGetValue(b, out b))
				{
					return false;
				}

				if (dom == b)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Gets the basic blocks in reverse post order.
		/// </summary>
		/// <returns></returns>
		public List<BasicBlock> GetReversePostOrder()
		{
			return reversePostOrder;
		}

		#endregion BaseDominanceAnalysis Members

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
				while (finger1?.Sequence > finger2.Sequence)
				{
					doms.TryGetValue(finger1, out BasicBlock f);
					finger1 = f;
				}

				while (finger2 != null && finger1 != null && finger2.Sequence > finger1.Sequence)
				{
					doms.TryGetValue(finger2, out BasicBlock f);
					finger2 = f;
				}
			}

			return finger1;
		}

		#endregion Internals
	}
}
