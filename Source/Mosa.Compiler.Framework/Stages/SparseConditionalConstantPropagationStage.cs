// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.Trace;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// Sparse Conditional Constant Propagation Stage
/// </summary>
public class SparseConditionalConstantPropagationStage : BaseMethodCompilerStage
{
	protected TraceLog trace;

	private readonly Counter ConstantCount = new Counter("SparseConditionalConstantPropagationStage.ConstantVariables");
	private readonly Counter ConditionalConstantPropagationCount = new Counter("SparseConditionalConstantPropagationStage.ConstantPropagations");
	private readonly Counter DeadBlockCount = new Counter("SparseConditionalConstantPropagationStage.DeadBlocks");
	private readonly Counter InstructionsRemovedCount = new Counter("SparseConditionalConstantPropagationStage.IRInstructionRemoved");

	protected bool changed;

	protected override void Initialize()
	{
		Register(ConstantCount);
		Register(ConditionalConstantPropagationCount);
		Register(DeadBlockCount);
		Register(InstructionsRemovedCount);
	}

	protected override void Setup()
	{
		trace = CreateTraceLog();

		changed = false;
	}

	protected override void Run()
	{
		if (!HasCode)
			return;

		if (!MethodCompiler.IsInSSAForm)
			return;

		if (HasProtectedRegions)
			return;

		var analysis = new SparseConditionalConstantPropagation(BasicBlocks, CreateTraceLog, Is32BitPlatform);

		var deadBlocks = analysis.GetDeadBlocked();
		var constants = analysis.GetIntegerConstants();

		RemoveDeadBlocks(deadBlocks);
		ReplaceVirtualRegistersWithConstants(constants);

		ConstantCount.Set(constants.Count);
		DeadBlockCount.Set(deadBlocks.Count);

		if (CompilerSettings.FullCheckMode)
			CheckAllPhiInstructions();
	}

	protected override void Finish()
	{
		trace = null;
	}

	protected void ReplaceVirtualRegistersWithConstants(List<Tuple<Operand, ulong>> constantVirtualRegisters)
	{
		foreach (var value in constantVirtualRegisters)
		{
			ReplaceVirtualRegisterWithConstant(value.Item1, value.Item2);
		}
	}

	protected void ReplaceVirtualRegisterWithConstant(Operand target, ulong value)
	{
		trace?.Log($"{target} = {value} Uses: {target.Uses.Count}");

		if (target.Definitions.Count == 0)
			return;

		if (target.Definitions.Count != 1)
			throw new CompilerException($"SCCP: {target} definition has move than one value");

		//Debug.Assert(target.Definitions.Count == 1);

		if (target.Uses.Count != 0)
		{
			Debug.Assert(!target.IsFloatingPoint);

			var constant = target.IsInt32
				? CreateConstant32((uint)value)
				: CreateConstant64(value);

			// for each statement T that uses operand, substituted c in statement T
			foreach (var node in target.Uses.ToArray())
			{
				Debug.Assert(node.Instruction != IRInstruction.AddressOf);

				for (var i = 0; i < node.OperandCount; i++)
				{
					var operand = node.GetOperand(i);

					if (operand != target)
						continue;

					trace?.Log("*** ConditionalConstantPropagation");
					trace?.Log($"BEFORE:\t{node}");
					node.SetOperand(i, constant);
					ConditionalConstantPropagationCount.Increment();
					trace?.Log($"AFTER: \t{node}");

					changed = true;
				}
			}
		}

		Debug.Assert(target.Uses.Count == 0);

		if (target.Definitions.Count == 0)
			return;

		var defNode = target.Definitions[0];

		trace?.Log($"REMOVED:\t{defNode}");
		defNode.SetNop();
		InstructionsRemovedCount.Increment();
	}

	protected void RemoveDeadBlocks(List<BasicBlock> deadBlocks)
	{
		foreach (var block in deadBlocks)
		{
			RemoveBranchesToDeadBlocks(block);
		}
	}

	protected void RemoveBranchesToDeadBlocks(BasicBlock deadBlock)
	{
		foreach (var previous in deadBlock.PreviousBlocks.ToArray())
		{
			// unable to handle more than two branch blocks
			// and if only one branch, then this block is dead as well
			if (previous.NextBlocks.Count != 2)
				return;

			var otherBlock = previous.NextBlocks[0] == deadBlock ? previous.NextBlocks[1] : previous.NextBlocks[0];

			for (var node = previous.Last.Previous; !node.IsBlockStartInstruction; node = node.Previous)
			{
				if (node.IsEmptyOrNop)
					continue;

				if (node.BranchTargetsCount == 0)
					continue;

				if (node.Instruction == IRInstruction.Branch32 || node.Instruction == IRInstruction.Branch64 || node.Instruction == IRInstruction.BranchObject)
				{
					trace?.Log("*** RemoveBranchesToDeadBlocks");
					trace?.Log($"REMOVED:\t{node}");
					node.SetNop();
					InstructionsRemovedCount.Increment();
					continue;
				}

				if (node.Instruction == IRInstruction.Jmp)
				{
					trace?.Log("*** RemoveBranchesToDeadBlocks");
					trace?.Log($"BEFORE:\t{node}");
					node.UpdateBranchTarget(0, otherBlock);
					trace?.Log($"AFTER: \t{node}");
					continue;
				}

				break;
			}
		}

		CheckAndClearEmptyBlock(deadBlock);
	}

	private void CheckAndClearEmptyBlock(BasicBlock block)
	{
		if (block.PreviousBlocks.Count != 0 || block.IsHeadBlock)
			return;

		trace?.Log($"*** Removed Block: {block}");

		var nextBlocks = block.NextBlocks.ToArray();

		EmptyBlockOfAllInstructions(block, true);

		//UpdatePhiBlocks(nextBlocks);

		foreach (var next in nextBlocks)
		{
			CheckAndClearEmptyBlock(next);
		}
	}
}
