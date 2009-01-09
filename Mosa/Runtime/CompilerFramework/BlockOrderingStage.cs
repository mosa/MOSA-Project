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
	/// BasicBlockReduction attempts to eliminate useless control flow created as a side effect of other compiler optimizations.
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

		#endregion // Properties

		#region IMethodCompilerStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public string Name
		{
			get { return @"Basic Block Reduction"; }
		}

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

			// Flag per basic block
			BitArray visited = new BitArray(blocks.Count);
			BitArray active = new BitArray(blocks.Count);

			// Start counter for unique loop index (starts at 1)
			int uniqueLoopIndexCount = 1; 

			// Retreive the first block
			BasicBlock firstBlock = blockProvider.FromLabel(-1);
			
			// Initial loop index for the first block is set to -1
			firstBlock.LoopIndex = -1;

			// Create list for loop headers
			List<BasicBlock> loopEndBlocks = new List<BasicBlock>();

			// Create queue for first iteration 
			Queue<ConnectedBlocks> queue = new Queue<ConnectedBlocks>();
			queue.Enqueue(new ConnectedBlocks(null, firstBlock));

			while (queue.Count != 0) {
				ConnectedBlocks at = queue.Dequeue();

				if (active.Get(at.to.Index)) {
					// Found a loop -
					//     the loop-header block is in at.to 
					// and the loop-end is in at.from

					// Add loop-end to list
					loopEndBlocks.Add(at.from);

					// Assign unique loop index (if not already set)
					if (at.to.LoopIndex <= 0)
						at.to.LoopIndex = uniqueLoopIndexCount++;

					// and continue iteration
					continue;
				}
				else {
					active.Set(at.to.Index, false);
				}

				visited.Set(at.to.Index, false);

				// Add successors to queue
				foreach (BasicBlock successor in at.to.NextBlocks)
					queue.Enqueue(new ConnectedBlocks(at.to, successor));
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
