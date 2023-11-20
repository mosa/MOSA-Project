// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Overwrite;

/// <summary>
/// Move64Overwrite
/// </summary>
public sealed class Move64Overwrite : BaseTransform
{
	public Move64Overwrite() : base(IR.Move64, TransformType.Manual | TransformType.Optimization | TransformType.Search)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Block.IsPrologue)
			return false;

		if (!context.Operand1.IsConstant)
			return false;

		if (context.Result.Uses.Count <= 1)
			return false;

		if (context.Result.IsDefinedOnce)
			return false;

		var targets = context.Result.Definitions;

		var uses = context.Result.Uses;
		var at = context.Node;

		while (true)
		{
			at = at.Next;

			if (at.IsEmptyOrNop)
				continue;

			if (at.IsBlockEndInstruction)
			{
				at = at.Block.NextBlocks[0].First;

				if (at.Block.NextBlocks.Count != 1)
					return false;

				continue;
			}

			if (uses.Contains(at))
				return false;

			if (targets.Contains(at))
				break;
		}

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetNop();
	}
}
