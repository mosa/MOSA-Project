// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.IR;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// IR Cleanup Stage
	/// </summary>
	public class IRCleanupStage : DeadBlockStage
	{
		protected override void Run()
		{
			if (!BasicBlocks.RuntimeValidation())
			{
				throw new CompilerException("IRCleanupStage (start): Block Validation Error in: " + Method);
			}

			RemoveNops();
			EmptyDeadBlocks();
			SkipEmptyBlocks();
			OrderBlocks();
		}

		private void OrderBlocks()
		{
			//var blockOrderAnalysis = new SimpleTraceBlockOrder();   // faster than others
			var blockOrderAnalysis = new LoopAwareBlockOrder();

			blockOrderAnalysis.Analyze(BasicBlocks);

			var newBlockOrder = blockOrderAnalysis.NewBlockOrder;

			if (newBlockOrder.Count != BasicBlocks.Count && HasProtectedRegions)
			{
				newBlockOrder = AddMissingBlocksIfRequired(newBlockOrder);
			}

			BasicBlocks.ReorderBlocks(newBlockOrder);

			if (!BasicBlocks.RuntimeValidation())
			{
				throw new CompilerException("IRCleanupStage: Block Validation Error in: " + Method);
			}
			Debug.Assert(BasicBlocks.RuntimeValidation());
		}

		private void RemoveNops()
		{
			foreach (var block in BasicBlocks)
			{
				for (var node = block.First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					if (node.IsNop)
					{
						node.Empty();
					}
				}
			}
		}
	}
}
