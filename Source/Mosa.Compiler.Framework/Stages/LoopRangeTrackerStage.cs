﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.Instructions;

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

		if (!(d.Instruction == IR.Add32
			|| d.Instruction == IR.Add64
			|| d.Instruction == IR.Sub32
			|| d.Instruction == IR.Sub64))
			return;

		var direction = d.Instruction == IR.Add32 || d.Instruction == IR.Add64;

		var incrementValueOperand = d.Operand2;
		var incrementVariableOperand = d.Operand1;

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

		if (signedAnalysis == SignedAnalysis.Unknown || signedAnalysis == SignedAnalysis.Both)
			return;

		bool signed = signedAnalysis == SignedAnalysis.Signed;

		if ((signed && incrementValueOperand.IsNegative)
			|| (!signed && incrementValueOperand.IsNegative))
		{
			direction = !direction;
		}

		var startOperand = x;

		if (direction)
		{
			if (!signed /*|| (signed && !startOperand.IsNegative)*/)
			{
				var start = startOperand.ConstantUnsigned64;

				result.BitValue.NarrowMin(start);

				trace?.Log($"{result} MinValue = {start}");
				MinDetermined.Increment();
			}

			if (DetermineMaxOut(incrementVariableOperand, incrementValueOperand, loop, out var max))
			{
				if (max >= 0 || !signed)
				{
					result.BitValue.NarrowMax((ulong)max);

					trace?.Log($"{result} MaxValue = {max}");
					MaxDetermined.Increment();
				}
			}
		}
		else
		{
			if (!signed /*|| (signed && !startOperand.IsNegative)*/)
			{
				var start = startOperand.ConstantUnsigned64;

				result.BitValue.NarrowMax(start);

				trace?.Log($"{result} MaxValue = {start}");
				MaxDetermined.Increment();
			}

			if (DetermineMinOut(incrementVariableOperand, incrementValueOperand, loop, out var min))
			{
				if (min >= 0 || !signed)
				{
					result.BitValue.NarrowMin((ulong)min);

					trace?.Log($"{result} MinValue = {min}");
					MinDetermined.Increment();
				}
			}
		}
	}

	private enum SignedAnalysis
	{ Unknown, Signed, NotSigned, Both };

	private static SignedAnalysis DetermineSignUsage(Operand value)
	{
		var signAnalysis = SignedAnalysis.Unknown;

		foreach (var b in value.Uses)
		{
			if (!(b.Instruction == IR.Branch32 || b.Instruction == IR.Branch64))
				continue;

			var condition = b.ConditionCode;
			var signed = condition.IsSigned();

			if (signAnalysis == SignedAnalysis.Unknown)
			{
				signAnalysis = signed ? SignedAnalysis.Signed : SignedAnalysis.NotSigned;
			}
			else
			{
				if (signed && signAnalysis == SignedAnalysis.Signed)
					continue;

				if (!signed && signAnalysis != SignedAnalysis.NotSigned)
					continue;

				return SignedAnalysis.Both;
			}
		}

		return signAnalysis;
	}

	private static bool DetermineMaxOut(Operand incrementVariable, Operand incrementValue, Loop loop, out long max)
	{
		bool determined = false;
		max = long.MaxValue;

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

			// form: x (variable) {>,>=,==} y (constant) -> where branch exits loop

			// change form - on branch
			if (loop.LoopBlocks.Contains(target))
			{
				(condition, target, othertarget) = (condition.GetOpposite(), othertarget, target);
			}

			// change form - on constant to right
			if (!y.IsResolvedConstant && (condition == ConditionCode.Equal || condition == ConditionCode.NotEqual))
			{
				(x, y) = (y, x);
			}

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

			if (condition == ConditionCode.NotEqual && !incrementValue.IsConstantOne)
				continue;

			if (condition == ConditionCode.Equal)
				continue;

			if (x != incrementVariable)
				continue;

			if (!y.IsResolvedConstant)
				continue;

			if (loop.LoopBlocks.Contains(target))
				continue; // exits loop

			var adj = condition == ConditionCode.GreaterOrEqual
				|| condition == ConditionCode.UnsignedGreaterOrEqual ? 0 : 1;

			var value = incrementValue.IsInt32
				? y.ConstantSigned32 + incrementValue.ConstantSigned32 + adj
				: y.ConstantSigned64 + incrementValue.ConstantSigned64 + adj;

			max = Math.Min(max, value);

			determined = true;
		}

		return determined;
	}

	private static bool DetermineMinOut(Operand incrementVariable, Operand incrementValue, Loop loop, out long min)
	{
		bool determined = false;
		min = long.MinValue;

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

			// form: x (variable) {<,<=,==} y (constant) -> where branch exits the loop

			// change form - on branch
			if (loop.LoopBlocks.Contains(target))
			{
				(condition, target, othertarget) = (condition.GetOpposite(), othertarget, target); // swap
			}

			// change form - on constant to right
			if (!y.IsResolvedConstant && (condition == ConditionCode.Equal || condition == ConditionCode.NotEqual))
			{
				(x, y) = (y, x);
			}

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

			if (condition == ConditionCode.NotEqual && !incrementValue.IsConstantOne)
				continue;

			if (condition == ConditionCode.Equal)
				continue;

			if (x != incrementVariable)
				continue;

			if (!y.IsResolvedConstant)
				continue;

			if (loop.LoopBlocks.Contains(target))
				continue; // exits loop

			var adj = condition == ConditionCode.LessOrEqual
				|| condition == ConditionCode.UnsignedLessOrEqual ? 1 : 0;

			var value = incrementValue.IsInt32
				? y.ConstantSigned32 + incrementValue.ConstantSigned32 + adj
				: y.ConstantSigned64 + incrementValue.ConstantSigned64 + adj;

			min = Math.Max(min, value);

			determined = true;
		}

		return determined;
	}
}
