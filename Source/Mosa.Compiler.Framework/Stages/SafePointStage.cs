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

	private const int MaxBitmapRegisterCount = 32;

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

	#region Insertion

	private void InsertSafePointAtPrologue()
	{
		BasicBlocks.PrologueBlock.ContextBeforeBranch.InsertAfter().SetInstruction(IR.SafePoint);

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

				backedge.ContextBeforeBranch.InsertAfter().SetInstruction(IR.SafePoint);

				SafePointsInsertedCount.Increment();

				trace?.Log($"SafePoint inserted at backedge: {backedge} -> {loop.Header}");
			}
		}
	}

	#endregion Insertion

	#region Annotation

	private void AnnotateSafePoints()
	{
		var numBlocks = BasicBlocks.Count;

		// Phase 1: Compute GEN and KILL bitmaps for each block (forward pass).
		// genObject[B] = object-reference registers first used before any definition in B.
		// genMPtr[B]   = managed-pointer registers first used before any definition in B.
		// kill[B]      = all physical registers defined in B (kills both GC root types).
		var genObject = new uint[numBlocks];
		var genMPtr = new uint[numBlocks];
		var kill = new uint[numBlocks];

		for (var i = 0; i < numBlocks; i++)
		{
			var block = BasicBlocks[i];
			uint go = 0, gm = 0, k = 0;

			for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmptyOrNop)
					continue;

				if (node.Instruction == IR.KillAll)
				{
					k = uint.MaxValue;
					continue;
				}

				if (node.Instruction == IR.KillAllExcept)
				{
					var exceptBit = TryGetBitmapBit(node.Operand1, out var bit) ? bit : 0;
					k |= ~exceptBit;
					continue;
				}

				foreach (var operand in node.Operands)
				{
					if (!TryGetBitmapBit(operand, out var bit))
						continue;

					if ((k & bit) == 0)
					{
						if (operand.IsObject) go |= bit;
						else if (operand.IsManagedPointer) gm |= bit;
					}
				}

				foreach (var result in node.Results)
				{
					if (TryGetBitmapBit(result, out var resultBit))
						k |= resultBit;
				}
			}

			genObject[i] = go;
			genMPtr[i] = gm;
			kill[i] = k;
		}

		// Phase 2: Backward dataflow fixpoint for object and managed-pointer roots.
		// liveOut[B] = union of liveIn[S] for all successors S.
		// liveIn[B]  = GEN[B] | (liveOut[B] & ~KILL[B]).
		var liveInObject = new uint[numBlocks];
		var liveOutObject = new uint[numBlocks];
		var liveInMPtr = new uint[numBlocks];
		var liveOutMPtr = new uint[numBlocks];

		bool changed;
		do
		{
			changed = false;

			for (var i = numBlocks - 1; i >= 0; i--)
			{
				var block = BasicBlocks[i];

				uint newLiveOutObject = 0, newLiveOutMPtr = 0;
				foreach (var succ in block.NextBlocks)
				{
					newLiveOutObject |= liveInObject[succ.Sequence];
					newLiveOutMPtr |= liveInMPtr[succ.Sequence];
				}

				var newLiveInObject = genObject[i] | (newLiveOutObject & ~kill[i]);
				var newLiveInMPtr = genMPtr[i] | (newLiveOutMPtr & ~kill[i]);

				if (newLiveOutObject != liveOutObject[i] || newLiveInObject != liveInObject[i]
					|| newLiveOutMPtr != liveOutMPtr[i] || newLiveInMPtr != liveInMPtr[i])
				{
					liveOutObject[i] = newLiveOutObject;
					liveInObject[i] = newLiveInObject;
					liveOutMPtr[i] = newLiveOutMPtr;
					liveInMPtr[i] = newLiveInMPtr;
					changed = true;
				}
			}
		} while (changed);

		// Phase 3: Annotate each IR.SafePoint with register and type bitmaps encoded as two
		// constant operands: Operand1 = registerBitmap, Operand2 = typeBitmap (1 = managed pointer).
		// For each block, traverse backward from liveOut[B], maintaining two live bitmaps.
		var liveTrace = trace != null ? CreateTraceLog("LiveRegisters", 5) : null;

		foreach (var block in BasicBlocks)
		{
			var liveObject = liveOutObject[block.Sequence];
			var liveMPtr = liveOutMPtr[block.Sequence];

			for (var node = block.BeforeLast; !node.IsBlockStartInstruction; node = node.Previous)
			{
				if (node.IsEmptyOrNop)
					continue;

				if (node.Instruction == IR.SafePoint)
				{
					var registerBitmap = liveObject | liveMPtr;
					var typeBitmap = liveMPtr;

					node.SetOperand(0, Operand.CreateConstant32(registerBitmap));
					node.SetOperand(1, Operand.CreateConstant32(typeBitmap));

					liveTrace?.Log($"SafePoint in {block}: registers=0x{registerBitmap:X8} managed-pointers=0x{typeBitmap:X8}");

					break;
				}

				if (node.Instruction == IR.KillAll)
				{
					liveObject = 0;
					liveMPtr = 0;
					continue;
				}

				if (node.Instruction == IR.KillAllExcept)
				{
					var exceptBit = TryGetBitmapBit(node.Operand1, out var bit) ? bit : 0;
					liveObject &= exceptBit;
					liveMPtr &= exceptBit;
					continue;
				}

				// DEF kills the physical register from both live bitmaps.
				foreach (var result in node.Results)
				{
					if (!TryGetBitmapBit(result, out var bit))
						continue;

					var killBit = ~bit;
					liveObject &= killBit;
					liveMPtr &= killBit;
				}

				// USE adds GC-root physical registers to the appropriate live bitmap.
				foreach (var operand in node.Operands)
				{
					if (!TryGetBitmapBit(operand, out var bit))
						continue;

					if (operand.IsObject) liveObject |= bit;
					else if (operand.IsManagedPointer) liveMPtr |= bit;
				}
			}
		}
	}

	private static bool TryGetBitmapBit(Operand operand, out uint bit)
	{
		bit = 0;

		if (operand == null || !operand.IsPhysicalRegister)
			return false;

		var register = operand.Register;
		if (register == null || register.IsSpecial)
			return false;

		if ((uint)register.Index >= MaxBitmapRegisterCount)
			return false;

		bit = 1u << register.Index;
		return true;
	}

	#endregion Annotation
}
