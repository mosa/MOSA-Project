// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Analysis;

namespace Mosa.Compiler.Framework.Stages;

// TODO:
// 2. For each SafePoint determine which registers contain objects or managed pointers
//    (requires SafePoint instruction to support variable operands via the source generator,
//    and liveness data to be available at this stage)

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

		// TODO 1A: Determine if method contains any references to objects or managed pointers
		if (!HasGCRoots())
			return;

		// TODO 1B: Collect and trace GC root operands from locals and parameters
		var roots = CollectGCRoots();

		if (trace != null)
		{
			var rootTrace = CreateTraceLog("Roots", 5);

			rootTrace.Log($"GC Roots: Total = {roots.Count}");
			rootTrace.Log();

			foreach (var root in roots)
				rootTrace.Log($"  {root}");
		}

		// TODO 1C.i: Insert SafePoint at method prologue
		InsertSafePointAtPrologue();

		// TODO 1C.ii: Insert SafePoint at each loop backedge
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

	private bool HasGCRoots()
	{
		foreach (var operand in MethodCompiler.LocalStack)
			if (operand.IsObject || operand.IsManagedPointer)
				return true;

		foreach (var operand in MethodCompiler.Parameters)
			if (operand.IsObject || operand.IsManagedPointer)
				return true;

		return false;
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
