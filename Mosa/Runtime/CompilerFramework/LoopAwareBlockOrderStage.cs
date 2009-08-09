/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// The Loop Aware Block Ordering Stage reorders Blocks to optimize loops and reduce the distance of jumps and branches.
	/// </summary>
	public class LoopAwareBlockOrderStage : IMethodCompilerStage, IBasicBlockOrder
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		protected IArchitecture arch;

		/// <summary>
		/// 
		/// </summary>
		private List<BasicBlock> blocks;
		/// <summary>
		/// 
		/// </summary>
		private BasicBlock firstBlock;
		/// <summary>
		/// 
		/// </summary>
		private List<ConnectedBlocks> loops;
		/// <summary>
		/// 
		/// </summary>
		private Dictionary<BasicBlock, int> loopDepths;
		/// <summary>
		/// 
		/// </summary>
		private int[] orderedBlocks;

		#endregion // Data members

		#region Properties

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public string Name
		{
			get { return @"Loop Aware Block Order"; }
		}

		/// <summary>
		/// Gets the ordered Blocks.
		/// </summary>
		/// <value>The ordered Blocks.</value>
		public int[] OrderedBlocks { get { return orderedBlocks; } }

		#endregion // Properties

		#region IMethodCompilerStage Members

		#region ConnectedBlocks class

		/// <summary>
		/// Pair of two Blocks; from/to 
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
			public BasicBlock from;

			/// <summary>
			/// Initializes a new instance of the <see cref="ConnectedBlocks"/> struct.
			/// </summary>
			/// <param name="from">From block.</param>
			/// <param name="to">To block.</param>
			public ConnectedBlocks(BasicBlock from, BasicBlock to)
			{
				this.to = to;
				this.from = from;
			}
		}

		#endregion

		/// <summary>
		/// Runs the specified compiler.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		public void Run(IMethodCompiler compiler)
		{
			// Retrieve the basic block provider
			IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));

			if (blockProvider == null)
				throw new InvalidOperationException(@"Loop Aware Block Order stage requires basic Blocks.");
			
			blocks = blockProvider.Blocks;

			// Retreive the first block
			firstBlock = blockProvider.FromLabel(-1);

			// Create list for loops
			loops = new List<ConnectedBlocks>();

			// Create dictionary for the depth of basic Blocks
			loopDepths = new Dictionary<BasicBlock, int>(blocks.Count);

			// Deteremine Loop Depths
			DetermineLoopDepths();

			// Order the Blocks based on loop depth
			DetermineBlockOrder();
		}

		/// <summary>
		/// Determines the loop depths.
		/// </summary>
		private void DetermineLoopDepths()
		{
			// Create queue for first iteration 
			Queue<ConnectedBlocks> queue = new Queue<ConnectedBlocks>();
			queue.Enqueue(new ConnectedBlocks(null, firstBlock));

			// Flag per basic block
			BitArray visited = new BitArray(blocks.Count, false);
			BitArray active = new BitArray(blocks.Count, false);

			// Create dictionary for loop header index assignments
			Dictionary<BasicBlock, int> loopHeaderIndexes = new Dictionary<BasicBlock, int>();

			while (queue.Count != 0) {
				ConnectedBlocks at = queue.Dequeue();

				if (active.Get(at.to.Index)) {
					// Found a loop -
					//     the loop-header block is in at.to 
					// and the loop-end is in at.from

					// Add loop-end to list
					loops.Add(at);

					// Assign unique loop index (if not already set)
					if (!loopHeaderIndexes.ContainsKey(at.to))
						loopHeaderIndexes.Add(at.to, loopHeaderIndexes.Count);

					// and continue iteration
					continue;
				}
				else {
					// Mark as active
					active.Set(at.to.Index, true);
				}

				// Mark as visited
				visited.Set(at.to.Index, true);

				// Add successors to queue
				foreach (BasicBlock successor in at.to.NextBlocks)
					queue.Enqueue(new ConnectedBlocks(at.to, successor));
			}

			// Create two-dimensional bit set of Blocks belonging to loops
			BitArray bitSet = new BitArray(loopHeaderIndexes.Count * blocks.Count, false);

			// Create stack of Blocks for next step of iterations
			Stack<BasicBlock> stack = new Stack<BasicBlock>();

			// Second set of iterations
			foreach (ConnectedBlocks loop in loops) {
				int index = loopHeaderIndexes[loop.to];

				// Add loop-tail to bit set
				bitSet[(index * blocks.Count) + loop.to.Index] = true;

				//Console.WriteLine(index.ToString() + " : B" + loop.to.Index.ToString());

				// Add loop-end to stack
				stack.Push(loop.from);

				// Clear visit flag
				visited = new BitArray(blocks.Count, false);

				while (stack.Count != 0) {
					BasicBlock at = stack.Pop();

					// already visited, continue loop
					if (visited.Get(at.Index))
						continue;

					// Mark as visisted
					visited.Set(at.Index, true);

					// Set predecessor to bit set
					bitSet[(index * blocks.Count) + at.Index] = true;

					//Console.WriteLine(index.ToString() + " : B" + at.Index.ToString());

					// Add predecessors to queue
					foreach (BasicBlock predecessor in at.PreviousBlocks)
						if (predecessor != loop.to) // Exclude if Loop-Header
							stack.Push(predecessor);
				}
			}

			// Last step, assign LoopIndex and LoopDepth to each basic block
			foreach (BasicBlock block in blocks) {
				// Loop depth is the number of bits that are set for the according block id
				int depth = 0;

				for (int i = 0; i < loopHeaderIndexes.Count; i++)
					if (bitSet[(i * blocks.Count) + block.Index])
						depth++;
				
				// Set loop depth
				loopDepths.Add(block, depth);
			}
		}

		#region Priority class
		/// <summary>
		/// 
		/// </summary>
		private class Priority : IComparable<Priority>
		{
			public int Depth;
			public int Order;

			/// <summary>
			/// Initializes a new instance of the <see cref="Priority"/> class.
			/// </summary>
			/// <param name="depth">The depth.</param>
			/// <param name="order">The order.</param>
			public Priority(int depth, int order)
			{
				Depth = depth;
				Order = order;
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
				else
					if (Depth < other.Depth)
						return -1;
					else
						if (Order > other.Order)
							return 1;
						else
							if (Order < other.Order)
								return -1;
							else
								return 0;
			}
		}
		#endregion

		/// <summary>
		/// Determines the block order.
		/// </summary>
		private void DetermineBlockOrder()
		{
			// Create an array to hold the forward branch count
			int[] forwardBranches = new int[blocks.Count];

			// Copy previous branch count to array
			for (int i = 0; i < blocks.Count; i++)
				forwardBranches[i] = blocks[i].PreviousBlocks.Count;

			// Calculate forward branch count (PreviousBlock.Count minus loops to head)
			foreach (ConnectedBlocks connecterBlock in loops)
				forwardBranches[connecterBlock.to.Index]--;

			// Allocate list of ordered Blocks
			orderedBlocks = new int[blocks.Count];
			int orderBlockCnt = 0;

			// Create bit array of refereced Blocks (by index)
			BitArray referencedBlocks = new BitArray(blocks.Count, false);

			// Create sorted worklist
			SortedList<Priority, BasicBlock> workList = new SortedList<Priority, BasicBlock>();

			// Start worklist with first block
			workList.Add(new Priority(0, 0), firstBlock);

			// Order value helps sorted the worklist
			int order = 0;

			while (workList.Count != 0) {
				BasicBlock block = workList.Values[workList.Count - 1];
				workList.RemoveAt(workList.Count - 1);

				referencedBlocks.Set(block.Index, true);

				orderedBlocks[orderBlockCnt++] = block.Index;

				foreach (BasicBlock successor in block.NextBlocks) {
					forwardBranches[successor.Index]--;

					if (forwardBranches[successor.Index] == 0)
						workList.Add(new Priority(loopDepths[successor], order++), successor);
				}
			}

			// Place unreferenced Blocks at the end of the list
			for (int i = 0; i < blocks.Count; i++)
				if (!referencedBlocks.Get(i))
					orderedBlocks[orderBlockCnt++] = i;			
		}

		/// <summary>
		/// Adds to pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline.</param>
		public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
			pipeline.InsertBefore<IL.CilToIrTransformationStage>(this);
		}

		#endregion // Methods
	}
}
