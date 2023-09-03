// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Rewrite;

public sealed class Branch32GreaterThanZero : BaseTransform
{
	public Branch32GreaterThanZero() : base(IRInstruction.Branch32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (context.ConditionCode != ConditionCode.UnsignedGreater)
			return false;

		if (!IsZero(context.Operand1))
			return false;

		if (context.Block.NextBlocks.Count == 1)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var target = context.BranchTargets[0];

		context.SetNop();

		TransformContext.UpdatePhiBlock(target);
	}
}
