// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Analysis;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// This stage inserts the garbage collection safe points.
/// </summary>
public class SafePointStage : BaseMethodCompilerStage
{
	private readonly Counter SafePointsInsertedCount = new("SafePoint.Total");

	private TraceLog trace;

	protected override void Initialize()
	{
		Register(SafePointsInsertedCount);
	}

	protected override void Run()
	{
		if (MethodCompiler.IsMethodPlugged)
			return;

		if (BasicBlocks.PrologueBlock == null)
			return;

		trace = CreateTraceLog(5);

		//var roots = CollectGCRoots();

		//if (trace != null)
		//{
		//	var rootTrace = CreateTraceLog("Roots", 5);

		//	rootTrace.Log($"GC Roots: Total = {roots.Count}");
		//	rootTrace.Log();

		//	foreach (var root in roots)
		//		rootTrace.Log($"  {root}");
		//}

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

		AnnotateSafePoints();
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

		SafePointsInsertedCount.Increment();

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

				SafePointsInsertedCount.Increment();

				trace?.Log($"SafePoint inserted at backedge: {backedge} -> {loop.Header}");
			}
		}
	}

	private void AnnotateSafePoints()
	{
		var numBlocks = BasicBlocks.Count;

		// Phase 1: Compute GEN and KILL for each block (forward pass).
		// GEN[B]  = physical registers first used as a GC root before any definition in B.
		// KILL[B] = all physical registers defined in B (including every register killed by KillAll).
		var gen = new HashSet<PhysicalRegister>[numBlocks];
		var kill = new HashSet<PhysicalRegister>[numBlocks];

		for (var i = 0; i < numBlocks; i++)
		{
			var block = BasicBlocks[i];
			var g = new HashSet<PhysicalRegister>();
			var k = new HashSet<PhysicalRegister>();

			for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmptyOrNop)
					continue;

				if (node.Instruction == IR.KillAll)
				{
					foreach (var physReg in Architecture.RegisterSet)
						k.Add(physReg);
					continue;
				}

				if (node.Instruction == IR.KillAllExcept)
				{
					var except = node.Operand1?.Register;
					foreach (var physReg in Architecture.RegisterSet)
						if (physReg != except)
							k.Add(physReg);
					continue;
				}

				foreach (var operand in node.Operands)
					if (operand.IsPhysicalRegister && IsGCRoot(operand) && !k.Contains(operand.Register))
						g.Add(operand.Register);

				foreach (var result in node.Results)
					if (result.IsPhysicalRegister)
						k.Add(result.Register);
			}

			gen[i] = g;
			kill[i] = k;
		}

		// Phase 2: Backward dataflow fixpoint.
		// liveOut[B] = union of liveIn[S] for all successors S.
		// liveIn[B]  = GEN[B] ∪ (liveOut[B] − KILL[B]).
		var liveIn = new HashSet<PhysicalRegister>[numBlocks];
		var liveOut = new HashSet<PhysicalRegister>[numBlocks];

		for (var i = 0; i < numBlocks; i++)
		{
			liveIn[i] = new HashSet<PhysicalRegister>();
			liveOut[i] = new HashSet<PhysicalRegister>();
		}

		bool changed;
		do
		{
			changed = false;

			for (var i = numBlocks - 1; i >= 0; i--)
			{
				var block = BasicBlocks[i];

				var newLiveOut = new HashSet<PhysicalRegister>();
				foreach (var succ in block.NextBlocks)
					newLiveOut.UnionWith(liveIn[succ.Sequence]);

				var newLiveIn = new HashSet<PhysicalRegister>(gen[i]);
				foreach (var r in newLiveOut)
					if (!kill[i].Contains(r))
						newLiveIn.Add(r);

				if (!newLiveOut.SetEquals(liveOut[i]) || !newLiveIn.SetEquals(liveIn[i]))
				{
					liveOut[i] = newLiveOut;
					liveIn[i] = newLiveIn;
					changed = true;
				}
			}
		} while (changed);

		// Phase 3: Annotate each IR.SafePoint with its live GC-root physical register operands.
		// For each block, traverse backward from liveOut[B], maintaining the live dictionary.
		var liveTrace = trace != null ? CreateTraceLog("LiveRegisters", 5) : null;

		foreach (var block in BasicBlocks)
		{
			var live = new Dictionary<PhysicalRegister, Operand>();
			foreach (var physReg in liveOut[block.Sequence])
				live[physReg] = null;

			for (var node = block.BeforeLast; !node.IsBlockStartInstruction; node = node.Previous)
			{
				if (node.IsEmptyOrNop)
					continue;

				if (node.Instruction == IR.SafePoint)
				{
					var liveRoots = new List<Operand>(live.Count);
					foreach (var (physReg, op) in live)
					{
						var resolved = op ?? FindGCRootOperand(physReg);
						if (resolved != null)
							liveRoots.Add(resolved);
					}

					if (liveRoots.Count > 0)
					{
						node.SetInstruction(IR.SafePoint, liveRoots.Count, 0);
						for (var j = 0; j < liveRoots.Count; j++)
							node.SetOperand(j, liveRoots[j]);
					}

					if (liveTrace != null)
					{
						liveTrace.Log($"SafePoint in {block}: {liveRoots.Count} live GC root(s)");
						foreach (var op in liveRoots)
							liveTrace.Log($"  {op}");
					}

					continue;
				}

				if (node.Instruction == IR.KillAll)
				{
					live.Clear();
					continue;
				}

				if (node.Instruction == IR.KillAllExcept)
				{
					var except = node.Operand1?.Register;
					foreach (var physReg in live.Keys.Where(r => r != except).ToList())
						live.Remove(physReg);
					continue;
				}

				// DEF kills the physical register from the live set.
				foreach (var result in node.Results)
					if (result.IsPhysicalRegister)
						live.Remove(result.Register);

				// USE adds GC-root physical registers to the live set with their operand.
				foreach (var operand in node.Operands)
					if (operand.IsPhysicalRegister && IsGCRoot(operand))
						live[operand.Register] = operand;
			}
		}
	}

	private static bool IsGCRoot(Operand operand) =>
		operand.IsObject || operand.IsManagedPointer;

	private Operand FindGCRootOperand(PhysicalRegister physReg)
	{
		foreach (var operand in Transform.PhysicalRegisters)
			if (operand.Register == physReg && IsGCRoot(operand))
				return operand;
		return null;
	}
}
