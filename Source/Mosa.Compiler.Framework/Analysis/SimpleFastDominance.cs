// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System.Collections.Generic;
using System.Diagnostics;

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
		private readonly BasicBlock[] doms;

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
		private readonly List<BasicBlock> reversePostOrder;

		private readonly int[] blockToPostorderIndex;

		#endregion Data Members

		/// <summary>
		/// Initializes a new instance of the <see cref="SimpleFastDominance" /> class.
		/// </summary>
		/// <param name="basicBlocks">The basic blocks.</param>
		/// <param name="entryBlock">The entry block.</param>
		public SimpleFastDominance(BasicBlocks basicBlocks, BasicBlock entryBlock)
		{
			int blockCount = basicBlocks.Count;

			doms = new BasicBlock[blockCount];
			domFrontierOfBlock = new List<BasicBlock>[blockCount];
			children = new List<BasicBlock>[blockCount];
			blockToPostorderIndex = new int[blockCount];

			// Blocks in reverse postorder
			reversePostOrder = basicBlocks.ReversePostorder(entryBlock);

			// Map block to reverse postorder index
			int i = reversePostOrder.Count;
			foreach (var block in reversePostOrder)
			{
				blockToPostorderIndex[block.Sequence] = --i;
			}

			CalculateDominance();
			CalculateChildren();
			CalculateDominanceFrontier();
		}

		/// <summary>
		/// Calculates the immediate dominance of all Blocks in the block provider.
		/// </summary>
		private void CalculateDominance()
		{
			var startNode = reversePostOrder[0];
			doms[startNode.Sequence] = startNode;

			bool changed = true;

			while (changed)
			{
				changed = false;
				foreach (var b in reversePostOrder)
				{
					if (b == startNode)
						continue;

					BasicBlock newIDom = null;

					foreach (var previous in b.PreviousBlocks)
					{
						var dom = doms[previous.Sequence];

						if (dom == null)
							continue;

						if (newIDom == null)
						{
							newIDom = previous;
						}
						else
						{
							newIDom = Intersect(previous, newIDom);
						}
					}

					if (doms[b.Sequence] != newIDom || doms[b.Sequence] == null)
					{
						doms[b.Sequence] = newIDom;
						changed = true;
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

				var list = children[immediateDominator.Sequence];

				if (list == null)
				{
					list = new List<BasicBlock>();
					children[immediateDominator.Sequence] = list;
				}

				if (block == immediateDominator)
					continue;

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

						while (runner != null && runner != doms[b.Sequence])
						{
							var runnerFrontier = domFrontierOfBlock[runner.Sequence];

							if (runnerFrontier == null)
							{
								runnerFrontier = new List<BasicBlock>();
								domFrontierOfBlock[runner.Sequence] = runnerFrontier;
							}

							domFrontier.AddIfNew(b);
							runnerFrontier.AddIfNew(b);

							runner = doms[runner.Sequence];
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
			{
				workList.Enqueue(block);
			}

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
			return doms[block.Sequence];
		}

		public List<BasicBlock> GetDominators(BasicBlock block)
		{
			var result = new List<BasicBlock>();
			var b = block;

			while (b != null)
			{
				result.Add(b);

				b = doms[b.Sequence];

				if (b == null)
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
				var c = doms[b.Sequence];

				if (c == null || c == b)
					return false;

				if (dom == c)
				{
					return true;
				}

				b = c;
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
			var finger1 = b1;
			var finger2 = b2;

			while (finger1 != finger2)
			{
				while (blockToPostorderIndex[finger1.Sequence] < blockToPostorderIndex[finger2.Sequence])
				{
					Debug.Assert(doms[finger1.Sequence] != null);
					finger1 = doms[finger1.Sequence];
				}

				while (blockToPostorderIndex[finger2.Sequence] < blockToPostorderIndex[finger1.Sequence])
				{
					Debug.Assert(doms[finger2.Sequence] != null);
					finger2 = doms[finger2.Sequence];
				}
			}

			return finger1;
		}

		#endregion Internals
	}
}
