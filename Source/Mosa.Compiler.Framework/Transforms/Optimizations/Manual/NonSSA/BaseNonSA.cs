// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Managers;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.NonSSA;

public abstract class BaseNonSA : BaseTransform
{
	public BaseNonSA(BaseInstruction instruction, TransformType type, bool log = false)
			: base(instruction, type, log)
	{ }

	protected static Node FindNextUsed(Node start, Operand target, Operand replacement, int window)
	{
		for (var at = start; ; at = at.Next)
		{
			if (window <= 0)
				return null;

			if (at.IsEmptyOrNop)
				continue;

			window--;

			if (at.IsBlockEndInstruction)
				return null;

			if (at.Instruction.IsReturn)
				return null;

			if (at.Instruction == IR.Epilogue)
				return null;

			if (at.Instruction.IsUnconditionalBranch)
			{
				if (at.Block.NextBlocks.Count == 1 && at.BranchTargets[0].PreviousBlocks.Count == 1)
				{
					at = at.BranchTargets[0].First;
					continue;
				}

				return null;
			}

			if (at.ResultCount >= 1 && (at.Result == target || at.Result == replacement))
				return null;

			if (at.ResultCount == 2 && (at.Result2 == target || at.Result2 == replacement))
				return null;

			if (at.OperandCount == 0)
				continue;

			foreach (var operand in at.Operands)
			{
				if (operand == target)
					return at;
			}
		}
	}

	protected static bool CheckDefinitionUnused(Node start, Operand target, int window)
	{
		for (var at = start; ; at = at.Next)
		{
			if (window <= 0)
				return false;

			if (at.IsEmptyOrNop)
				continue;

			window--;

			if (at.IsBlockEndInstruction)
				return false;

			if (at.Instruction.IsReturn)
				return false;

			if (at.Instruction == IR.Epilogue)
				return false;

			if (at.Instruction.IsUnconditionalBranch)
			{
				if (at.Block.NextBlocks.Count == 1 && at.BranchTargets[0].PreviousBlocks.Count == 1)
				{
					at = at.BranchTargets[0].First;
					continue;
				}

				return false;
			}

			if (at.ResultCount >= 1 && (at.Result == target || at.Result == target))
				return true;

			if (at.ResultCount == 2 && (at.Result2 == target || at.Result2 == target))
				return true;

			if (at.OperandCount == 0)
				continue;

			foreach (var operand in at.Operands)
			{
				if (operand == target)
					return false;
			}
		}
	}

	public static bool IsWithinHandler(Transform transform, Operand operand)
	{
		var manager = transform.GetManager<ExceptionHandlerOperandManager>();

		if (manager == null)
			return false;

		return manager.IsWithinHandler(operand);
	}
}
