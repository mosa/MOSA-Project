// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.BitValue;

[Transform()]
public sealed class DivSigned64ButNotSigned : BaseTransform
{
	public DivSigned64ButNotSigned() : base(IR.DivSigned64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 20;

	public override bool Match(Context context, Transform transform)
	{
		if (!IsBitValueSignBitCleared64(context.Operand1))
			return false;

		if (!IsBitValueSignBitCleared64(context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2;

		context.SetInstruction(IR.DivUnsigned64, result, t1, t2);
	}
}
