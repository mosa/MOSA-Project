// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Analysis;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// Loop Range Tracker Stage
/// </summary>
public sealed class LoopRangeTrackerStage : BaseMethodCompilerStage
{
	private readonly Counter MinDetermined = new("LoopRangeTrackerStage.MinDetermined");
	private readonly Counter MaxDetermined = new("LoopRangeTrackerStage.MaxDetermined");

	private TraceLog trace;

	protected override void Initialize()
	{
		Register(MinDetermined);
		Register(MaxDetermined);
	}

	protected override void Finish()
	{
		trace = null;
	}

	protected override void Run()
	{
		if (HasProtectedRegions)
			return;

		// Method is empty - must be a plugged method
		if (BasicBlocks.HeadBlocks.Count != 1)
			return;

		if (BasicBlocks.PrologueBlock == null)
			return;

		if (!MethodCompiler.IsInSSAForm)
			return;

		trace = CreateTraceLog(5);

		var loops = LoopDetector.FindLoops(BasicBlocks);

		if (loops.Count == 0)
			return;

		ProcessLoops(loops);
	}

	private void ProcessLoops(List<Loop> loops)
	{
		foreach (var loop in loops)
		{
			ProcessLoop(loop);
		}
	}

	private void ProcessLoop(Loop loop)
	{
		if (loop.Header.PreviousBlocks.Count != 2)
			return;

		for (var node = loop.Header.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
		{
			if (node.IsEmptyOrNop)
				continue;

			if (!node.Instruction.IsPhi)
				return;

			if (node.OperandCount != 2)
				continue;

			if (node.Instruction == IR.Phi32)
				ProcessNode(node, loop, true);
			else if (node.Instruction == IR.Phi64)
				ProcessNode(node, loop, false);
		}
	}

	private void ProcessNode(Node node, Loop loop, bool is32Bit)
	{
		var headerblock = node.Block.PreviousBlocks[0];

		var result = node.Result;
		var x = node.Operand1;
		var y = node.Operand2;

		var b1 = node.PhiBlocks[0];
		var b2 = node.PhiBlocks[1];

		if (!x.IsResolvedConstant || b1 != headerblock)
		{
			// swap
			(x, y, b1, b2) = (y, x, b2, b1);
		}

		if (!x.IsResolvedConstant)
			return;

		if (b1 != headerblock)
			return;

		if (!y.IsDefinedOnce)
			return;

		var d = y.Definitions[0];

		// Future: determine direction base on IR.Add or IR.Sub and constant

		if (!(d.Instruction == IR.Add32 || d.Instruction == IR.Add64))
			return;

		if (d.Result != y)
			return;

		if (d.Operand1 != result)
			return;

		if (!d.Operand2.IsResolvedConstant)
			return;

		if (d.Operand2.ConstantUnsigned64 <= 0)
			return;

		if (!loop.LoopBlocks.Contains(d.Block))
			return;

		result.BitValue.NarrowMin(x.ConstantUnsigned64);

		trace?.Log($"{result} MinValue = {x.ConstantUnsigned64}");
		MinDetermined.Increment();

		if (DetermineMaxOut(d.Operand1, d.Operand2, loop, out var max))
		{
			result.BitValue.NarrowMax((ulong)max);

			trace?.Log($"{result} MaxValue = {max}");
			MaxDetermined.Increment();
		}
	}

	private static bool DetermineMaxOut(Operand incrementVariable, Operand incrementValue, Loop loop, out long max)
	{
		bool determined = false;
		max = long.MaxValue;

		foreach (var b in incrementVariable.Uses)
		{
			if (!(b.Instruction == IR.Branch32 || b.Instruction == IR.Branch64))
				continue;

			// only that are the header or backedge (if only one)
			if (!(b.Block == loop.Header || (loop.Backedges.Count == 1 && loop.Backedges.Contains(b.Block))))
				continue;

			var x = b.Operand1;
			var y = b.Operand2;
			var condition = b.ConditionCode;
			var target = b.BranchTarget1;
			var othertarget = target != b.Block.NextBlocks[0]
				? b.Block.NextBlocks[0]
				: b.Block.NextBlocks[1];

			// form: x (variable) </<=/== y (constant) -> where branch exits the loop

			// change form - on condition
			if (condition is not (ConditionCode.Less
				or ConditionCode.LessOrEqual
				or ConditionCode.UnsignedLess
				or ConditionCode.UnsignedLessOrEqual
				or ConditionCode.Equal))
			{
				(x, y, condition) = (y, x, condition.GetOpposite()); // swap
			}

			// change form - on branch
			if (!loop.LoopBlocks.Contains(target))
			{
				(condition, target, othertarget) = (condition.GetOpposite(), othertarget, target); // swap
			}

			// change form - on constant to right
			if (!y.IsResolvedConstant && condition == ConditionCode.Equal)
			{
				(x, y) = (y, x);
			}

			if (condition is not (ConditionCode.Less
				or ConditionCode.LessOrEqual
				or ConditionCode.UnsignedLess
				or ConditionCode.UnsignedLessOrEqual
				or ConditionCode.Equal))
				continue;

			if (x != incrementVariable)
				continue;

			if (!y.IsResolvedConstant)
				continue;

			if (!loop.LoopBlocks.Contains(target))
				continue; // exits loop

			var adj = condition == ConditionCode.LessOrEqual || condition == ConditionCode.Equal ? 1 : 0;

			var branchmax = y.ConstantSigned64 + incrementValue.ConstantSigned64 - 1 + adj;

			max = Math.Min(max, branchmax);

			determined = true;
		}

		return determined;
	}
}
