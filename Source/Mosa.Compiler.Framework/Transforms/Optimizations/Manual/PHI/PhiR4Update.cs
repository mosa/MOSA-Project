// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Phi;

public sealed class PhiR4Update : BaseTransform
{
	public PhiR4Update() : base(IRInstruction.Phi32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return context.OperandCount != context.Block.PreviousBlocks.Count;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		TransformContext.UpdatePhi(context.Node);
	}
}
