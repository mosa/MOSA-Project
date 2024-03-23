// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;
using Mosa.Compiler.Framework.Common;

namespace Mosa.Compiler.Framework.Transforms.BasicBlocks;

public class RemoveUnreachableBlocks : BaseBlockTransform
{
	public override int Process(Transform transform)
	{
		var basicBlocks = transform.BasicBlocks;
		var hasProtectedRegions = transform.MethodCompiler.HasProtectedRegions;
		var trace = transform.TraceLog;

		var emptied = 0;

		var stack = new Stack<BasicBlock>();
		var bitmap = new BlockBitSet(basicBlocks);

		foreach (var block in basicBlocks)
		{
			if (block.IsHeadBlock || block.IsHandlerHeadBlock || block.IsTryHeadBlock)
			{
				bitmap.Add(block);
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

				if (!bitmap.Contains(next))
				{
					bitmap.Add(next);
					stack.Push(next);
				}
			}
		}

		foreach (var block in basicBlocks)
		{
			if (bitmap.Contains(block))
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
