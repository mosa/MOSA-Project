/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.InternalTrace;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// The Block Ordering Stage reorders blocks to optimize loops and reduce the distance of jumps and branches.
	/// </summary>
	public class BlockOrderingStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{
		#region Data members

		private CompilerTrace trace;

		#endregion Data members

		#region IMethodCompilerStage Members

		/// <summary>
		/// Runs the specified compiler.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			trace = CreateTrace();

			var blockOrderAnalysis = methodCompiler.Compiler.CompilerOptions.BlockOrderAnalysisFactory();
			blockOrderAnalysis.PerformAnalysis(basicBlocks);

			basicBlocks.ReorderBlocks(blockOrderAnalysis.NewBlockOrder);

			if (trace.Active)
			{
				DumpTrace(blockOrderAnalysis);
			}
		}

		private void DumpTrace(IBlockOrderAnalysis blockOrderAnalysis)
		{
			int index = 0;
			foreach (var block in blockOrderAnalysis.NewBlockOrder)
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
				int depth = blockOrderAnalysis.GetLoopDepth(block);
				int depthindex = blockOrderAnalysis.GetLoopIndex(block);

				trace.Log("Block " + block.ToString() + " #" + block.Sequence.ToString() + " -> Depth: " + depth.ToString() + " index: " + depthindex.ToString());
			}
		}

		#endregion IMethodCompilerStage Members
	}
}