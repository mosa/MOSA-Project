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

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// 
	/// </summary>
	public class LoopAwareBlockOrder
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		protected BasicBlocks basicBlocks;
		/// <summary>
		/// 
		/// </summary>
		private List<LoopEdge> loopEdges;
		/// <summary>
		/// 
		/// </summary>
		private Dictionary<BasicBlock, int> depths;
		/// <summary>
		/// 
		/// </summary>
		private BasicBlock[] ordered;

		#endregion // Data members

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

		/// <summary>
		/// Gets the loop edges.
		/// </summary>
		public List<LoopEdge> LoopEdges { get { return loopEdges; } }

		/// <summary>
		/// Runs the specified compiler.
		/// </summary>
		public LoopAwareBlockOrder(BasicBlocks basicBlocks)
		{
			this.basicBlocks = basicBlocks;

			// Create list for loops
			loopEdges = new List<LoopEdge>();

			// Create dictionary for the depth of basic Blocks
			depths = new Dictionary<BasicBlock, int>(basicBlocks.Count);

			OrderBlock();
		}

		protected void OrderBlock()
		{
			// Retrieve the first block
			BasicBlock first = FindBlock(-1);

			// Determine Loop Depths
			FindLoops(first);

			DetermineLoopDepths();

			// Order the Blocks based on loop depth
			DetermineBlockOrder(first);
		}

		/// <summary>
		/// Finds the block.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		protected BasicBlock FindBlock(int index)
		{
			foreach (var block in basicBlocks)
				if (block.Index == index)
					return block;

			return null;
		}

		/// <summary>
		/// Determines the loop depths.
		/// </summary>
		protected void FindLoops(BasicBlock start)
		{
			// Create a list of loops ends

			// Create queue for first iteration 
			Queue<LoopEdge> queue = new Queue<LoopEdge>();
			queue.Enqueue(new LoopEdge(null, start));

			// Flag per basic block
			BitArray visited = new BitArray(basicBlocks.Count, false);
			BitArray active = new BitArray(basicBlocks.Count, false);

			while (queue.Count != 0)
			{
				LoopEdge state = queue.Dequeue();
				BasicBlock current = state.Tail;

				// Mark as visited
				visited.Set(current.Sequence, true);

				if (active.Get(current.Sequence))
				{
					// Found a loop

					// Add to loop list
					loopEdges.Add(state);

					// and continue iteration
					continue;
				}

				// Mark as active
				active.Set(current.Sequence, true);

				// Add successors to queue
				foreach (BasicBlock successor in current.NextBlocks)
					queue.Enqueue(new LoopEdge(current, successor));
			}

		}

		protected void DetermineLoopDepths()
		{

			// Create two-dimensional bit set of blocks belonging to loops
			//BitArray bitSet = new BitArray(loopHeaderIndexes.Count * BasicBlocks.Count, false);

			Dictionary<BasicBlock, int> visited = new Dictionary<BasicBlock, int>(basicBlocks.Count);
			Dictionary<BasicBlock, List<BasicBlock>> count = new Dictionary<BasicBlock, List<BasicBlock>>();

			// Create stack of blocks 
			Stack<BasicBlock> stack = new Stack<BasicBlock>();

			// Second set of iterations
			foreach (LoopEdge loop in loopEdges)
			{

				// Add loop-tail to list
				List<BasicBlock> current;

				if (!count.ContainsKey(loop.Tail))
				{
					current = new List<BasicBlock>();
					current.Add(loop.Tail);
					count.Add(loop.Tail, current);
				}
				else
				{
					current = count[loop.Tail];
					if (!current.Contains(loop.Tail))
						current.Add(loop.Tail);
				}

				//Console.WriteLine(index.ToString() + " : B" + loop.to.Index.ToString());

				// Add loop-end to stack
				stack.Push(loop.Head);

				// Clear visit flag
				visited.Clear();
				//visited = new Dictionary<BasicBlock, int>(basicBlocks.Count);

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
						if (predecessor != loop.Tail) // Exclude if Loop-Header
							stack.Push(predecessor);
				}
			}

			// Last step, assign LoopIndex and LoopDepth to each basic block
			foreach (BasicBlock block in basicBlocks)
				if (count.ContainsKey(block))
					depths.Add(block, count[block].Count);
				else
					depths.Add(block, 0);
		}

		/// <summary>
		/// Determines the block order.
		/// </summary>
		protected void DetermineBlockOrder(BasicBlock start)
		{
			// Create an array to hold the forward branch count
			int[] forward = new int[basicBlocks.Count];

			Dictionary<BasicBlock, int> referenced = new Dictionary<BasicBlock, int>(basicBlocks.Count);

			// Copy previous branch count to array
			for (int i = 0; i < basicBlocks.Count; i++)
				forward[i] = basicBlocks[i].PreviousBlocks.Count;

			// Calculate forward branch count (PreviousBlock.Count minus loops to head)
			foreach (LoopEdge connecterBlock in loopEdges)
				forward[connecterBlock.Tail.Sequence]--;

			// Allocate list of ordered Blocks
			ordered = new BasicBlock[basicBlocks.Count];
			int orderBlockCnt = 0;

			// Create sorted worklist
			SortedList<Priority, BasicBlock> workList = new SortedList<Priority, BasicBlock>();

			// Start worklist with first block
			workList.Add(new Priority(0, 0, true), start);

			// Order value helps sorted the worklist
			int order = 0;

			while (workList.Count != 0)
			{
				BasicBlock block = workList.Values[workList.Count - 1];
				workList.RemoveAt(workList.Count - 1);

				referenced.Add(block, 0);
				ordered[orderBlockCnt++] = block;

				foreach (BasicBlock successor in block.NextBlocks)
				{
					forward[successor.Sequence]--;

					if (forward[successor.Sequence] == 0)
						workList.Add(new Priority(depths[successor], order++, block.HintTarget != -1 && block.HintTarget == successor.Label), successor);
				}
			}

			// Place unreferenced blocks at the end of the list
			foreach (BasicBlock block in basicBlocks)
				if (!referenced.ContainsKey(block))
					ordered[orderBlockCnt++] = block;
		}

	}
}

