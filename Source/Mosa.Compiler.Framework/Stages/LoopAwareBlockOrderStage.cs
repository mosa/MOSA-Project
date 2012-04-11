/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

// FIXME PG

using System;
using System.Collections;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// The Loop Aware Block Ordering Stage reorders blocks to optimize loops and reduce the distance of jumps and branches.
	/// </summary>
	public class LoopAwareBlockOrderStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage, IBlockOrderStage
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private BasicBlock _first;
		/// <summary>
		/// 
		/// </summary>
		private List<ConnectedBlocks> _loops;
		/// <summary>
		/// 
		/// </summary>
		private Dictionary<BasicBlock, int> _depths;
		/// <summary>
		/// 
		/// </summary>
		private BasicBlock[] _ordered;

		#endregion // Data members

		#region Properties

		/// <summary>
		/// Gets the ordered Blocks.
		/// </summary>
		/// <value>The ordered Blocks.</value>
		public BasicBlock[] OrderedBlocks { get { return _ordered; } }

		#endregion // Properties

		#region ConnectedBlocks class

		/// <summary>
		/// Pair of two Blocks; From/to 
		/// </summary>
		protected struct ConnectedBlocks
		{
			/// <summary>
			/// Current Block
			/// </summary>
			public BasicBlock to;
			/// <summary>
			/// Succssor Block
			/// </summary>
			public BasicBlock From;

			/// <summary>
			/// Initializes a new instance of the <see cref="ConnectedBlocks"/> struct.
			/// </summary>
			/// <param name="from">From block.</param>
			/// <param name="to">To block.</param>
			public ConnectedBlocks(BasicBlock from, BasicBlock to)
			{
				this.to = to;
				this.From = from;
			}
		}

		#endregion

		#region Priority class

		/// <summary>
		/// 
		/// </summary>
		private class Priority : IComparable<Priority>
		{
			public int Depth;
			public int Order;
			public bool Hinted;

			/// <summary>
			/// Initializes a new instance of the <see cref="Priority"/> class.
			/// </summary>
			/// <param name="depth">The depth.</param>
			/// <param name="order">The order.</param>
			/// <param name="hinted">if set to <c>true</c> [hinted].</param>
			public Priority(int depth, int order, bool hinted)
			{
				Depth = depth;
				Order = order;
				Hinted = hinted;
			}

			/// <summary>
			/// Compares the current object with another object of the same type.
			/// </summary>
			/// <param name="other">An object to compare with this object.</param>
			/// <returns>
			/// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
			/// Value
			/// Meaning
			/// Less than zero
			/// This object is less than the <paramref name="other"/> parameter.
			/// Zero
			/// This object is equal to <paramref name="other"/>.
			/// Greater than zero
			/// This object is greater than <paramref name="other"/>.
			/// </returns>
			public int CompareTo(Priority other)
			{
				if (Depth > other.Depth)
					return 1;
				if (Depth < other.Depth)
					return -1;
				if (Order > other.Order)
					return 1;
				if (Order < other.Order)
					return -1;
				if (Hinted)
					return 1;
				return 0;
			}
		}

		#endregion

		#region IMethodCompilerStage Members

		/// <summary>
		/// Runs the specified compiler.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			// Retrieve the first block
			_first = FindBlock(-1);

			// Create list for loops
			_loops = new List<ConnectedBlocks>();

			// Create dictionary for the depth of basic Blocks
			_depths = new Dictionary<BasicBlock, int>(basicBlocks.Count);

			// Determine Loop Depths
			DetermineLoopDepths();

			// Order the Blocks based on loop depth
			DetermineBlockOrder();

			_loops = null;
			_depths = null;

			OrderBlocks();
		}

		/// <summary>
		/// Determines the loop depths.
		/// </summary>
		private void DetermineLoopDepths()
		{
			// Create queue for first iteration 
			Queue<ConnectedBlocks> queue = new Queue<ConnectedBlocks>();
			queue.Enqueue(new ConnectedBlocks(null, _first));

			// Flag per basic block
			Dictionary<BasicBlock, int> visited = new Dictionary<BasicBlock, int>(basicBlocks.Count);
			Dictionary<BasicBlock, int> active = new Dictionary<BasicBlock, int>(basicBlocks.Count);

			// Create dictionary for loop header index assignments
			Dictionary<BasicBlock, int> loopHeaderIndexes = new Dictionary<BasicBlock, int>();

			while (queue.Count != 0)
			{
				ConnectedBlocks at = queue.Dequeue();

				if (active.ContainsKey(at.to))
				{
					// Found a loop -
					//     the loop-header block is in at.to 
					// and the loop-end is in at.From

					// Add loop-end to list
					_loops.Add(at);

					// Assign unique loop index (if not already set)
					if (!loopHeaderIndexes.ContainsKey(at.to))
						loopHeaderIndexes.Add(at.to, loopHeaderIndexes.Count);

					// and continue iteration
					continue;
				}

				// Mark as active
				active.Add(at.to, 0);

				// Mark as visited
				visited.Add(at.to, 0);

				// Add successors to queue
				foreach (BasicBlock successor in at.to.NextBlocks)
					queue.Enqueue(new ConnectedBlocks(at.to, successor));
			}

			// Create two-dimensional bit set of blocks belonging to loops
			//BitArray bitSet = new BitArray(loopHeaderIndexes.Count * BasicBlocks.Count, false);

			Dictionary<BasicBlock, List<BasicBlock>> count = new Dictionary<BasicBlock, List<BasicBlock>>();

			// Create stack of Blocks for next step of iterations
			Stack<BasicBlock> stack = new Stack<BasicBlock>();

			// Second set of iterations
			foreach (ConnectedBlocks loop in _loops)
			{

				// Add loop-tail to list
				List<BasicBlock> current;

				if (!count.ContainsKey(loop.to))
				{
					current = new List<BasicBlock>();
					current.Add(loop.to);
					count.Add(loop.to, current);
				}
				else
				{
					current = count[loop.to];
					if (!current.Contains(loop.to))
						current.Add(loop.to);
				}

				//Console.WriteLine(index.ToString() + " : B" + loop.to.Index.ToString());

				// Add loop-end to stack
				stack.Push(loop.From);

				// Clear visit flag
				visited = new Dictionary<BasicBlock, int>(basicBlocks.Count);

				while (stack.Count != 0)
				{
					BasicBlock at = stack.Pop();

					// Already visited, continue loop
					if (visited.ContainsKey(at))
						continue;

					// Mark as visited
					if (!visited.ContainsKey(at))
						visited.Add(at, 0);

					// Add predecessor to list (needed for count)
					if (!current.Contains(at))
						current.Add(at);

					//Console.WriteLine(index.ToString() + " : B" + at.Index.ToString());

					// Add predecessors to queue
					foreach (BasicBlock predecessor in at.PreviousBlocks)
						if (predecessor != loop.to) // Exclude if Loop-Header
							stack.Push(predecessor);
				}
			}

			// Last step, assign LoopIndex and LoopDepth to each basic block
			foreach (BasicBlock block in basicBlocks)
				if (count.ContainsKey(block))
					_depths.Add(block, count[block].Count);
				else
					_depths.Add(block, 0);
		}

		/// <summary>
		/// Determines the block order.
		/// </summary>
		private void DetermineBlockOrder()
		{
			// Create an array to hold the forward branch count
			int[] forward = new int[basicBlocks.Count];

			Dictionary<BasicBlock, int> referenced = new Dictionary<BasicBlock, int>(basicBlocks.Count);

			// Copy previous branch count to array
			for (int i = 0; i < basicBlocks.Count; i++)
				forward[i] = basicBlocks[i].PreviousBlocks.Count;

			// Calculate forward branch count (PreviousBlock.Count minus loops to head)
			foreach (ConnectedBlocks connecterBlock in _loops)
				forward[connecterBlock.to.Sequence]--;

			// Allocate list of ordered Blocks
			_ordered = new BasicBlock[basicBlocks.Count];
			int orderBlockCnt = 0;

			// Create sorted worklist
			SortedList<Priority, BasicBlock> workList = new SortedList<Priority, BasicBlock>();

			// Start worklist with first block
			workList.Add(new Priority(0, 0, true), _first);

			// Order value helps sorted the worklist
			int order = 0;

			while (workList.Count != 0)
			{
				BasicBlock block = workList.Values[workList.Count - 1];
				workList.RemoveAt(workList.Count - 1);

				referenced.Add(block, 0);
				_ordered[orderBlockCnt++] = block;

				foreach (BasicBlock successor in block.NextBlocks)
				{
					forward[successor.Sequence]--;

					if (forward[successor.Sequence] == 0)
						workList.Add(new Priority(_depths[successor], order++, block.HintTarget != -1 && block.HintTarget == successor.Label), successor);
				}
			}

			// Place unreferenced blocks at the end of the list
			foreach (BasicBlock block in basicBlocks)
				if (!referenced.ContainsKey(block))
					_ordered[orderBlockCnt++] = block;
		}

		/// <summary>
		/// Orders the blocks.
		/// </summary>
		private void OrderBlocks()
		{
			for (int i = 0; i < basicBlocks.Count; i++)
				basicBlocks[i] = _ordered[i];
		}

		#endregion // Methods
	}
}
