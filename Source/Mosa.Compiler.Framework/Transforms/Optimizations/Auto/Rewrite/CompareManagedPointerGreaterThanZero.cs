// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.Rewrite;

/// <summary>
/// CompareManagedPointerGreaterThanZero
/// </summary>
[Transform("IR.Optimizations.Auto.Rewrite")]
public sealed class CompareManagedPointerGreaterThanZero : BaseTransform
{
	public CompareManagedPointerGreaterThanZero() : base(IRInstruction.CompareManagedPointer, TransformType.Auto | TransformType.Optimization)
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

		context.SetInstruction(IRInstruction.CompareManagedPointer, ConditionCode.NotEqual, result, t1, t2);
	}
}

/// <summary>
/// CompareManagedPointerGreaterThanZero_v1
/// </summary>
[Transform("IR.Optimizations.Auto.Rewrite")]
public sealed class CompareManagedPointerGreaterThanZero_v1 : BaseTransform
{
	public CompareManagedPointerGreaterThanZero_v1() : base(IRInstruction.CompareManagedPointer, TransformType.Auto | TransformType.Optimization)
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

		context.SetInstruction(IRInstruction.CompareManagedPointer, ConditionCode.NotEqual, result, t2, t1);
	}
}
