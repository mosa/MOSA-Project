// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Analysis;

namespace Mosa.Compiler.Framework.Stages;

// TODO:
// 1. Analyze method and determine:
//    A. Determine if method contains any references to objects, or have any managed pointers
//    B. Determine start/end ranges of objects and managed pointers on the local stack and parameters
//    C. Place SafePoint at:
//       i.  Method prologue
//       ii. Each loop backedge
// 2. Add SafePoint and for each determine which registers contain objects or managed pointers

/// <summary>
/// This stage inserts the garbage collection safe points.
/// </summary>
public class SafePointStage : BaseMethodCompilerStage
{
	private TraceLog trace;

	protected override void Run()
	{
		if (MethodCompiler.IsMethodPlugged)
			return;

		trace = CreateTraceLog();

		var loops = LoopDetector.FindLoops(BasicBlocks);

		if (trace != null)
		{
			var loopTrace = CreateTraceLog("Loops");

			loopTrace.Log($"Loops: Total = {loops.Count}");
			loopTrace.Log();

			LoopDetector.DumpLoops(loopTrace, loops);
		}
	}

	protected override void Finish()
	{
		trace = null;
	}
}
