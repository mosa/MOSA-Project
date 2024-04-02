// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Framework;

public sealed class Loop
{
	public BasicBlock Header { get; set; }
	public readonly List<BasicBlock> Backedges = new();
	public readonly List<BasicBlock> LoopBlocks = new();
	//private readonly HashSet<BasicBlock> LoopBlocksSet = new();

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
		//LoopBlocksSet.Add(block);
	}

	//public bool Contains(BasicBlock block)
	//{
	//	return LoopBlocksSet.Contains(block);
	//}
}
