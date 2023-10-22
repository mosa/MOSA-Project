// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Common.Exceptions;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// Leave SSA Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseMethodCompilerStage" />
public class ExitSSAStage : BaseMethodCompilerStage
{
	private readonly Counter InstructionCount = new("ExitSSAStage.IRInstructions");
	private readonly Counter MoveAvoidedCount = new("ExitSSAStage.MoveAvoided");

	protected override void Initialize()
	{
		Register(InstructionCount);
		Register(MoveAvoidedCount);
	}

	protected override void Run()
	{
		if (!HasCode)
			return;

		//if (HasProtectedRegions)
		//	return;

		foreach (var block in BasicBlocks)
		{
			for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmptyOrNop)
					continue;

				InstructionCount.Increment();

				if (!node.Instruction.IsPhi)
					break;

				if (node.OperandCount != node.Block.PreviousBlocks.Count)
					throw new CompilerException($"ExitSSAStage: Invalid block counts: {node}");

				ProcessPhiInstruction(node);
			}
		}

		MethodCompiler.IsInSSAForm = false;
	}

	/// <summary>
	/// Processes the phi instruction.
	/// </summary>
	/// <param name="node">The context.</param>
	private void ProcessPhiInstruction(Node node)
	{
		var sourceBlocks = node.PhiBlocks;

		for (var index = 0; index < sourceBlocks.Count; index++)
		{
			var operand = node.GetOperand(index);
			var predecessor = sourceBlocks[index];

			InsertCopyStatement(predecessor, node.Result, operand);
		}

		node.Empty();
	}

	/// <summary>
	/// Inserts the copy statement.
	/// </summary>
	/// <param name="predecessor">The predecessor.</param>
	/// <param name="destination">The result.</param>
	/// <param name="source">The operand.</param>
	private void InsertCopyStatement(BasicBlock predecessor, Operand destination, Operand source)
	{
		if (destination == source)
			return;

		if (source.IsDefinedOnce && source.IsUsedOnce && source.Definitions[0].Block == predecessor)
		{
			if (destination.IsUsedOnce || CheckIfLast(predecessor, destination, source))
			{
				source.Definitions[0].Result = destination;
				MoveAvoidedCount.Increment();
				return;
			}
		}

		var node = predecessor.BeforeLast;

		while (node.Instruction != IRInstruction.Jmp)
		{
			node = node.Previous;
		}

		node = node.Previous;

		Debug.Assert(node.Instruction != IRInstruction.Branch32);
		Debug.Assert(node.Instruction != IRInstruction.Branch64);
		Debug.Assert(node.Instruction != IRInstruction.BranchObject);

		var context = new Context(node);

		var moveInstruction = MethodCompiler.GetMoveInstruction(destination.Primitive);
		context.AppendInstruction(moveInstruction, destination, source);
	}

	private bool CheckIfLast(BasicBlock block, Operand stop, Operand okay)
	{
		for (var node = block.BeforeLast; !node.IsBlockStartInstruction; node = node.Previous)
		{
			if (node.IsEmptyOrNop)
				continue;

			if (node.ContainsOperand(stop))
				return false;

			if (node.ContainsOperand(okay))
				return true;
		}

		return false;
	}
}
