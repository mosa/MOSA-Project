// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Framework;

public sealed class Loop
{
	public BasicBlock Header { get; set; }
	public readonly List<BasicBlock> Backedges = new();
	public readonly List<BasicBlock> LoopBlocks = new();

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
