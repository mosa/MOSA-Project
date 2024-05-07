﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Analysis;

public class ProtectedRegion
{
	public MosaExceptionHandler Handler { get; }

	public List<BasicBlock> HandlerBlocks { get; } = new List<BasicBlock>();

	private readonly HashSet<BasicBlock> included = new();
	private readonly HashSet<BasicBlock> excluded = new();

	public ProtectedRegion(BasicBlocks basicBlocks, MosaExceptionHandler exceptionHandler)
	{
		Handler = exceptionHandler;

		foreach (var block in basicBlocks)
		{
			//if (block.Label >= exceptionHandler.TryStart && block.Label < exceptionHandler.TryEnd)
			if (exceptionHandler.IsLabelWithinTry(block.Label))
				included.Add(block);
			else if (!block.IsCompilerBlock)
				excluded.Add(block);
		}
	}

	public void Finalize(BasicBlocks basicBlocks)
	{
		foreach (var block in included)
		{
			if (!basicBlocks.Contains(block))
				continue;

			Trace(block);
		}
	}

	private void Trace(BasicBlock block)
	{
		if (excluded.Contains(block))
			return;

		if (HandlerBlocks.Contains(block))
			return;

		HandlerBlocks.Add(block);

		foreach (var next in block.NextBlocks)
			Trace(next);

		foreach (var prev in block.PreviousBlocks)
			Trace(prev);
	}

	public static List<ProtectedRegion> CreateProtectedRegions(BasicBlocks basicBlocks, IList<MosaExceptionHandler> exceptionHandlers)
	{
		var protectedRegions = new List<ProtectedRegion>(exceptionHandlers.Count);

		foreach (var handler in exceptionHandlers)
		{
			var protectedRegion = new ProtectedRegion(basicBlocks, handler);
			protectedRegions.Add(protectedRegion);
		}

		return protectedRegions;
	}

	public static void FinalizeAll(BasicBlocks basicBlocks, List<ProtectedRegion> protectedRegions)
	{
		foreach (var region in protectedRegions)
		{
			region.Finalize(basicBlocks);
		}
	}
}
