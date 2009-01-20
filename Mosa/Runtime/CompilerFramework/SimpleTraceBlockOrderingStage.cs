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
	/// SimpleTraceBlockOrderingStage reorders blocks to optimize loops and reduce the distance of jumps and branches.
	/// </summary>
	public class SimpleTraceBlockOrderingStage : IMethodCompilerStage
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

		/// <summary>
		/// 
		/// </summary>
		private List<BasicBlock> blocks;
		/// <summary>
		/// 
		/// </summary>
		private BasicBlock firstBlock;

		#region IMethodCompilerStage Members

		#region ConnectedBlocks class

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

		#endregion

		/// <summary>
		/// Runs the specified compiler.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		public void Run(IMethodCompiler compiler)
		{
			// Retrieve the basic block provider
			IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));
			blocks = blockProvider.Blocks;

			// Retreive the first block
			firstBlock = blockProvider.FromLabel(-1);

			// Order the blocks based on loop depth
			OrderBlocks();
		}

		/// <summary>
		/// Orders the blocks.
		/// </summary>
		private void OrderBlocks()
		{
			// Create bit array of refereced blocks (by index)
			BitArray referencedBlocks = new BitArray(blocks.Count, false);

			// Create new list of ordered blocks
			int[] orderedBlocks = new int[blocks.Count];
			int orderBlockCnt = 0;

			// Create sorted worklist
			Stack<BasicBlock> workList = new Stack<BasicBlock>();

			// Start worklist with first block
			workList.Push(firstBlock);

			while (workList.Count != 0) {
				BasicBlock block = workList.Pop();

				referencedBlocks.Set(block.Index, true);
				orderedBlocks[orderBlockCnt++] = block.Index;

				foreach (BasicBlock successor in block.NextBlocks)
					if (!referencedBlocks.Get(successor.Index))
						workList.Push(successor);
			}

			// Place unreferenced blocks at the end of the list
			for (int i = 0; i < blocks.Count; i++)
				if (!referencedBlocks.Get(i))
					orderedBlocks[orderBlockCnt++] = i;

			// Reorder the block list
			for (int i = 0; i < orderBlockCnt; i++)
				blocks[orderedBlocks[i]].Index = i;

			blocks.Sort(BasicBlock.CompareBlocksByIndex);
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
