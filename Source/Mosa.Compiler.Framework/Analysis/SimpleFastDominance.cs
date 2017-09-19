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
	public sealed class SimpleFastDominance : BaseDominanceAnalysis
	{
		#region Data members

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
		private readonly Dictionary<BasicBlock, List<BasicBlock>> domFrontierOfBlock = new Dictionary<BasicBlock, List<BasicBlock>>();

		/// <summary>
		/// The children
		/// </summary>
		private readonly Dictionary<BasicBlock, List<BasicBlock>> children = new Dictionary<BasicBlock, List<BasicBlock>>();

		#endregion Data members

		/// <summary>
		/// Initializes a new instance of the <see cref="SimpleFastDominance"/> class.
		/// </summary>
		public SimpleFastDominance()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SimpleFastDominance"/> class.
		/// </summary>
		/// <param name="basicBlocks">The basic blocks.</param>
		/// <param name="entryBlock">The entry block.</param>
		public SimpleFastDominance(BasicBlocks basicBlocks, BasicBlock entryBlock)
		{
			PerformAnalysis(basicBlocks, entryBlock);
		}

		public override void PerformAnalysis(BasicBlocks basicBlocks, BasicBlock entryBlock)
		{
			// Blocks in reverse post order topology
			var blocks = BasicBlocks.ReversePostorder(entryBlock);

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
		/// <param name="basicBlocks">The basic blocks.</param>
		private void CalculateChildren(List<BasicBlock> basicBlocks)
		{
			foreach (var block in basicBlocks)
			{
				var immediateDominator = GetImmediateDominator(block);

				if (immediateDominator == null)
					continue;

				if (!children.TryGetValue(immediateDominator, out List<BasicBlock> child))
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
		/// <param name="basicBlocks">The basic blocks.</param>
		private void CalculateDominanceFrontier(List<BasicBlock> basicBlocks)
		{
			foreach (var b in basicBlocks)
			{
				if (b.PreviousBlocks.Count > 1)
				{
					foreach (var p in b.PreviousBlocks)
					{
						BasicBlock runner = p;

						while (runner != null && !ReferenceEquals(runner, doms[b]))
						{
							if (!domFrontierOfBlock.TryGetValue(runner, out List<BasicBlock> runnerFrontier))
							{
								runnerFrontier = new List<BasicBlock>();
								domFrontierOfBlock.Add(runner, runnerFrontier);
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
		/// <param name="blocks">The blocks.</param>
		/// <returns></returns>
		public override List<BasicBlock> IteratedDominanceFrontier(List<BasicBlock> blocks)
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

		public override BasicBlock GetImmediateDominator(BasicBlock block)
		{
			if (block == null)
				throw new ArgumentNullException(nameof(block));

			doms.TryGetValue(block, out BasicBlock idom);

			return idom;
		}

		public override List<BasicBlock> GetDominators(BasicBlock block)
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

		public override List<BasicBlock> GetDominanceFrontier()
		{
			return domFrontier;
		}

		public override List<BasicBlock> GetDominanceFrontier(BasicBlock block)
		{
			if (block == null)
				throw new ArgumentNullException(nameof(block));

			if (domFrontierOfBlock.TryGetValue(block, out List<BasicBlock> domofBlock))
				return domofBlock;
			else
				return new List<BasicBlock>(); // Empty List
		}

		public override List<BasicBlock> GetChildren(BasicBlock block)
		{
			if (children.TryGetValue(block, out List<BasicBlock> child))
				return child;
			else
				return new List<BasicBlock>(); // Empty List
		}

		/// <summary>
		/// Determines whether the specified block is dominated by another specific block
		/// </summary>
		/// <param name="dom">The DOM.</param>
		/// <param name="block">The block.</param>
		/// <returns>
		///   <c>true</c> if the specified DOM is dominator; otherwise, <c>false</c>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// dom
		/// or
		/// block
		/// </exception>
		public override bool IsDominator(BasicBlock dom, BasicBlock block)
		{
			if (dom == null)
				throw new ArgumentNullException(nameof(dom));

			if (block == null)
				throw new ArgumentNullException(nameof(block));

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
