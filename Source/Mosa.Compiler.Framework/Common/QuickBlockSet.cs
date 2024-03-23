// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;

namespace Mosa.Compiler.Framework.Common;

public sealed class QuickBlockSet
{
	private readonly BasicBlocks BasicBlocks;
	private readonly BitArray ArraySet;

	public QuickBlockSet(BasicBlocks basicBlocks)
	{
		BasicBlocks = basicBlocks;
		ArraySet = new BitArray(basicBlocks.Count, false);
	}

	public QuickBlockSet(BasicBlocks basicBlocks, List<BasicBlock> blocks)
	{
		BasicBlocks = basicBlocks;
		ArraySet = new BitArray(basicBlocks.Count, false);

		if (blocks != null)
		{
			Add(blocks);
		}
	}

	public void Add(BasicBlock block)
	{
		ArraySet.Set(block.Sequence, true);
	}

	public void Add(List<BasicBlock> blocks)
	{
		foreach (var block in blocks)
		{
			ArraySet.Set(block.Sequence, true);
		}
	}

	public bool Contains(BasicBlock block)
	{
		return ArraySet.Get(block.Sequence);
	}

	public void Remove(BasicBlock block)
	{
		ArraySet.Set(block.Sequence, false);
	}

	public IEnumerable<BasicBlock> GetBlocks()
	{
		for (var i = 0; i < ArraySet.Count; i++)
		{
			if (ArraySet.Get(i))
			{
				yield return BasicBlocks[i];
			}
		}
	}
}
