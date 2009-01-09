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

			// Start counter for unique loop index (starts at 1)
			int uniqueLoopIndexCount = 1;

			// Retreive the first block
			BasicBlock firstBlock = blockProvider.FromLabel(-1);

			// Initial loop index for the first block is set to -1
			firstBlock.LoopIndex = -1;

			// Create list for loop 
			List<ConnectedBlocks> loops = new List<ConnectedBlocks>();

			// Create queue for first iteration 
			Queue<ConnectedBlocks> queue = new Queue<ConnectedBlocks>();
			queue.Enqueue(new ConnectedBlocks(null, firstBlock));

			// Flag per basic block
			BitArray visited = new BitArray(blocks.Count, false);
			BitArray active = new BitArray(blocks.Count, false);

			while (queue.Count != 0) {
				ConnectedBlocks at = queue.Dequeue();

				if (active.Get(at.to.Index)) {
					// Found a loop -
					//     the loop-header block is in at.to 
					// and the loop-end is in at.from

					// Add loop-end to list
					loops.Add(at);

					// Assign unique loop index (if not already set)
					if (at.to.LoopIndex <= 0)
						at.to.LoopIndex = uniqueLoopIndexCount++;

					// and continue iteration
					continue;
				}
				else {
					// Mark as active
					active.Set(at.to.Index, false);
				}

				// Mark as visited
				visited.Set(at.to.Index, false);

				// Add successors to queue
				foreach (BasicBlock successor in at.to.NextBlocks)
					queue.Enqueue(new ConnectedBlocks(at.to, successor));
			}

			// Create stack of blocks for next step of iterations
			Stack<BasicBlock> stack = new Stack<BasicBlock>();

			// Second set of iterations
			foreach (ConnectedBlocks loop in loops) {
				BasicBlock loopHeader = loop.to;
				BasicBlock loopEnd = loop.from;

				// Creat list of blocks in loop
				List<BasicBlock> loopBlocks = new List<BasicBlock>();

				// Add loop-end to stack
				stack.Push(loopEnd);

				// Clear visit flag
				visited = new BitArray(blocks.Count, false);

				while (stack.Count != 0) {
					BasicBlock at = stack.Pop();

					// already visited, continue loop
					if (visited.Get(at.Index)) 						
						continue;

					// Mark as visisted
					visited.Set(at.Index, false);

					// Add loop to list of blocks in loop
					loopBlocks.Add(at);

					// Add predecessors to queue
					foreach (BasicBlock predecessor in at.PreviousBlocks)
						if (predecessor != loopHeader)
							stack.Push(predecessor);
				}

				loopBlocks.Add(loopHeader);

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
