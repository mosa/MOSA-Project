// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Analysis;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// IR Cleanup Stage
	/// </summary>
	public class IRCleanupStage : BaseTransformationStage
	{
		public IRCleanupStage() : base(false, true)
		{
		}

		protected override void Run()
		{
			RemoveNops();

			base.Run();

			OrderBlocks();
		}

		private void RemoveNops()
		{
			foreach (var block in BasicBlocks)
			{
				for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty || !node.IsNop)
						continue;

					node.Empty();
				}
			}
		}

		private void OrderBlocks()
		{
			var blockOrderAnalysis = new SimpleTraceBlockOrder();

			blockOrderAnalysis.Analyze(BasicBlocks);

			var newBlockOrder = blockOrderAnalysis.NewBlockOrder;

			newBlockOrder = AddMissingBlocksIfRequired(newBlockOrder);

			BasicBlocks.ReorderBlocks(newBlockOrder);
		}
	}
}
