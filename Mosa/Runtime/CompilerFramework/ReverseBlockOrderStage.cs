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
	/// The Reserver Block Order Stage reorders blocks in reverse order. This stage is for testing use only.
	/// </summary>
	public class ReverseBlockOrderStage : IMethodCompilerStage, IBasicBlockOrder
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		protected IArchitecture arch;
		/// <summary>
		/// 
		/// </summary>
		protected List<BasicBlock> blocks;
		/// <summary>
		/// 
		/// </summary>
		protected BasicBlock firstBlock;
		/// <summary>
		/// 
		/// </summary>
		protected int[] orderedBlocks;

		#endregion // Data members

		#region Properties

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public string Name
		{
			get { return @"Simple Trace Block Order"; }
		}

		/// <summary>
		/// Gets the ordered blocks.
		/// </summary>
		/// <value>The ordered blocks.</value>
		public int[] OrderedBlocks { get { return orderedBlocks; } }

		#endregion // Properties

		#region IMethodCompilerStage Members

		/// <summary>
		/// Runs the specified compiler.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		public void Run(IMethodCompiler compiler)
		{
			// Retrieve the basic block provider
			IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));

			if (blockProvider == null)
				throw new InvalidOperationException(@"Simple Trace Block Order stage requires basic blocks.");

			blocks = blockProvider.Blocks;

			// Retreive the first block
			firstBlock = blockProvider.FromLabel(-1);

			// Determines the block order
			DetermineBlockOrder();
		}

		/// <summary>
		/// Determines the block order.
		/// </summary>
		private void DetermineBlockOrder()
		{
			// Allocate list of ordered blocks
			orderedBlocks = new int[blocks.Count];

			Debug.Assert(firstBlock.Index == 0);
			orderedBlocks[0] = firstBlock.Index;
			int orderBlockCnt = 1;

			for (int i = blocks.Count - 1; i > 0; i--)
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
