// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding;

[Transform("IR.Optimizations.Auto.ConstantFolding")]
public sealed class Compare64x32LessThanZero : BaseTransform
{
	public Compare64x32LessThanZero() : base(IR.Compare64x32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, Transform transform)
	{
		if (context.ConditionCode != ConditionCode.UnsignedGreater)
			return false;

		if (!context.Operand1.IsResolvedConstant)
			return false;

		if (context.Operand1.ConstantUnsigned64 != 0)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var c1 = Operand.CreateConstant(0);

		context.SetInstruction(IR.Move32, result, c1);
	}
}
