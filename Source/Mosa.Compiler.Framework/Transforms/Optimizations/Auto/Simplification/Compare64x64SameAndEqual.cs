// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.Simplification;

/// <summary>
/// Compare64x64SameAndEqual
/// </summary>
[Transform("IR.Optimizations.Auto.Simplification")]
public sealed class Compare64x64SameAndEqual : BaseTransform
{
	public Compare64x64SameAndEqual() : base(IR.Compare64x64, TransformType.Auto | TransformType.Optimization)
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

		var e1 = Operand.CreateConstant(To64(1));

		context.SetInstruction(IR.Move64, result, e1);
	}
}
