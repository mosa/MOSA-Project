// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using System.Text;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Analysis;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// Loop Invariant Code Motion Stage
/// </summary>
public sealed class LoopInvariantCodeMotionStage : BaseMethodCompilerStage
{
	private readonly Counter LandingPadsCount = new("LoopInvariantCodeMotion.LandingPads");
	private readonly Counter CodeMotionCount = new("LoopInvariantCodeMotion.CodeMotion");
	private readonly Counter Methods = new("LoopInvariantCodeMotion.Methods");

	private HashSet<Operand> ParamStoreSet = new();

	private TraceLog trace;

	protected override void Initialize()
	{
		Register(CodeMotionCount);
		Register(LandingPadsCount);
		Register(Methods);
	}

	protected override void Run()
	{
		if (!MethodCompiler.IsInSSAForm)
			return;

		if (HasProtectedRegions)
			return;

		// Method is empty - must be a plugged method
		if (BasicBlocks.HeadBlocks.Count != 1)
			return;

		if (BasicBlocks.PrologueBlock == null)
			return;

		trace = CreateTraceLog(5);

		ParamStoreSet = CollectParamStores();

		var loops = LoopDetector.FindLoops(BasicBlocks);

		if (loops.Count == 0)
			return;

		if (trace != null)
		{
			var loopTrace = CreateTraceLog("Loops");

			LoopDetector.DumpLoops(loopTrace, loops);
		}

		SortLoopsByBlockCount(loops);

		FindLoopInvariantInstructions(loops);

		if (CodeMotionCount.Count != 0)
		{
			Methods.Increment();
		}

		if (MosaSettings.FullCheckMode)
			CheckAllPhiInstructions();
	}

	protected override void Finish()
	{
		ParamStoreSet = null;
	}

	private static void SortLoopsByBlockCount(List<Loop> loops)
	{
		loops.Sort((Loop p1, Loop p2) => p1.LoopBlocks.Count < p2.LoopBlocks.Count ? 0 : 1);
	}

	private void FindLoopInvariantInstructions(List<Loop> loops)
	{
		foreach (var loop in loops)
		{
			var invariants = FindLoopInvariantInstructions(loop);

			MoveToPreHeader(invariants, loop);
		}
	}

