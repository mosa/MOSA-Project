// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Framework.Transforms.BasicBlocks;

public abstract class MergeBlocks : BaseBlockTransform
{
	public override bool Process(TransformContext transformContext)
	{
		var basicBlocks = transformContext.BasicBlocks;
		var hasProtectedRegions = transformContext.MethodCompiler.HasProtectedRegions;
		var isInSSAForm = transformContext.MethodCompiler.IsInSSAForm;
		var trace = transformContext.TraceLog;

		var emptied = 0;
		var changed = true;

		while (changed)
		{
			changed = false;

			foreach (var block in basicBlocks)
			{
				if (block.NextBlocks.Count != 1)
					continue;

				if (block.IsEpilogue
					|| block.IsPrologue
					|| block.IsTryHeadBlock
					|| block.IsHandlerHeadBlock
					|| (!block.IsCompilerBlock && hasProtectedRegions))
					continue;

				// don't remove block if it jumps back to itself
				if (block.PreviousBlocks.Contains(block))
					continue;

				var next = block.NextBlocks[0];

				if (next.PreviousBlocks.Count != 1)
					continue;

				if (next.IsEpilogue
					|| next.IsPrologue
					|| next.IsTryHeadBlock
					|| next.IsHandlerHeadBlock)
					continue;

				trace?.Log($"Merge Blocking: {block} with: {next}");

				if (isInSSAForm)
				{
					PhiHelper.UpdatePhiTargets(next.NextBlocks, next, block);
				}

				var insertPoint = block.BeforeLast.BackwardsToNonEmpty;

				var beforeInsertPoint = insertPoint.Previous.BackwardsToNonEmpty;

				if (beforeInsertPoint.BranchTargetsCount != 0)
				{
					Debug.Assert(beforeInsertPoint.BranchTargets[0] == next);
					beforeInsertPoint.Empty();
				}

				insertPoint.Empty();
				insertPoint.MoveFrom(next.AfterFirst.ForwardToNonEmpty, next.Last.Previous.BackwardsToNonEmpty);
				emptied++;
				changed = true;
			}
		}

		return emptied != 0;
	}
}
