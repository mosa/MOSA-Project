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
				ProcessNode32(node, loop);

			//if (node.Instruction == IR.Phi64))
			//ProcessNode64(node, loop);
		}
	}

	private void ProcessNode32(Node node, Loop loop)
	{
		var headerblock = node.Block.PreviousBlocks[0];

		//	match a = phi(x, y) { B1, B2}
		//	in header loop
		//	where x is constant
		//	B1 is loop.header.previous

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

		//y.defined by
		//instruction = Add
		//op1 = x
		//op2 = positive constant c (increment)
		//in block within loop        // may not be necessary in SSA form
		// => at this point, we know the lower of a is constant x

		if (!y.IsDefinedOnce)
			return;

		var d = y.Definitions[0];

		if (d.Instruction != IR.Add32)
			return;

		if (d.Result != y)
			return;

		if (d.Operand1 != result)
			return;

		if (!d.Operand2.IsResolvedConstant)
			return;

		if (d.Operand2.ConstantUnsigned32 <= 0)
			return;

		if (!loop.LoopBlocks.Contains(d.Block))
			return;

		result.BitValue.NarrowMin(x.ConstantUnsigned32);

		trace?.Log($"{result} MinValue = {x.ConstantUnsigned32}");
		MinDetermined.Increment();

		if (DetermineMaxOut32(d.Operand1, d.Operand2, loop, out var max))
		{
			result.BitValue.NarrowMax((ulong)max);

			trace?.Log($"{result} MaxValue = {max}");
			MaxDetermined.Increment();
		}
	}

	private bool DetermineMaxOut32(Operand incrementVariable, Operand incrementValue, Loop loop, out int max)
	{
		bool determined = false;
		max = int.MaxValue;

		//a.used by
		//instruction = branch
		//compare: <
		//op1 = result
		//op2 = constant d
		//branch is to phi node
		//in block within loop

		foreach (var b in incrementVariable.Uses)
		{
			if (b.Instruction != IR.Branch32)
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

			if (!(condition == ConditionCode.Less
				|| condition == ConditionCode.LessOrEqual
				|| condition == ConditionCode.UnsignedLess
				|| condition == ConditionCode.UnsignedLessOrEqual
				|| condition == ConditionCode.Equal))
			{
				// swap
				(x, y, condition, target, othertarget) = (y, x, condition.GetOpposite(), othertarget, target);
			}

			// form: x (variable) </<=/== y (constant) -> which branch exits the loop
			if (!(condition == ConditionCode.Less
				|| condition == ConditionCode.LessOrEqual
				|| condition == ConditionCode.UnsignedLess
				|| condition == ConditionCode.UnsignedLessOrEqual
				|| condition == ConditionCode.Equal))
				continue;

			if (x != incrementVariable)
				continue;

			if (!y.IsResolvedConstant)
				continue;

			if (!loop.LoopBlocks.Contains(target))  // exits loop
				continue;

			var adj = condition == ConditionCode.LessOrEqual || condition == ConditionCode.Equal ? 1 : 0;

			var branchmax = y.ConstantSigned32 + incrementValue.ConstantSigned32 - 1 + adj;

			max = Math.Min(max, branchmax);

			determined = true;
		}

		return determined;
	}
}
