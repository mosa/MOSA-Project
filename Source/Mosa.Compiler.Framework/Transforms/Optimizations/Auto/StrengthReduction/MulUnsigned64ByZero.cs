// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction;

[Transform()]
public sealed class MulUnsigned64ByZero : BaseTransform
{
	public MulUnsigned64ByZero() : base(IR.MulUnsigned64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 80;

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsConstantZero)
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

[Transform()]
public sealed class MulUnsigned64ByZero_v1 : BaseTransform
{
	public MulUnsigned64ByZero_v1() : base(IR.MulUnsigned64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 80;

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsConstantZero)
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
