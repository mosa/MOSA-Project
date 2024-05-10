// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;

namespace Mosa.Compiler.Framework.Common;

public sealed class BlockBitSet
{
	private readonly BitArray ArraySet;

	public BlockBitSet(BasicBlocks basicBlocks)
	{
		ArraySet = new BitArray(basicBlocks.Count, false);
	}

	public BlockBitSet(BasicBlocks basicBlocks, List<BasicBlock> blocks)
	{
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

	public void Or(BlockBitSet set)
	{
		ArraySet.Or(set.ArraySet);
	}

	public void And(BlockBitSet set)
	{
		ArraySet.And(set.ArraySet);
	}

	public IEnumerable<BasicBlock> GetBlocks(BasicBlocks basicBlocks)
	{
		for (var i = 0; i < ArraySet.Count; i++)
		{
			if (ArraySet.Get(i))
			{
				yield return basicBlocks[i];
			}
		}
	}
}
