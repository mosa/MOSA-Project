// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.Simplification;

public sealed class Compare64x32SameAndNotEqual : BaseTransform
{
	public Compare64x32SameAndNotEqual() : base(IR.Compare64x32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		var condition = context.ConditionCode;

		if (!(context.ConditionCode is ConditionCode.NotEqual or ConditionCode.Greater or ConditionCode.Less or ConditionCode.UnsignedGreater or ConditionCode.UnsignedLess))
			return false;

		if (!AreSame(context.Operand1, context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var e1 = Operand.Constant32_0;

		context.SetInstruction(IR.Move32, result, e1);
	}
}
