// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Phi;

public sealed class PhiManagedPointerUpdate : BasePhiTransform
{
	public PhiManagedPointerUpdate() : base(IRInstruction.PhiManagedPointer, TransformType.Manual | TransformType.Optimization)
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
