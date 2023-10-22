// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Simplification;

public sealed class Branch32OnlyOneExit : BaseTransform
{
	public Branch32OnlyOneExit() : base(IRInstruction.Branch32, TransformType.Manual | TransformType.Optimization)
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
		var target = context.BranchTargets[0];

		context.SetNop();

		Framework.Transform.UpdatePhiBlock(target);
	}
}
