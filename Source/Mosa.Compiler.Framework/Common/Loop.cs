// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Common
{
	public sealed class Loop
	{
		public BasicBlock Header { get; set; }
		public readonly List<BasicBlock> Backedges = new List<BasicBlock>();
		public readonly List<BasicBlock> LoopBlocks = new List<BasicBlock>();

		public Loop(BasicBlock header, BasicBlock backedge)
		{
			Header = header;
			Backedges.Add(backedge);
		}

		public void AddBackEdge(BasicBlock backedge)
		{
			Backedges.Add(backedge);
		}

		public void AddNode(BasicBlock block)
		{
			Debug.Assert(!LoopBlocks.Contains(block));
			LoopBlocks.Add(block);
		}
	}
}
