// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Analysis;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// This stage inserts the garbage collection safe points.
/// </summary>
public class SafePointStage : BaseMethodCompilerStage
{
	private const int SmallLoopThreshold = 64;

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
			//if (IsSmallBoundedLoop(loop))
			//{
			//	trace?.Log($"SafePoint skipped (small bounded loop): {loop.Header}");
			//	continue;
			//}

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

	private bool IsSmallBoundedLoop(Loop loop)
	{
		var blocksToSearch = loop.Backedges.Count == 1
			? new[] { loop.Header, loop.Backedges[0] }
			: new[] { loop.Header };

		foreach (var block in blocksToSearch)
		{
			for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmptyOrNop)
					continue;

				if (node.Instruction != IR.Branch32 && node.Instruction != IR.Branch64)
					continue;

				if (TryGetExitBound(node, loop, out var bound))
					return bound >= 0 && bound < SmallLoopThreshold;
			}
		}

		return false;
	}

	private static bool TryGetExitBound(Node node, Loop loop, out long bound)
	{
		bound = 0;

		if (node.Block.NextBlocks.Count != 2)
			return false;

		var x = node.Operand1;
		var y = node.Operand2;
		var condition = node.ConditionCode;
		var target = node.BranchTarget1;
		var otherTarget = target != node.Block.NextBlocks[0]
			? node.Block.NextBlocks[0]
			: node.Block.NextBlocks[1];

		// Normalize: make target the exit block (outside the loop)
		if (loop.LoopBlocks.Contains(target))
		{
			(condition, target, otherTarget) = (condition.GetOpposite(), otherTarget, target);
		}

		if (loop.LoopBlocks.Contains(target))
			return false;

		// Normalize: put the constant on the right side
		if (!y.IsResolvedConstant)
		{
			(x, y, condition) = (y, x, condition.GetOpposite());
		}

		if (!y.IsResolvedConstant)
			return false;

		// Exit when x >= y (or x > y) → loop iterated at most y (or y+1) times from zero
		if (condition is not (ConditionCode.Greater
			or ConditionCode.GreaterOrEqual
			or ConditionCode.UnsignedGreater
			or ConditionCode.UnsignedGreaterOrEqual))
			return false;

		var adj = condition is ConditionCode.GreaterOrEqual
			or ConditionCode.UnsignedGreaterOrEqual ? 0 : 1;

		bound = y.IsInt32
			? (long)y.ConstantSigned32 + adj
			: y.ConstantSigned64 + adj;

		return true;
	}
}
