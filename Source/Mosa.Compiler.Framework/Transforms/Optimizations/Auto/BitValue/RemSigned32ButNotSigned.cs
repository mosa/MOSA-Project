// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.BitValue;

public sealed class RemSigned32ButNotSigned : BaseTransform
{
	public RemSigned32ButNotSigned() : base(IR.RemSigned32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 20;

	public override bool Match(Context context, Transform transform)
	{
		if (!IsBitValueSignBitCleared32(context.Operand1))
			return false;

		if (!IsBitValueSignBitCleared32(context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2;

		context.SetInstruction(IR.RemUnsigned32, result, t1, t2);
	}
}
