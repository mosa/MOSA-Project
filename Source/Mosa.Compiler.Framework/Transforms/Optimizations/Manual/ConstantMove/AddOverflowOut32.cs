// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.ConstantMove;

/// <summary>
/// AddOverflowOut32
/// </summary>
public sealed class AddOverflowOut32 : BaseTransform
{
	public AddOverflowOut32() : base(IRInstruction.AddOverflowOut32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, Transform transform)
	{
		if (!IsResolvedConstant(context.Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		SwapOperands1And2(context);
	}
}
