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
	/// The Simple Trace Block Order Stage reorders Blocks to optimize loops and reduce the distance of jumps and branches.
	/// </summary>
	public class SimpleTraceBlockOrderStage : BaseStage, IMethodCompilerStage, IPipelineStage, IBlockOrderStage
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		protected BasicBlock[] _ordered;

		#endregion // Data members

		#region IMethodCompilerStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name
		{
			get { return @"Simple Trace Block Order"; }
		}

		/// <summary>
		/// Runs the specified compiler.
		/// </summary>
		public void Run()
		{
			// Retreive the first block
			BasicBlock first = FindBlock(-1);

			// Create dictionary of refereced blocks
			Dictionary<BasicBlock, int> referenced = new Dictionary<BasicBlock, int>(BasicBlocks.Count);

			// Allocate list of ordered Blocks
			_ordered = new BasicBlock[BasicBlocks.Count];
			int orderBlockCnt = 0;

			// Create sorted worklist
			Stack<BasicBlock> workList = new Stack<BasicBlock>();

			// Start worklist with first block
			workList.Push(first);

			while (workList.Count != 0) {
				BasicBlock block = workList.Pop();

				if (!referenced.ContainsKey(block)) {
					referenced.Add(block, 0);
					_ordered[orderBlockCnt++] = block;

					foreach (BasicBlock successor in block.NextBlocks)
						if (!referenced.ContainsKey(successor))
							workList.Push(successor);
				}
			}

			// Place unreferenced Blocks at the end of the list
			foreach (BasicBlock block in BasicBlocks)
				if (!referenced.ContainsKey(block))
					_ordered[orderBlockCnt++] = block;
		}

		#endregion // Methods
	}
}
