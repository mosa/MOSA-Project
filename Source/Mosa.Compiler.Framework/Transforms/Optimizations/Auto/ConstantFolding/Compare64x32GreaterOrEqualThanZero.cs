// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding;

/// <summary>
/// Compare64x32GreaterOrEqualThanZero
/// </summary>
[Transform("IR.Optimizations.Auto.ConstantFolding")]
public sealed class Compare64x32GreaterOrEqualThanZero : BaseTransform
{
	public Compare64x32GreaterOrEqualThanZero() : base(IRInstruction.Compare64x32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, TransformContext transform)
	{
		if (context.ConditionCode != ConditionCode.UnsignedGreaterOrEqual)
			return false;

		if (!context.Operand2.IsResolvedConstant)
			return false;

		if (context.Operand2.ConstantUnsigned64 != 0)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var c1 = Operand.CreateConstant(1);

		context.SetInstruction(IRInstruction.Move32, result, c1);
	}
}
