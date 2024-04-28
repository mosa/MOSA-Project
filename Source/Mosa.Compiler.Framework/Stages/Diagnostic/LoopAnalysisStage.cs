// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Analysis;

namespace Mosa.Compiler.Framework.Stages.Diagnostic;

/// <summary>
/// Loop Analysis Stage
/// </summary>
public sealed class LoopAnalysisStage : BaseMethodCompilerStage
{
	private readonly Counter LoopCount = new("LoopAnalysis.LoopCount");
	private readonly Counter LoopSize1 = new("LoopAnalysis.LoopSize-1");
	private readonly Counter LoopSize2 = new("LoopAnalysis.LoopSize-2");
	private readonly Counter LoopSize3 = new("LoopAnalysis.LoopSize-3");
	private readonly Counter LoopSize4 = new("LoopAnalysis.LoopSize-4");
	private readonly Counter LoopSize5Plus = new("LoopAnalysis.LoopSize-5+");

	protected override void Initialize()
	{
		Register(LoopCount);
		Register(LoopSize1);
		Register(LoopSize2);
		Register(LoopSize3);
		Register(LoopSize4);
		Register(LoopSize5Plus);
	}

	protected override void Run()
	{
		// Method is empty - must be a plugged method
		if (HasProtectedRegions)
			return;

		if (BasicBlocks.HeadBlocks.Count != 1)
			return;

		if (BasicBlocks.PrologueBlock == null)
			return;

		OutputLoops();
	}

	private void OutputLoops()
	{
		var loops = LoopDetector.FindLoops(BasicBlocks);

		LoopCount.Set(loops.Count);

		foreach (var loop in loops)
		{
			var size = loop.LoopBlocks.Count;

			switch (size)
			{
				case 1: LoopSize1.Increment(); break;
				case 2: LoopSize2.Increment(); break;
				case 3: LoopSize3.Increment(); break;
				case 4: LoopSize4.Increment(); break;
				default:
					LoopSize5Plus.Increment(); break;
			}
		}
	}
}
