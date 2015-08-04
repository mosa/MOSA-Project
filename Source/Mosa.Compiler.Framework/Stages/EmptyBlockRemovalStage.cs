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
			// don't remove any blocks when the flow control is unusual
			if (HasProtectedRegions)
				return;

			foreach (var block in BasicBlocks)
			{
				// don't process other unusual blocks (header blocks, return block, etc.)
				if (block.NextBlocks.Count == 0 || block.PreviousBlocks.Count == 0)
					continue;

				// don't remove block if it jumps back to itself
				if (block.PreviousBlocks.Contains(block))
					continue;

				if (!IsEmptyBlockWithSingleJump(block))
					continue;

				RemoveEmptyBlockWithSingleJump(block);
			}
		}
	}
}
