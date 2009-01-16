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
	/// BlockOrderingStage reorders blocks to optimize loops and reduce the distance of jumps and branches.
	/// </summary>
	public class BlockOrderingStage : IMethodCompilerStage
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		protected IArchitecture arch;

		#endregion // Data members

		#region Properties

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public string Name
		{
			get { return @"Basic Block Reduction"; }
		}

		#endregion // Properties

		#region IMethodCompilerStage Members

		/// <summary>
		/// Pair of two blocks; from/to 
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

		/// <summary>
		/// Runs the specified compiler.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		public void Run(IMethodCompiler compiler)
		{
			// Retrieve the basic block provider
			IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));
			List<BasicBlock> blocks = blockProvider.Blocks;

			// Retreive the first block
			BasicBlock firstBlock = blockProvider.FromLabel(-1);

			// Create list for loop 
			List<ConnectedBlocks> loops = new List<ConnectedBlocks>();

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
						loopHeaderIndexes.Add(at.to, loopHeaderIndexes.Count + 1);

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

			// Create two-dimensional bit set of blocks belonging to loops
			BitArray bitSet = new BitArray(loopHeaderIndexes.Count * blocks.Count, false);

			// Create stack of blocks for next step of iterations
			Stack<BasicBlock> stack = new Stack<BasicBlock>();

			// Second set of iterations
			foreach (ConnectedBlocks loop in loops) {
				// Add loop-tail to bit set
				bitSet[(loopHeaderIndexes[loop.to] * loopHeaderIndexes.Count) + loop.to.Index] = true;

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
					bitSet[(loopHeaderIndexes[loop.to] * loopHeaderIndexes.Count) + at.Index] = true;

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
					if (bitSet[(i * loopHeaderIndexes.Count) + block.Index])
						depth++;

				block.LoopDepth = depth;

				// Loop index is the index of the lowest bit that is set

			}

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
