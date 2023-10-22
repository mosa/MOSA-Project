// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Rewrite;

public sealed class Branch64LessThanZero : BaseTransform
{
	public Branch64LessThanZero() : base(IRInstruction.Branch64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.ConditionCode != ConditionCode.UnsignedLess)
			return false;

		if (!IsZero(context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var target = context.BranchTargets[0];

		context.SetNop();

		Framework.Transform.UpdatePhiBlock(target);
	}
}
