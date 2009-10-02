/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// The Reserver Block Order Stage reorders Blocks in reverse order. This stage is for testing use only.
	/// </summary>
	public class ReverseBlockOrderStage : BaseStage, IMethodCompilerStage, IBasicBlockOrder
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		protected IArchitecture Architecture;
		/// <summary>
		/// 
		/// </summary>
		protected List<BasicBlock> Blocks;
		/// <summary>
		/// 
		/// </summary>
		protected BasicBlock FirstBlock;
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
		/// Gets the ordered Blocks.
		/// </summary>
		/// <value>The ordered Blocks.</value>
		public int[] OrderedBlocks { get { return orderedBlocks; } }

		#endregion // Properties

		#region IMethodCompilerStage Members

		/// <summary>
		/// Runs the specified compiler.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		public override void Run(IMethodCompiler compiler)
		{
			base.Run(compiler);

			// Retreive the first block
			FirstBlock = FindBlock(-1);

			// Determines the block order
			DetermineBlockOrder();
		}

		/// <summary>
		/// Determines the block order.
		/// </summary>
		private void DetermineBlockOrder()
		{
			// Allocate list of ordered Blocks
			orderedBlocks = new int[Blocks.Count];

			Debug.Assert(FirstBlock.Index == 0);
			orderedBlocks[0] = FirstBlock.Index;
			int orderBlockCnt = 1;

			for (int i = Blocks.Count - 1; i > 0; i--)
				orderedBlocks[orderBlockCnt++] = i;
		}

		/// <summary>
		/// Adds to pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline.</param>
		public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
			pipeline.InsertBefore<CIL.CilToIrTransformationStage>(this);
		}

		#endregion // Methods
	}
}
