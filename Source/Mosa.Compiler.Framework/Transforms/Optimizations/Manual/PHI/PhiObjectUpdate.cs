// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Phi;

public sealed class PhiObjectUpdate : BasePhiTransform
{
	public PhiObjectUpdate() : base(IRInstruction.PhiObject, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return context.OperandCount != context.Block.PreviousBlocks.Count;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		UpdatePhi(context.Node);
	}
}
