// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.Rewrite;

/// <summary>
/// CompareObjectGreaterThanZero
/// </summary>
[Transform("IR.Optimizations.Auto.Rewrite")]
public sealed class CompareObjectGreaterThanZero : BaseTransform
{
	public CompareObjectGreaterThanZero() : base(IR.CompareObject, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.ConditionCode != ConditionCode.UnsignedGreater)
			return false;

		if (!IsZero(context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2;

		context.SetInstruction(IR.CompareObject, ConditionCode.NotEqual, result, t1, t2);
	}
}

/// <summary>
/// CompareObjectGreaterThanZero_v1
/// </summary>
[Transform("IR.Optimizations.Auto.Rewrite")]
public sealed class CompareObjectGreaterThanZero_v1 : BaseTransform
{
	public CompareObjectGreaterThanZero_v1() : base(IR.CompareObject, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.ConditionCode != ConditionCode.UnsignedLess)
			return false;

		if (!IsZero(context.Operand1))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2;

		context.SetInstruction(IR.CompareObject, ConditionCode.NotEqual, result, t2, t1);
	}
}
