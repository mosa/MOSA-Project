// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.Simplification;

public sealed class CompareObjectSameAndEqual : BaseTransform
{
	public CompareObjectSameAndEqual() : base(IR.CompareObject, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		var condition = context.ConditionCode;

		if (!(context.ConditionCode is ConditionCode.Equal or ConditionCode.GreaterOrEqual or ConditionCode.LessOrEqual or ConditionCode.UnsignedGreaterOrEqual or ConditionCode.UnsignedLessOrEqual))
			return false;

		if (!AreSame(context.Operand1, context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var e1 = Operand.Constant32_1;

		context.SetInstruction(IR.Move32, result, e1);
	}
}
