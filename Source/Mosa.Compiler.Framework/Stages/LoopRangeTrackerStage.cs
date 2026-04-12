// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework.Analysis;
using Mosa.Utility.Configuration;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// Loop Range Tracker Stage
/// </summary>
public sealed class LoopRangeTrackerStage : BaseMethodCompilerStage
{
	private readonly Counter RangeDetermined = new("LoopRangeTrackerStage.RangeDetermined");

	private TraceLog trace;

	protected override void Initialize()
	{
		Register(RangeDetermined);
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

			if (node.Instruction == IR.Phi32 || node.Instruction == IR.Phi64)
				ProcessNode(node, loop);
		}
	}

	private static BasicBlock GetPreHeaderBlock(Loop loop)
	{
		foreach (var block in loop.Header.PreviousBlocks)
		{
			if (!loop.LoopBlocks.Contains(block))
				return block;
		}

		return null;
	}

	private void ProcessNode(Node node, Loop loop)
	{
		var headerblock = GetPreHeaderBlock(loop);
		if (headerblock == null)
			return;

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

		if (!(d.Instruction == IR.Add32
			|| d.Instruction == IR.Add64
			|| d.Instruction == IR.Sub32
			|| d.Instruction == IR.Sub64))
			return;

		var direction = d.Instruction == IR.Add32 || d.Instruction == IR.Add64;

		var incrementValueOperand = d.Operand2;
		var incrementVariableOperand = d.Operand1;

		if ((d.Instruction == IR.Add32 || d.Instruction == IR.Add64)
			&& incrementVariableOperand.IsResolvedConstant
			&& !incrementValueOperand.IsResolvedConstant)
		{
			(incrementVariableOperand, incrementValueOperand) = (incrementValueOperand, incrementVariableOperand);
		}

		if (d.Result != y)
			return;

		if (incrementVariableOperand != result)
			return;

		if (!incrementValueOperand.IsResolvedConstant)
			return;

		if (!loop.LoopBlocks.Contains(d.Block))
			return;

		if (incrementValueOperand.ConstantSigned64 == 0)
			return;

		var signedAnalysis = DetermineSignUsage(incrementVariableOperand);

		if (signedAnalysis == SignAnalysis.Unknown || signedAnalysis == SignAnalysis.Both)
			return;

		var signed = signedAnalysis == SignAnalysis.Signed;

		if (incrementValueOperand.IsNegative)
		{
			direction = !direction;
		}

		var startOperand = x;

		if (direction)
		{
			if (DetermineMaxOut(incrementVariableOperand, incrementValueOperand, loop, out var end))
			{
				if (!signed)
				{
					if (end < 0)
						return;

					var start = startOperand.ConstantUnsigned64;

					result.BitValue.NarrowMin(start).NarrowMax((ulong)end);

					trace?.Log($"{result}");
					trace?.Log($"** Start = {start}");
					trace?.Log($"** End   = {end}");
					trace?.Log($"{result.BitValue}");

					SpecialTrace?.Log($"Method: {Method}");
					SpecialTrace?.Log($"  {result} range = {start} to {end}");

					RangeDetermined.Increment();
				}
				else
				{
					var start = startOperand.ConstantSigned64;

					result.BitValue.NarrowSignRange(start, end);

					trace?.Log($"{result}");
					trace?.Log($"** Start = {start}");
					trace?.Log($"** End   = {end}");
					trace?.Log($"{result.BitValue}");

					SpecialTrace?.Log($"Method: {Method}");
					SpecialTrace?.Log($"  {result} range = {start} to {end}");

					RangeDetermined.Increment();
				}
			}
		}
		else
		{
			if (DetermineMinOut(incrementVariableOperand, incrementValueOperand, loop, out var end))
			{
				if (!signed)
				{
					if (end < 0)
						return;

					var start = startOperand.ConstantUnsigned64;

					result.BitValue.NarrowMax(start).NarrowMin((ulong)end);

					trace?.Log($"{result}");
					trace?.Log($"** Start = {start}");
					trace?.Log($"** End   = {end}");
					trace?.Log($"{result.BitValue}");

					SpecialTrace?.Log($"Method: {Method}");
					SpecialTrace?.Log($"  {result} range = {start} to {end}");

					RangeDetermined.Increment();
				}
				else
				{
					var start = startOperand.ConstantSigned64;

					result.BitValue.NarrowSignRange(start, end);

					trace?.Log($"{result}");
					trace?.Log($"** Start = {start}");
					trace?.Log($"** End   = {end}");
					trace?.Log($"{result.BitValue}");

					SpecialTrace?.Log($"Method: {Method}");
					SpecialTrace?.Log($"  {result} range = {start} to {end}");

					RangeDetermined.Increment();
				}
			}
		}
	}

	private enum SignAnalysis
	{ Unknown, Signed, NotSigned, Both };

	private static SignAnalysis DetermineSignUsage(Operand value)
	{
		var signAnalysis = SignAnalysis.Unknown;

		foreach (var use in value.Uses)
		{
			if (!(use.Instruction == IR.Branch32 || use.Instruction == IR.Branch64))
				continue;

			var condition = use.ConditionCode;
			var signed = condition.IsSigned();

			if (signAnalysis == SignAnalysis.Unknown)
			{
				signAnalysis = signed ? SignAnalysis.Signed : SignAnalysis.NotSigned;
			}
			else
			{
				if (signed && signAnalysis == SignAnalysis.Signed)
					continue;

				if (!signed && signAnalysis == SignAnalysis.NotSigned)
					continue;

				return SignAnalysis.Both;
			}
		}

		return signAnalysis;
	}

	private static bool DetermineMaxOut(Operand incrementVariable, Operand incrementValue, Loop loop, out long max)
	{
		var determined = false;
		max = long.MaxValue;

		// limit to increament values that can not cause overflow
		if (incrementValue.IsInt32 && (incrementValue.ConstantSigned32 >= short.MaxValue - 1 || incrementValue.ConstantSigned32 <= short.MinValue + 1))
			return false;

		if (!incrementValue.IsInt32 && (incrementValue.ConstantSigned64 >= short.MaxValue - 1 || incrementValue.ConstantSigned64 <= short.MinValue + 1))
			return false;

		foreach (var use in incrementVariable.Uses)
		{
			if (!(use.Instruction == IR.Branch32 || use.Instruction == IR.Branch64))
				continue;

			// only analysis the header or backedge (if only one)
			if (!(use.Block == loop.Header || (loop.Backedges.Count == 1 && loop.Backedges.Contains(use.Block))))
				continue;

			var x = use.Operand1;
			var y = use.Operand2;
			var condition = use.ConditionCode;
			var target = use.BranchTarget1;
			var othertarget = target != use.Block.NextBlocks[0]
				? use.Block.NextBlocks[0]
				: use.Block.NextBlocks[1];

			// form: x (variable) {>,>=} y (constant) -> where branch exits loop

			// change form - on branch
			if (loop.LoopBlocks.Contains(target))
			{
				(condition, target, othertarget) = (condition.GetOpposite(), othertarget, target);
			}

			if (loop.LoopBlocks.Contains(target) || !loop.LoopBlocks.Contains(othertarget))
				continue;

			// change form - on condition
			if (!y.IsResolvedConstant)
			{
				(x, y, condition) = (y, x, condition.GetOpposite()); // swap
			}

			if (condition is not (
				   ConditionCode.Greater
				or ConditionCode.GreaterOrEqual
				or ConditionCode.UnsignedGreater
				or ConditionCode.UnsignedGreaterOrEqual))
				continue;

			if (x != incrementVariable)
				continue;

			if (!y.IsResolvedConstant)
				continue;

			Debug.Assert(y.IsInt32 == incrementValue.IsInt32);

			if (y.IsInt32 && (y.ConstantSigned32 >= short.MaxValue - 1 || y.ConstantSigned32 <= short.MinValue + 1))
				continue;

			if (!y.IsInt32 && (y.ConstantSigned64 >= short.MaxValue - 1 || y.ConstantSigned64 <= short.MinValue + 1))
				continue;

			var adj = condition == ConditionCode.GreaterOrEqual
				|| condition == ConditionCode.UnsignedGreaterOrEqual ? 0 : 1;

			var value = y.IsInt32
				? y.ConstantSigned32 + incrementValue.ConstantSigned32 + adj
				: y.ConstantSigned64 + incrementValue.ConstantSigned64 + adj;

			max = Math.Min(max, value);

			determined = true;
		}

		return determined;
	}

	private static bool DetermineMinOut(Operand incrementVariable, Operand incrementValue, Loop loop, out long min)
	{
		var determined = false;
		min = long.MinValue;

		// limit to increament values that can not cause overflow
		if (incrementValue.IsInt32 && (incrementValue.ConstantSigned32 >= short.MaxValue - 1 || incrementValue.ConstantSigned32 <= short.MinValue + 1))
			return false;

		if (!incrementValue.IsInt32 && (incrementValue.ConstantSigned64 >= short.MaxValue - 1 || incrementValue.ConstantSigned64 <= short.MinValue + 1))
			return false;

		foreach (var use in incrementVariable.Uses)
		{
			if (!(use.Instruction == IR.Branch32 || use.Instruction == IR.Branch64))
				continue;

			// only analysis the header or backedge (if only one)
			if (!(use.Block == loop.Header || (loop.Backedges.Count == 1 && loop.Backedges.Contains(use.Block))))
				continue;

			var x = use.Operand1;
			var y = use.Operand2;
			var condition = use.ConditionCode;
			var target = use.BranchTarget1;
			var othertarget = target != use.Block.NextBlocks[0]
				? use.Block.NextBlocks[0]
				: use.Block.NextBlocks[1];

			// form: x (variable) {<,<=} y (constant) -> where branch exits the loop

			// change form - on branch
			if (loop.LoopBlocks.Contains(target))
			{
				(condition, target, othertarget) = (condition.GetOpposite(), othertarget, target); // swap
			}

			if (loop.LoopBlocks.Contains(target) || !loop.LoopBlocks.Contains(othertarget))
				continue;

			// change form - on condition
			if (!y.IsResolvedConstant)
			{
				(x, y, condition) = (y, x, condition.GetOpposite()); // swap
			}

			if (condition is not (
				   ConditionCode.Less
				or ConditionCode.LessOrEqual
				or ConditionCode.UnsignedLess
				or ConditionCode.UnsignedLessOrEqual))
				continue;

			if (x != incrementVariable)
				continue;

			if (!y.IsResolvedConstant)
				continue;

			Debug.Assert(y.IsInt32 == incrementValue.IsInt32);

			if (y.IsInt32 && (y.ConstantSigned32 >= short.MaxValue - 1 || y.ConstantSigned32 <= short.MinValue + 1))
				continue;

			if (!y.IsInt32 && (y.ConstantSigned64 >= short.MaxValue - 1 || y.ConstantSigned64 <= short.MinValue + 1))
				continue;

			var adj = condition == ConditionCode.LessOrEqual
				|| condition == ConditionCode.UnsignedLessOrEqual ? 1 : 0;

			var value = y.IsInt32
				? y.ConstantSigned32 + incrementValue.ConstantSigned32 + adj
				: y.ConstantSigned64 + incrementValue.ConstantSigned64 + adj;

			min = Math.Max(min, value);

			determined = true;
		}

		return determined;
	}
}
