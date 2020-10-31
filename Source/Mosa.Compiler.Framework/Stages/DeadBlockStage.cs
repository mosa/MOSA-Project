// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///	This stage removes dead and empty blocks.
	/// </summary>
	public class DeadBlockStage : BaseMethodCompilerStage
	{
		// WARNING!!!! This stage is not PHI instruction aware!

		private readonly Counter DeadBlocksRemovedCount = new Counter("DeadBlockStage.DeadBlocksRemoved");
		private Counter EmptyBlocksRemovedCount = new Counter("DeadBlockStage.EmptyBlocksRemoved");
		private Counter SkipEmptyBlocksCount = new Counter("DeadBlockStage.SkipEmptyBlocks");

		protected override void Initialize()
		{
			Register(EmptyBlocksRemovedCount);
			Register(DeadBlocksRemovedCount);
			Register(SkipEmptyBlocksCount);
		}

		protected override void Run()
		{
			EmptyDeadBlocks();
			SkipEmptyBlocks();
			RemoveDeadBlocks();
		}

		protected void EmptyDeadBlocks()
		{
			bool changed = true;

			while (changed)
			{
				changed = false;

				foreach (var block in BasicBlocks)
				{
					if (block.IsKnownEmpty)
						continue;

					if (block.IsPrologue || block.IsEpilogue)
						continue;

					if (block.IsHandlerHeadBlock || block.IsTryHeadBlock)
						continue;

					if (HasProtectedRegions && !block.IsCompilerBlock)
						continue;

					if (block.PreviousBlocks.Count != 0)
						continue;

					// don't remove block if it jumps back to itself
					if (block.PreviousBlocks.Contains(block))
						continue;

					if (EmptyBlockOfAllInstructions(block))
					{
						EmptyBlocksRemovedCount++;
						changed = true;
					}
				}
			}
		}

		protected void SkipEmptyBlocks()
		{
			foreach (var block in BasicBlocks)
			{
				if (block.IsKnownEmpty)
					continue;

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
				SkipEmptyBlocksCount++;
			}
		}

		public void RemoveDeadBlocks()
		{
			var list = new List<BasicBlock>(BasicBlocks.Count);

			foreach (var block in BasicBlocks)
			{
				if (block.HasNextBlocks
					|| block.HasPreviousBlocks
					|| block.IsHandlerHeadBlock
					|| block.IsTryHeadBlock
					|| block.IsEpilogue
					|| block.IsPrologue
					|| (HasProtectedRegions && !block.IsCompilerBlock)
					|| block.IsHeadBlock)
				{
					list.Add(block);
				}
			}

			DeadBlocksRemovedCount.Count = BasicBlocks.Count - list.Count;

			if (list.Count == BasicBlocks.Count)
				return;

			BasicBlocks.ReorderBlocks(list);
		}
	}
}
