// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.ConstantMove;

/// <summary>
/// MulCarryOut64
/// </summary>
public sealed class MulCarryOut64 : BaseTransform
{
	public MulCarryOut64() : base(IR.MulCarryOut64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!IsResolvedConstant(context.Operand1))
			return false;

		if (IsResolvedConstant(context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		SwapOperands1And2(context);
	}
}
