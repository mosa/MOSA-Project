// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Rewrite;

public sealed class Branch64GreaterThanZero : BaseTransform
{
	public Branch64GreaterThanZero() : base(IR.Branch64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.ConditionCode != ConditionCode.UnsignedGreater)
			return false;

		if (!IsZero(context.Operand1))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var target = context.BranchTarget1;

		context.SetNop();

		Framework.Transform.UpdatePhiBlock(target);
	}
}
