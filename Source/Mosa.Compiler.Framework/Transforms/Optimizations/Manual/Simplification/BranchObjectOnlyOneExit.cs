// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Simplification;

public sealed class BranchObjectOnlyOneExit : BaseTransform
{
	public BranchObjectOnlyOneExit() : base(IR.BranchObject, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.Block.NextBlocks.Count != 1)
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
