// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Common
{
	public sealed class QuickBlockSetLookup
	{
		private readonly BitArray array;

		public QuickBlockSetLookup(BasicBlocks basicBlocks)
		{
			array = new BitArray(basicBlocks.Count, false);
		}

		public QuickBlockSetLookup(BasicBlocks basicBlocks, List<BasicBlock> blocks)
		{
			array = new BitArray(basicBlocks.Count, false);

			if (blocks != null)
			{
				Add(blocks);
			}
		}

		public void Add(List<BasicBlock> blocks)
		{
			foreach (var block in blocks)
			{
				array.Set(block.Sequence, true);
			}
		}

		public bool Contains(BasicBlock block)
		{
			return array.Get(block.Sequence);
		}

		public void Add(BasicBlock block)
		{
			array.Set(block.Sequence, true);
		}

		public void Remove(BasicBlock block)
		{
			array.Set(block.Sequence, false);
		}

		public IEnumerable<BasicBlock> GetBlocks(BasicBlocks basicBlocks)
		{
			for (int i = 0; i < array.Count; i++)
			{
				if (array.Get(i))
				{
					yield return basicBlocks[i];
				}
			}
		}
	}
}
