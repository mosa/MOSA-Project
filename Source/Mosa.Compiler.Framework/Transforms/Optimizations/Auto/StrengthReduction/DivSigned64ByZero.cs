// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction;

public sealed class DivSigned64ByZero : BaseTransform
{
	public DivSigned64ByZero() : base(IR.DivSigned64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 80;

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsConstantZero)
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		if (IsZero(context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var e1 = Operand.Constant64_0;

		context.SetInstruction(IR.Move64, result, e1);
	}
}
