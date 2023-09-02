// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Transforms.BasicBlocks;

public class RemoveUnreachableBlocks : BaseBlockTransform
{
	public override int Process(TransformContext transformContext)
	{
		var basicBlocks = transformContext.BasicBlocks;
		var hasProtectedRegions = transformContext.MethodCompiler.HasProtectedRegions;
		var trace = transformContext.TraceLog;

		var emptied = 0;

		var stack = new Stack<BasicBlock>();
		var bitmap = new BitArray(basicBlocks.Count, false);

		foreach (var block in basicBlocks)
		{
			if (block.IsHeadBlock || block.IsHandlerHeadBlock || block.IsTryHeadBlock)
			{
				bitmap.Set(block.Sequence, true);
				stack.Push(block);
			}
		}

		while (stack.Count != 0)
		{
			var block = stack.Pop();

			//trace?.Log($"Used Block: {block}");

			foreach (var next in block.NextBlocks)
			{
				//trace?.Log($"Visited Block: {block}");

				if (!bitmap.Get(next.Sequence))
				{
					bitmap.Set(next.Sequence, true);
					stack.Push(next);
				}
			}
		}

		foreach (var block in basicBlocks)
		{
			if (bitmap.Get(block.Sequence))
				continue;

			if (block.IsHandlerHeadBlock || block.IsTryHeadBlock)
				continue;

			if (hasProtectedRegions && !block.IsCompilerBlock)
				continue;

			if (block.IsCompletelyEmpty)
				continue;

			var nextBlocks = block.NextBlocks.ToArray();

			block.EmptyBlockOfAllInstructions(true);
			PhiHelper.UpdatePhiBlocks(nextBlocks);

			emptied++;

			trace?.Log($"Removed Unreachable Block: {block}");
		}

		return emptied;
	}
}
