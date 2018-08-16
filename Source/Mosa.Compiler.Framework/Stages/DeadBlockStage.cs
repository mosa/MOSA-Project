// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///	This stage removes empty blocks.
	/// </summary>
	public class DeadBlockStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			EmptyDeadBlocks();
			SkipEmptyBlocks();
		}

		protected void EmptyDeadBlocks()
		{
			HashSet<BasicBlock> emptiedBlocks = null;
			bool changed = true;

			while (changed)
			{
				changed = false;

				foreach (var block in BasicBlocks)
				{
					if (block.IsPrologue || block.IsEpilogue)
						continue;

					if (block.IsHandlerHeadBlock || block.IsTryHeadBlock)
						continue;

					if (HasProtectedRegions && !block.IsCompilerBlock)
						continue;

					// don't remove block if it jumps back to itself
					if (block.PreviousBlocks.Contains(block))
						continue;

					if (block.PreviousBlocks.Count != 0)
						continue;

					if (emptiedBlocks != null && emptiedBlocks.Contains(block))
						continue;

					EmptyBlockOfAllInstructions(block);

					(emptiedBlocks ?? (emptiedBlocks = new HashSet<BasicBlock>())).Add(block);
					changed = true;
				}
			}
		}

		protected void SkipEmptyBlocks()
		{
			foreach (var block in BasicBlocks)
			{
				if (block.IsPrologue || block.IsEpilogue)
					continue;

				if (block.IsHandlerHeadBlock || block.IsTryHeadBlock)
					continue;

				if (HasProtectedRegions && !block.IsCompilerBlock)
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