	private List<Node> FindLoopInvariantInstructions(Loop loop)
	{
		var invariantsSet = new HashSet<Node>();
		var invariantsList = new List<Node>();

		var changed = true;

		trace?.Log($"Loop: {loop.Header}");

		while (changed)
		{
			changed = false;
			foreach (var block in loop.LoopBlocks)
			{
				for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmptyOrNop)
						continue;

					// note - similar code in ValueNumberingStage::CanAssignValueNumberToExpression()
					if (node.ResultCount != 1
						|| node.OperandCount is 0 or > 2
						|| node.Instruction.IsMemoryWrite
						|| node.Instruction.IsIOOperation
						|| node.Instruction.HasUnspecifiedSideEffect
						|| node.Instruction.HasVariableOperands
						|| !node.Instruction.IsFlowNext
						|| node.Instruction.IgnoreDuringCodeGeneration
						|| node.Operand1.IsUnresolvedConstant
						|| (node.OperandCount == 2 && node.Operand2.IsUnresolvedConstant)
						|| (node.Instruction.IsMemoryRead && !(node.Instruction.IsParameterLoad && !ParamStoreSet.Contains(node.Operand1))))
						continue;

					if (invariantsSet.Contains(node))
						continue;

					// Check operand 1
					if (!IsInvariant(node.Operand1, loop, invariantsSet))
						continue;

					// check operand 2
					if (node.OperandCount == 2 && !IsInvariant(node.Operand2, loop, invariantsSet))
						continue;

					invariantsSet.Add(node);
					invariantsList.Add(node);

					trace?.Log($"  {node}");

					changed = true;
				}
			}
		}

		return invariantsList;
	}

	private static bool IsInvariant(Operand operand, Loop loop, HashSet<Node> invariants)
	{
		// constant
		if (operand.IsResolvedConstant)
			return true;

		if (operand.IsVirtualRegister && operand.IsDefinedOnce)
		{
			var def = operand.Definitions[0];

			// defined is invariant instruction
			if (/*invariants != null &&*/ invariants.Contains(def))
				return true;

			// defined outside of the loop
			if (!loop.LoopBlocks.Contains(def.Block))
				return true;
		}

		return false;
	}

	private BasicBlock CreatePreHeader(Loop loop)
	{
		var header = loop.Header;

		// Create pre-header block
		var landingpadBlock = BasicBlocks.CreateBlock();
		var landingpad = new Context(landingpadBlock);

		var previousBlocks = new List<BasicBlock>(header.PreviousBlocks);

		foreach (var previous in previousBlocks)
		{
			if (!loop.Backedges.Contains(previous))
			{
				BasicBlocks.ReplaceBranchTargets(previous, header, landingpadBlock);
			}
		}

		// Update and create PHIs in loop header and landing pads
		for (var node = header.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
		{
			if (node.IsEmptyOrNop)
				continue;

			if (!node.Instruction.IsPhi)
				break;

			var landingpadSourceBlocks = new List<BasicBlock>(node.OperandCount);
			var landingpadSourceOperands = new List<Operand>(node.OperandCount);
			var headerSourceBlocks = new List<BasicBlock>(node.OperandCount);
			var headerSourceOperands = new List<Operand>(node.OperandCount);

			var transitionOperand = MethodCompiler.VirtualRegisters.Allocate(node.Result);

			headerSourceBlocks.Add(landingpadBlock);
			headerSourceOperands.Add(transitionOperand);

			for (var i = 0; i < node.PhiBlocks.Count; i++)
			{
				var sourceOperand = node.GetOperand(i);
				var sourceBlock = node.PhiBlocks[i];

				if (loop.Backedges.Contains(sourceBlock))
				{
					Debug.Assert(loop.LoopBlocks.Contains(sourceBlock));

					headerSourceBlocks.Add(sourceBlock);
					headerSourceOperands.Add(sourceOperand);
				}
				else
				{
					Debug.Assert(!loop.LoopBlocks.Contains(sourceBlock));

					landingpadSourceBlocks.Add(sourceBlock);
					landingpadSourceOperands.Add(sourceOperand);
				}
			}

			landingpad.AppendInstruction(node.Instruction, transitionOperand, landingpadSourceOperands);
			landingpad.PhiBlocks = landingpadSourceBlocks;

			node.SetInstruction(node.Instruction, node.Result, headerSourceOperands);
			node.PhiBlocks = headerSourceBlocks;
		}

		landingpad.AppendInstruction(IR.Jmp, header);

		LandingPadsCount.Increment();

		return landingpadBlock;
	}

	private void MoveToPreHeader(List<Node> nodes, Loop loop)
	{
		if (nodes.Count == 0)
			return;

		if (loop.Header.PreviousBlocks.Count == 0)
			return; // special case - a pre-header can not be made because the loop header is already the first block

		var preheader = CreatePreHeader(loop);

		var at = preheader.BeforeLast;

		while (at.IsEmpty || at.Instruction != IR.Jmp)
		{
			at = at.Previous;
		}

		at = at.Previous;

		foreach (var node in nodes)
		{
			at.MoveFrom(node);
			at = node;
		}

		CodeMotionCount.Increment(nodes.Count);
	}

	private HashSet<Operand> CollectParamStores()
	{
		var paramSet = new HashSet<Operand>();

		foreach (var block in BasicBlocks)
		{
			for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmptyOrNop)
					continue;

				if (node.Instruction.IsParameterStore)
				{
					paramSet.AddIfNew(node.Operand1);
				}
			}
		}

		return paramSet;
	}
}
