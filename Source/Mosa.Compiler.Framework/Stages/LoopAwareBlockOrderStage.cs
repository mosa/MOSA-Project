/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.InternalTrace;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// The Loop Aware Block Ordering Stage reorders blocks to optimize loops and reduce the distance of jumps and branches.
	/// </summary>
	public class LoopAwareBlockOrderStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage, IBlockOrderStage
	{
		#region Data members

		private LoopAwareBlockOrder loopAwareBlockOrder;

		private CompilerTrace trace;

		#endregion Data members

		#region IMethodCompilerStage Members

		/// <summary>
		/// Runs the specified compiler.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			trace = CreateTrace();

			loopAwareBlockOrder = new LoopAwareBlockOrder(this.basicBlocks);

			basicBlocks.ReorderBlocks(loopAwareBlockOrder.NewBlockOrder);

			if (trace.Active)
			{ 
				DumpTrace(); 
			}
		}

		private void DumpTrace()
		{
			int index = 0;
			foreach (var block in loopAwareBlockOrder.NewBlockOrder)
			{
				if (block != null)
					trace.Log("# " + index.ToString() + " Block " + block.ToString() + " #" + block.Sequence.ToString());
				else
					trace.Log("# " + index.ToString() + " NONE");

				index++;
			}

			trace.Log(string.Empty);

			foreach (var block in basicBlocks)
			{
				int depth = loopAwareBlockOrder.GetLoopDepth(block);
				int depthindex = loopAwareBlockOrder.GetLoopIndex(block);

				trace.Log("Block " + block.ToString() + " #" + block.Sequence.ToString() + " -> Depth: " + depth.ToString() + " index: " + depthindex.ToString());
			}
		}

		#endregion IMethodCompilerStage Members
	}
}