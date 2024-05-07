// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Common;

namespace Mosa.Compiler.Framework.Managers;

/// <summary>
/// Exception Handler Operand Manager
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseTransformManager" />
public class ExceptionHandlerOperandManager : BaseTransformManager
{
	private readonly HashSet<Operand> HandlerOperands = new();
	private readonly HashSet<Operand> EvaluatedOperands = new();

	private BlockBitSet HandlerBlocks;
	private BlockBitSet UnknownHandlerBlocks;

	private BasicBlocks BasicBlocks;

	private bool HasProtectedRegions;

	public override void Initialize(Compiler compiler)
	{
	}

	public override void Setup(MethodCompiler methodCompiler)
	{
		Reset();

		HasProtectedRegions = methodCompiler.HasProtectedRegions;
		BasicBlocks = methodCompiler.BasicBlocks;

		HandlerBlocks = new BlockBitSet(BasicBlocks);
		UnknownHandlerBlocks = new BlockBitSet(BasicBlocks);

		MarkExceptionHandlerBlocks(methodCompiler);
	}

	public override void Reset()
	{
		EvaluatedOperands.Clear();
		HandlerOperands.Clear();

		HandlerBlocks = null;
		UnknownHandlerBlocks = null;
	}

	public bool IsWithinHandler(Operand operand)
	{
		if (!HasProtectedRegions)
			return false;

		if (!operand.IsUsed)
			return false;

		// Check Cache

		if (HandlerOperands.Contains(operand))
			return true;

		if (EvaluatedOperands.Contains(operand))
			return false;

		// Go check

		foreach (var use in operand.Uses)
		{
			var block = use.Block;

			if (CheckBlockInHandler(block))
			{
				HandlerOperands.Add(operand);
				EvaluatedOperands.Add(operand);
				return true;
			}
			else
			{
				EvaluatedOperands.Add(operand);
			}
		}


		foreach (var def in operand.Definitions)
		{
			var block = def.Block;

			if (CheckBlockInHandler(block))
			{
				HandlerOperands.Add(operand);
				EvaluatedOperands.Add(operand);
				return true;
			}
			else
			{
				EvaluatedOperands.Add(operand);
			}
		}

		return false;
	}

	private bool CheckBlockInHandler(BasicBlock block)
	{
		if (HandlerBlocks.Contains(block))
			return true;

		if (!UnknownHandlerBlocks.Contains(block))
			return false;

		var visited = new BlockBitSet(BasicBlocks);
		var worklist = new Stack<BasicBlock>();

		worklist.Push(block);

		var handlerBlock = false;

		while (worklist.Count > 0)
		{
			var at = worklist.Pop();

			if (visited.Contains(at))
				continue;

			visited.Add(at);

			if (HandlerBlocks.Contains(at))
			{
				handlerBlock = true;
				break;
			}
			foreach (var prev in at.PreviousBlocks)
			{
				if (!visited.Contains(prev))
				{
					worklist.Push(prev);
				}
			}
		}

		if (handlerBlock)
		{
			foreach (var at in visited.GetBlocks(BasicBlocks))
			{
				HandlerBlocks.Add(at);
				UnknownHandlerBlocks.Remove(at);
			}
		}

		return handlerBlock;
	}

	private void MarkExceptionHandlerBlocks(MethodCompiler methodCompiler)
	{
		foreach (var block in BasicBlocks)
		{
			if (block.IsHandlerHeadBlock || block.IsTryHeadBlock)
			{
				HandlerBlocks.Add(block);
				continue;
			}

			foreach (var handler in methodCompiler.Method.ExceptionHandlers)
			{
				if (handler.IsLabelWithinHandler(block.Label))
				{
					HandlerBlocks.Add(block);
					continue;
				}
			}

			if (!block.IsCompilerBlock)
				continue;

			UnknownHandlerBlocks.Add(block);
		}
	}
}
