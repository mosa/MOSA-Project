// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction;

public sealed class Or64Zero : BaseTransform
{
	public Or64Zero() : base(IR.Or64, TransformType.Auto | TransformType.Optimization, 80)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsConstantZero)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;

		context.SetInstruction(IR.Move64, result, t1);
	}
}

public sealed class Or64Zero_v1 : BaseTransform
{
	public Or64Zero_v1() : base(IR.Or64, TransformType.Auto | TransformType.Optimization, 80)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsConstantZero)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand2;

		context.SetInstruction(IR.Move64, result, t1);
	}
}
