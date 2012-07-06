/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

// FIXME PG


namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// The Loop Aware Block Ordering Stage reorders blocks to optimize loops and reduce the distance of jumps and branches.
	/// </summary>
	public class LoopAwareBlockOrderStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage, IBlockOrderStage
	{
		#region Data members

		LoopAwareBlockOrder loopAwareBlockOrder;

		#endregion // Data members

		#region IMethodCompilerStage Members

		/// <summary>
		/// Runs the specified compiler.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			loopAwareBlockOrder = new LoopAwareBlockOrder(this.basicBlocks);

			basicBlocks.ReorderBlocks(loopAwareBlockOrder.NewBlockOrder);

			if (IsLogging)
				DumpTrace();
		}

		private void DumpTrace()
		{
			int index = 0;
			foreach (var block in loopAwareBlockOrder.NewBlockOrder)
			{
				if (block != null)
					Trace("# " + index.ToString() + " Block " + block.ToString() + " #" + block.Sequence.ToString());
				else
					Trace("# " + index.ToString() + " NONE");
				index++;
			}

			Trace(string.Empty);

			foreach (var block in basicBlocks)
			{
				int depth = loopAwareBlockOrder.GetLoopDepth(block);
				int depthindex = loopAwareBlockOrder.GetLoopIndex(block);

				Trace("Block " + block.ToString() + " #" + block.Sequence.ToString() + " -> Depth: " + depth.ToString() + " index: " + depthindex.ToString());
			}

		}

		#endregion // Methods
	}
}
