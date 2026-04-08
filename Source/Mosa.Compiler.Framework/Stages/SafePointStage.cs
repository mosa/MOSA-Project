// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Analysis;

namespace Mosa.Compiler.Framework.Stages;

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

		if (BasicBlocks.PrologueBlock == null)
			return;

		trace = CreateTraceLog(5);

		var roots = CollectGCRoots();

		if (roots.Count == 0)
			return;

		if (trace != null)
		{
			var rootTrace = CreateTraceLog("Roots", 5);

			rootTrace.Log($"GC Roots: Total = {roots.Count}");
			rootTrace.Log();

			foreach (var root in roots)
				rootTrace.Log($"  {root}");
		}

		InsertSafePointAtPrologue();

		var loops = LoopDetector.FindLoops(BasicBlocks);

		if (trace != null)
		{
			var loopTrace = CreateTraceLog("Loops", 5);

			loopTrace.Log($"Loops: Total = {loops.Count}");
			loopTrace.Log();

			LoopDetector.DumpLoops(loopTrace, loops);
		}

		InsertSafePointsAtBackedges(loops);
	}

	protected override void Finish()
	{
		trace = null;
	}

	private List<Operand> CollectGCRoots()
	{
		var roots = new List<Operand>();

		foreach (var operand in MethodCompiler.LocalStack)
			if (operand.IsObject || operand.IsManagedPointer)
				roots.Add(operand);

		foreach (var operand in MethodCompiler.Parameters)
			if (operand.IsObject || operand.IsManagedPointer)
				roots.Add(operand);

		return roots;
	}

	private void InsertSafePointAtPrologue()
	{
		var ctx = new Context(BasicBlocks.PrologueBlock);
		ctx.AppendInstruction(IR.SafePoint);

		trace?.Log($"SafePoint inserted at prologue: {BasicBlocks.PrologueBlock}");
	}

	private void InsertSafePointsAtBackedges(List<Loop> loops)
	{
		var visited = new HashSet<BasicBlock>();

		foreach (var loop in loops)
		{
			foreach (var backedge in loop.Backedges)
			{
				if (!visited.Add(backedge))
					continue;

				var ctx = new Context(backedge.BeforeLast);
				ctx.InsertBefore().SetInstruction(IR.SafePoint);

				trace?.Log($"SafePoint inserted at backedge: {backedge} -> {loop.Header}");
			}
		}
	}
}
