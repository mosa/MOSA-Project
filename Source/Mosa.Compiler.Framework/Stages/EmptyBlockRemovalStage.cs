// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///	This stage removes empty blocks.
	/// </summary>
	public class EmptyBlockRemovalStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			RemoveEmptyBlocks();
		}

		protected void RemoveEmptyBlocks()
		{
			foreach (var block in BasicBlocks)
			{
				// don't remove unusual blocks (header blocks, return block, etc.)
				if (block.NextBlocks.Count == 0 || block.PreviousBlocks.Count == 0)
					continue;

				// don't remove block if it jumps back to itself
				if (block.PreviousBlocks.Contains(block))
					continue;

				if (HasProtectedRegions && !block.IsCompilerBlock)
					continue;

				if (block.IsHandlerHeadBlock)
					continue;

				if (block.IsTryHeadBlock)
					continue;

				if (!IsEmptyBlockWithSingleJump(block))
					continue;

				RemoveEmptyBlockWithSingleJump(block);
			}
		}
	}
}
