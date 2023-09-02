// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.BasicBlocks;

public class SkipEmptyBlocks : BaseBlockTransform
{
	public override bool Process(TransformContext transformContext)
	{
		var basicBlocks = transformContext.BasicBlocks;
		var hasProtectedRegions = transformContext.MethodCompiler.HasProtectedRegions;
		var isInSSAForm = transformContext.MethodCompiler.IsInSSAForm;
		var trace = transformContext.TraceLog;

		var emptied = 0;

		foreach (var block in basicBlocks)
		{
			if (block.NextBlocks.Count != 1)
				continue;

			if (block.IsPrologue || block.IsEpilogue)
				continue;

			if (block.IsHandlerHeadBlock || block.IsTryHeadBlock)
				continue;

			if (hasProtectedRegions && !block.IsCompilerBlock)
				continue;

			if (block.PreviousBlocks.Count == 0)
				continue;

			if (block.IsCompletelyEmpty)
				continue;

			if (block.PreviousBlocks.Contains(block))
				continue;

			if (!block.IsEmptyBlockWithSingleJump())
				continue;

			var hasPhi = isInSSAForm && block.NextBlocks[0].HasPhiInstruction();

			if (hasPhi && isInSSAForm && (block.PreviousBlocks.Count != 1 || block.NextBlocks[0].PreviousBlocks.Count != 1))
				continue;

			trace?.Log($"Removed Block: {block} - Skipped to: {block.NextBlocks[0]}");

			if (isInSSAForm)
			{
				PhiHelper.UpdatePhiTargets(block.NextBlocks, block, block.PreviousBlocks[0]);
			}

			block.RemoveEmptyBlockWithSingleJump(true);

			emptied++;
		}

		return emptied != 0;
	}
}
