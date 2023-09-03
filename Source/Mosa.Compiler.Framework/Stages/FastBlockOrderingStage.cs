// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Analysis;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// This stage quickly reorders branches.
/// </summary>
public class FastBlockOrderingStage : BaseTransformStage
{
	protected override void Run()
	{
		var blockOrderAnalysis = new SimpleTraceBlockOrder();

		blockOrderAnalysis.Analyze(BasicBlocks);

		var newBlockOrder = blockOrderAnalysis.NewBlockOrder;

		newBlockOrder = AddMissingBlocksIfRequired(newBlockOrder);

		BasicBlocks.ReorderBlocks(newBlockOrder);
	}
}
