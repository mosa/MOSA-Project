// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Merges Blocks
	/// </summary>
	public class BlockMergeStage : BaseMethodCompilerStage
	{
		private Counter BlocksMergedCount = new Counter("BlockMergeStage.BlocksMerged");

		protected override void Initialize()
		{
			Register(BlocksMergedCount);
		}

		protected override void Run()
		{
			MergeBlocks();
		}

		private void MergeBlocks()
		{
			var changed = true;

			while (changed)
			{
				changed = false;

				foreach (var block in BasicBlocks)
				{
					if (block.IsEpilogue
						|| block.IsPrologue
						|| block.IsTryHeadBlock
						|| block.IsHandlerHeadBlock
						|| (!block.IsCompilerBlock && HasProtectedRegions))
						continue;

					if (block.NextBlocks.Count != 1)
						continue;

					var next = block.NextBlocks[0];

					if (next.PreviousBlocks.Count != 1)
						continue;

					if (next.IsEpilogue
						|| next.IsPrologue
						|| next.IsTryHeadBlock
						|| next.IsHandlerHeadBlock)
						continue;

					var insertPoint = block.BeforeLast.GoBackwardsToNonEmpty();

					var beforeInsertPoint = insertPoint.Previous.GoBackwardsToNonEmpty();

					if (beforeInsertPoint.BranchTargetsCount != 0)
					{
						Debug.Assert(beforeInsertPoint.BranchTargets[0] == next);
						beforeInsertPoint.Empty();
					}

					insertPoint.Empty();
					insertPoint.CutFrom(next.AfterFirst.GoForwardToNonEmpty(), next.Last.Previous.GoBackwardsToNonEmpty());
					BlocksMergedCount++;
					changed = true;
				}
			}
		}
	}
}
